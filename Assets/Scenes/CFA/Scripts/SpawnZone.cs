using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Spawns/Zone")]
public class SpawnZone : MonoBehaviour //ScriptableObject
{
    public float minX, maxX, minY, maxY, minZ, maxZ; // prism boundaries
    public GameObject mobPrefab;
    public int maxMobs;
    public int currentMobs = 0;
    public List<GameObject> mobs;
    public List<GameObject> actualMobs;
}