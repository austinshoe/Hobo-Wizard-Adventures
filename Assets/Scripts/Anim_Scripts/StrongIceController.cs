using System.Collections;
using UnityEngine;

public class StrongIceController : MonoBehaviour
{
    public GameObject Hail;
    public GameObject Shard;
    public GameObject iceShardPrefab;
    void Awake()
    {
        Shard.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootShard(Vector3 endPos)
    {
        StartCoroutine(ShootBigShard(endPos));
    }

    IEnumerator ShootBigShard(Vector3 endPos)
    {
        Shard.transform.localScale = Vector3.zero;
        Vector3 initP = Shard.transform.position;
        endPos += Vector3.up * 0.6f;
        Shard.SetActive(true);
        float t = 0;
        Hail.GetComponent<ParticleSystem>().Stop();
        while (t < 0.25f)
        {
            float normalizedT = t / 0.25f;
            Shard.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 25, normalizedT);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < 0.125f)
        {
            float normalizedT = t / 0.125f;
            Shard.transform.position = Vector3.Lerp(initP, endPos, normalizedT);
            t += Time.deltaTime;
            yield return null;
        }

        Renderer rend = Shard.GetComponent<Renderer>();
        Material mat = rend.material;
        Color emissionCol = mat.GetColor("_EmissionColor");
        float redStep = emissionCol.r / 10;
        float greenStep = emissionCol.g / 10;
        float blueStep = emissionCol.b / 10;
        for (int i = 0; i < 10; i++)
        {
            Color emissionColor = mat.GetColor("_EmissionColor");
            emissionColor.r = Mathf.Clamp(emissionColor.r, 0f, 1f);
            emissionColor.g = Mathf.Clamp(emissionColor.g, 0f, 1f);
            emissionColor.b = Mathf.Clamp(emissionColor.b, 0f, 1f);
            emissionColor.r -= redStep;
            emissionColor.g -= greenStep;
            emissionColor.b -= blueStep;
            mat.SetColor("_EmissionColor", emissionColor);
            yield return new WaitForSeconds(0.5f / 10);
        }

    }

    public void DestroyIce()
    {
        GameObject ShardEff = Instantiate(iceShardPrefab, Shard.transform.position, Quaternion.identity);
        ShardEff.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DieInAHoleBrochacho());
    }
    
    IEnumerator DieInAHoleBrochacho()
    {
        yield return new WaitForSeconds(0.125f);
        Destroy(gameObject);
    }
}
