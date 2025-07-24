
using System.Collections;
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
            }
        }
    }

    public void ContractPenta()
    {
        StartCoroutine(CombineColumns());
    }

    IEnumerator CombineColumns()
    {
        yield return 0.5f;
        
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
