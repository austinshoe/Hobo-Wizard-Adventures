

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltController : MonoBehaviour
{
    private bool stopUpdate;
    private LineRenderer Bolt;
    private int posCount;
    public Material mat;
    public GameObject LightningSparkPrefab;
    public GameObject LightningSparkInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void CreateBolt(Vector3 startPos, Vector3 endPos, float size)
    {
        StartCoroutine(SpawnBolt(startPos, endPos, size));
        /*LineRenderer bolt = new LineRenderer();
        bolt.widthMultiplier = 0.1f;
        bolt.useWorldSpace = true;

        int segments = (int)Random.Range(Vector3.Distance(startPos, endPos) / 2, Vector3.Distance(startPos, endPos) / 0.8f);
        bolt.positionCount = segments + 1;

        bolt.SetPosition(0, startPos);
        bolt.SetPosition(segments, endPos);
        Vector3 dist = endPos - startPos;
        for (int i = 1; i < segments; i++)
        {
            Vector3 eqPos = startPos + dist / segments * i;
            Vector3 offset = new Vector3(Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f));
            bolt.SetPosition(i, eqPos + offset);

        }*/
    }

    IEnumerator SpawnBolt(Vector3 startPos, Vector3 endPos, float size)
    {
        LineRenderer bolt = gameObject.AddComponent<LineRenderer>();
        bolt.material = mat;
        bolt.widthMultiplier = size;
        bolt.useWorldSpace = true;
        int segments = (int)Random.Range(Vector3.Distance(startPos, endPos) / 2, Vector3.Distance(startPos, endPos) / 0.8f);
        //bolt.positionCount = segments + 1;
        bolt.positionCount = 1;
        bolt.SetPosition(0, startPos);
        yield return new WaitForSeconds(0.05f);
        Vector3 dist = endPos - startPos;
        for (int i = 1; i < segments; i++)
        {
            Vector3 eqPos = startPos + dist / segments * i;
            //Vector3 offset = new Vector3(Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f));
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            offset = offset.normalized * 0.5f;
            bolt.positionCount++;
            bolt.SetPosition(i, eqPos + offset);
            yield return new WaitForSeconds(0.05f);
        }
        bolt.positionCount++;
        bolt.SetPosition(segments, endPos);
        //doneSpawning.Add(true);
        Bolt = bolt;
        stopUpdate = false;
        StartCoroutine(BeginBoltUpdate(bolt));

    }

    public void CreateBolt(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(SpawnBolt(startPos, endPos));
        /*LineRenderer bolt = new LineRenderer();
        bolt.widthMultiplier = 0.1f;
        bolt.useWorldSpace = true;

        int segments = (int)Random.Range(Vector3.Distance(startPos, endPos) / 2, Vector3.Distance(startPos, endPos) / 0.8f);
        bolt.positionCount = segments + 1;

        bolt.SetPosition(0, startPos);
        bolt.SetPosition(segments, endPos);
        Vector3 dist = endPos - startPos;
        for (int i = 1; i < segments; i++)
        {
            Vector3 eqPos = startPos + dist / segments * i;
            Vector3 offset = new Vector3(Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f));
            bolt.SetPosition(i, eqPos + offset);

        }*/
    }
    IEnumerator SpawnBolt(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer bolt = gameObject.AddComponent<LineRenderer>();
        bolt.material = mat;
        bolt.widthMultiplier = 0.1f;
        bolt.useWorldSpace = true;
        int segments = (int)Random.Range(Vector3.Distance(startPos, endPos) / 2, Vector3.Distance(startPos, endPos) / 0.8f);
        //bolt.positionCount = segments + 1;
        bolt.positionCount = 1;
        bolt.SetPosition(0, startPos);
        yield return new WaitForSeconds(0.05f);
        Vector3 dist = endPos - startPos;
        for (int i = 1; i < segments; i++)
        {
            Vector3 eqPos = startPos + dist / segments * i;
            //Vector3 offset = new Vector3(Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f));
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            offset = offset.normalized * 0.5f;
            bolt.positionCount++;
            bolt.SetPosition(i, eqPos + offset);
            yield return new WaitForSeconds(0.05f);
        }
        bolt.positionCount++;
        bolt.SetPosition(segments, endPos);
        //doneSpawning.Add(true);
        Bolt = bolt;
        stopUpdate = false;
        StartCoroutine(BeginBoltUpdate(bolt));

    }

    public void CreateSparkVFX(Vector3 enemyPos)
    {
        LightningSparkInstance = Instantiate(LightningSparkPrefab, enemyPos, Quaternion.identity);
        LightningSparkInstance.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator BeginBoltUpdate(LineRenderer bolt)
    {
        posCount = bolt.positionCount;
        while (!stopUpdate)
        {
            Vector3 startPos = bolt.GetPosition(0);
            Vector3 endPos = bolt.GetPosition(bolt.positionCount - 1);
            int segments = (int)Random.Range(posCount - 2, posCount + 2);
            Vector3 dist = endPos - startPos;
            bolt.positionCount = segments;
            for (int j = 1; j < segments - 1; j++)
            {
                Vector3 eqPos = startPos + dist / segments * j;
                //Vector3 offset = new Vector3(Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f), Random.Range(-0.66f, 0.66f));
                Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                offset = offset.normalized * 0.5f;
                bolt.SetPosition(j, eqPos + offset);
                //yield return new WaitForSeconds(0.005f);
            }
            bolt.SetPosition(segments - 1, endPos);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void DestroyBolt()
    {
        stopUpdate = true;
        Destroy(Bolt, 0.02f);
        if (LightningSparkInstance != null)
        {
            LightningSparkInstance.GetComponent<ParticleSystem>().Stop();
            Destroy(LightningSparkInstance, 0.02f);
        }

    }
}
