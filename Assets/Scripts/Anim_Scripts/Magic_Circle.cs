using System.Collections;
using UnityEngine;

public class Magic_Circle : MonoBehaviour
{
    public GameObject[] rings;
    public GameObject particlePrefab;
    private float[] speeds;

    public GameObject iceShardPrefab;

    void Awake()
    {
        particlePrefab.SetActive(false); // Ensure the particle prefab is inactive initially
        // Initialize the rings array with the child GameObjects
        rings = new GameObject[transform.childCount - 1];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            rings[i] = transform.GetChild(i).gameObject;
            rings[i].transform.localScale = Vector3.zero; // Set initial scale to zero
        }
        speeds = new float[rings.Length];
        for (int i = 0; i < rings.Length; i++)
        {
            if (i % 2 == 0)
            {
                speeds[i] = 60f * Mathf.Sqrt(rings.Length - 1 - i);
            }
            else
            {
                speeds[i] = 60f * -Mathf.Sqrt(rings.Length - 1 - i);
            }
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RingSpawnAnim());
        StartCoroutine(ParticleSystemActivate());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rings.Length; i++)
        {
            // Rotate each ring around the Y-axis
            rings[i].transform.Rotate(Vector3.forward, Time.deltaTime * speeds[i]);
        }

    }


    IEnumerator RingSpawnAnim()
    {
        for (int i = 0; i < rings.Length; i++)
        {
            GameObject ring = rings[i];
            float t = 0f;
            while (t < 0.25f)
            {
                ring.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / 0.25f);
                t += Time.deltaTime;
                yield return null;
            }
            ring.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.125f);
        }
    }

    IEnumerator ParticleSystemActivate()
    {

        yield return new WaitForSeconds(0.125f);
        particlePrefab.SetActive(true);
        particlePrefab.GetComponent<ParticleSystem>().Play();

    }
    
    public void DestroyCircle()
    {
        Vector3 createPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GameObject ShardEff = Instantiate(iceShardPrefab, transform.position, Quaternion.identity);
        ShardEff.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DieInAHoleBrochacho());
    }
    
    IEnumerator DieInAHoleBrochacho()
    {
        yield return new WaitForSeconds(0.125f);
        Destroy(gameObject);
    }
}
