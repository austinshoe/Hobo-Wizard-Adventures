using System.Collections;
using UnityEngine;

public class Orange_Top_Level : MonoBehaviour
{
    public GameObject[] seeds;
    public GameObject[] slices;
    public GameObject peel;

    private float rotateSpeed = 0.0f;
    private float rotateAccel = 90.0f;

    void Start()
    {
        peel.SetActive(false);
        foreach (GameObject slice in slices)
        {
            slice.SetActive(false);
        }
        foreach (GameObject seed in seeds)
        {
            seed.transform.localScale = Vector3.zero;
            float angle = -seed.transform.localEulerAngles.y;
            seed.transform.localPosition = new Vector3(8.5f / 2.0f *Mathf.Sin(angle * Mathf.Deg2Rad), 0, 8.5f / 2.0f * Mathf.Cos(angle * Mathf.Deg2Rad));
        }
        StartCoroutine(OrangeAnim());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

    }

    IEnumerator OrangeAnim()
    {
        float t = 0.0f;
        StartCoroutine(SpawnSeedsIn());
        while (t < 1f)
        {
            rotateSpeed += rotateAccel * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator SpawnSeedsIn()
    {
        foreach (GameObject seed in seeds)
        {
            float t = 0.0f;
            while (t < 0.125f)
            {
                seed.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / 0.125f);
                t += Time.deltaTime;
                yield return null;
            }
            seed.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(Random.Range(0.125f, 0.5f));
        }
        yield return new WaitForSeconds(0.5f);
        
    }
}
