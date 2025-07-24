
using System.Collections;
using System.Data;
using UnityEngine;

public class FireColumnController : MonoBehaviour
{
    bool isClone = false;
    public GameObject FlameEmitter;
    public GameObject EmberEmitter;
    private GameObject[] FireColumns;
    void Awake()
    {
        SetEmittion(false);
        // FlameEmitter.transform.localScale = Vector3.one * 2;
        // EmberEmitter.transform.localScale = Vector3.one * 2;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPenta(Vector3 pos, float radius)
    {
        FireColumns = new GameObject[5];
        if (!isClone)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 spawnPos = pos + Quaternion.Euler(0f, i * 72f, 0f) * Vector3.forward * radius;
                GameObject smallCol = Instantiate(gameObject, spawnPos, Quaternion.identity);
                smallCol.GetComponent<FireColumnController>().SetClone(true);
                smallCol.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.one * 0.5f;
                smallCol.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = Vector3.one * 0.5f;
                smallCol.GetComponent<FireColumnController>().SetEmittion(true);
                FireColumns[i] = smallCol;
            }
        }
    }

    public void ContractPenta()
    {
        StartCoroutine(CombineColumns());
    }

    IEnumerator CombineColumns()
    {
        Vector3[] colPos = new Vector3[5];
        for (int i = 0; i < 5; i++)
        {
            colPos[i] = FireColumns[i].transform.position;
        }
        float t = 0f;
        while (t < 0.5f)
        {
            float normT = t / 0.5f;
            for (int i = 0; i < 5; i++)
            {
                FireColumns[i].transform.position = Vector3.Lerp(colPos[i], transform.position, normT);
            }
            t += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < 5; i++)
        {
            FireColumns[i].transform.position = transform.position;
        }

        yield return new WaitForSeconds(0.5f);
        t = 0f;
        while (t < 0.25f)
        {
            float normT = (0.25f - t) / 0.25f;
            normT *= 0.5f;
            for (int i = 0; i < 5; i++)
            {
                FireColumns[i].GetComponent<FireColumnController>().EmberEmitter.transform.localScale = Vector3.one * normT;
                FireColumns[i].GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.one * normT;
            }
            t += Time.deltaTime;
            yield return null;

        }
        for (int i = 0; i < 5; i++)
        {
            FireColumns[i].GetComponent<FireColumnController>().EmberEmitter.transform.localScale = Vector3.zero;
            FireColumns[i].GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.zero;
            Destroy(FireColumns[i]);
        }
        yield return new WaitForSeconds(1f);
        FlameEmitter.transform.localScale = Vector3.one * 2;
        EmberEmitter.transform.localScale = Vector3.one * 2;
        SetEmittion(true);

    }

    public void SetClone(bool clone)
    {
        isClone = clone;
    }

    public void SetEmittion(bool on)
    {
        if (on)
        {
            FlameEmitter.SetActive(true);
            EmberEmitter.SetActive(true);
        }
        else
        {
            FlameEmitter.SetActive(false);
            EmberEmitter.SetActive(false);
        }
    }
}
