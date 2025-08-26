float pcg_hash3(uint3 p)
{
    const uint M = 1664525u;
    const uint C = 1013904223u;
    
    uint3 state = p * M + C;
    state = state ^ (state >> 16u);
    state *= M;
    state += C;
    state = state ^ (state >> 16u);
    
    uint hash = state.x ^ state.y ^ state.z;
    
    return float(hash) / 4294967296.0;
}

struct VoronoiOutput {
    float distance;
    float3 color;
    float3 position;
};

struct VoronoiParams {
    float randomness;
    float smoothness;
    float metric; // 0=Euclidean, 1=Manhattan, 2=Chebychev
    float scale;
};

float3 hash_int3_to_float3(int3 p) {
    uint3 q = uint3(p) * uint3(1597334673u, 3812015801u, 2912667907u);
    q = (q.x ^ q.y ^ q.z) * uint3(1597334673u, 3812015801u, 2912667907u);
    return float3(q) / float(0xFFFFFFFFu);
}

float voronoi_distance(float3 a, float3 b, VoronoiParams params) {
    float3 d = abs(a - b);
    
    if (params.metric == 0.0) {       // Euclidean
        return length(d);
    }
    else if (params.metric == 1.0) {  // Manhattan
        return d.x + d.y + d.z;
    }
    else {                            // Chebychev
        return max(max(d.x, d.y), d.z);
    }
}

VoronoiOutput voronoi_smooth_f1(VoronoiParams params, float3 coord) {
    coord *= params.scale;
    
    const float3 cellPosition_f = floor(coord);
    const float3 localPosition = coord - cellPosition_f;
    const int3 cellPosition = int3(cellPosition_f);

    float smoothDistance = 0.0;
    float3 smoothColor = float3(0.0, 0.0, 0.0);
    float3 smoothPosition = float3(0.0, 0.0, 0.0);
    float h = -1.0;
    
    [unroll]
    for (int k = -2; k <= 2; k++) {
        [unroll]
        for (int j = -2; j <= 2; j++) {
            [unroll]
            for (int i = -2; i <= 2; i++) {
                const int3 cellOffset = int3(i, j, k);
                const float3 pointPosition = float3(cellOffset) + 
                                           hash_int3_to_float3(cellPosition + cellOffset) * 
                                           params.randomness;
                const float distanceToPoint = voronoi_distance(pointPosition, localPosition, params);
                
                h = h == -1.0 ? 
                    1.0 : 
                    smoothstep(0.0, 1.0, 0.5 + 0.5 * (smoothDistance - distanceToPoint) / params.smoothness);
                
                float correctionFactor = params.smoothness * h * (1.0 - h);
                smoothDistance = lerp(smoothDistance, distanceToPoint, h) - correctionFactor;
                correctionFactor /= 1.0 + 3.0 * params.smoothness;
                
                const float3 cellColor = hash_int3_to_float3(cellPosition + cellOffset);
                smoothColor = lerp(smoothColor, cellColor, h) - correctionFactor;
                smoothPosition = lerp(smoothPosition, pointPosition, h) - correctionFactor;
            }
        }
    }

    VoronoiOutput octave;
    octave.distance = smoothDistance / params.scale;
    octave.color = smoothColor;
    octave.position = (cellPosition_f + smoothPosition) / params.scale;
    return octave;
}

float voronoi_distance_fast(float3 a, float3 b, VoronoiParams params)
{
    float3 diff = a - b;
    return dot(diff, diff);
}

float3 hash_int3_to_float3_optimized(int3 cell)
{
    uint3 p = uint3(cell);
    p = p * 1664525u + 1013904223u;  
    p.x += p.y*p.z;
    p.y += p.z*p.x;
    p.z += p.x*p.y;
    p ^= p >> 16u;
    p = p * 1664525u + 1013904223u;
    return float3(p & uint3(0x00FFFFFFu, 0x00FFFFFFu, 0x00FFFFFFu)) / float(0x00FFFFFFu);
}

VoronoiOutput voronoi_simple_f1(VoronoiParams params, float3 coord)
{
    coord *= params.scale;

    float3 cellPosition_f = floor(coord);
    float3 localPosition = coord - cellPosition_f;
    int3 cellPosition = int3(cellPosition_f);
    
    int3 targetOffset = int3(0, 0, 0);
    float3 pointPosition = hash_int3_to_float3(cellPosition) * params.randomness;
    float minDistance = voronoi_distance_fast(pointPosition, localPosition, params);
    float3 targetPosition = pointPosition;

    for (int k = -1; k <= 1; k++) {
        for (int j = -1; j <= 1; j++) {
            //manual unroll
            {
                int i = -1;
                int3 cellOffset = int3(i, j, k);
                pointPosition = float3(cellOffset) + 
                               hash_int3_to_float3(cellPosition + cellOffset) * 
                               params.randomness;
                float distanceToPoint = voronoi_distance_fast(pointPosition, localPosition, params);
                if (distanceToPoint < minDistance) {
                    targetOffset = cellOffset;
                    minDistance = distanceToPoint;
                    targetPosition = pointPosition;
                }
            }
            {
                int i = 0;
                int3 cellOffset = int3(i, j, k);
                pointPosition = float3(cellOffset) + 
                               hash_int3_to_float3(cellPosition + cellOffset) * 
                               params.randomness;
                float distanceToPoint = voronoi_distance_fast(pointPosition, localPosition, params);
                if (distanceToPoint < minDistance) {
                    targetOffset = cellOffset;
                    minDistance = distanceToPoint;
                    targetPosition = pointPosition;
                }
            }
            {
                int i = 1;
                int3 cellOffset = int3(i, j, k);
                pointPosition = float3(cellOffset) + 
                               hash_int3_to_float3(cellPosition + cellOffset) * 
                               params.randomness;
                float distanceToPoint = voronoi_distance_fast(pointPosition, localPosition, params);
                if (distanceToPoint < minDistance) {
                    targetOffset = cellOffset;
                    minDistance = distanceToPoint;
                    targetPosition = pointPosition;
                }
            }
        }
    }

    VoronoiOutput octave;
    octave.distance = minDistance / params.scale;
    octave.color = hash_int3_to_float3(cellPosition + targetOffset);
    octave.position = (targetPosition + cellPosition_f) / params.scale;
    return octave;
}

void PainterlyNormalsSimple_float(float3 normalOS, float randomness, float scale, out float3 normal, out float3 color, out float distance) {
    VoronoiParams params;
    params.randomness = randomness;
    params.scale = scale;

    VoronoiOutput result = voronoi_simple_f1(params, normalOS);
    normal = result.position;
    color = result.color;
    distance = result.distance;
}

void PainterlyNormalsSimple_half(half3 normalOS, half randomness, half scale, out half3 normal, out half3 color, out half distance) {
    VoronoiParams params;
    params.randomness = randomness;
    params.scale = scale;

    VoronoiOutput result = voronoi_simple_f1(params, normalOS);
    normal = result.position;
    color = result.color;
    distance = result.distance;
}

void PainterlyNormals_float(float3 normalOS, float randomness, float smoothness, float metric, float scale, out float3 normal, out float3 color, out float distance) {
    VoronoiParams params;
    params.randomness = randomness;
    params.smoothness = smoothness;
    params.metric = metric;
    params.scale = scale;

    VoronoiOutput result = voronoi_smooth_f1(params, normalOS);
    normal = result.position;
    color = result.color;
    distance = result.distance;
}

void PainterlyNormals_half(half3 normalOS, half randomness, half smoothness, half metric, half scale, out half3 normal, out half3 color, out half distance) {
    VoronoiParams params;
    params.randomness = randomness;
    params.smoothness = smoothness;
    params.metric = metric;
    params.scale = scale;

    VoronoiOutput result = voronoi_smooth_f1(params, normalOS);
    normal = result.position;
    color = result.color;
    distance = result.distance;
}