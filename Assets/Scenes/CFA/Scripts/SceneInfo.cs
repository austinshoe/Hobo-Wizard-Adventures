using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public float waterlevel;
    public List<GameObject> spawnZones;
    public bool archerfishHostile = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject spzn in spawnZones)
        {
            SpawnZone spawnZone = spzn.GetComponent<SpawnZone>();
            for (int i = 0; i < spawnZone.maxMobs; i++)
            {
                Vector3 SpawnPos = new Vector3(Random.Range(spawnZone.minX, spawnZone.maxX),
                Random.Range(spawnZone.minY, spawnZone.maxY), Random.Range(spawnZone.minZ, spawnZone.maxZ));
                GameObject obj = Instantiate(spawnZone.mobPrefab, SpawnPos, Quaternion.Euler(0, 0, 0));
                spawnZone.currentMobs++;
                spawnZone.mobs.Add(obj);
                GameObject actualMob = obj.transform.GetChild(0).gameObject;
                actualMob.GetComponent<MobMovement>().sceneInfo = gameObject;
                actualMob.GetComponent<MobMovement>().spawnZone = spawnZone;
                actualMob.GetComponent<MobMovement>().movementSpeed = 25f;
                spawnZone.actualMobs.Add(actualMob);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
