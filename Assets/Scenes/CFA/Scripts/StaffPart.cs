using UnityEngine;

public enum StaffPartType
{
    Base,
    Head,
    Bauble,
    HiltBauble
}

[CreateAssetMenu(menuName = "Crafting/Staff Part")]
public class StaffPart : ScriptableObject
{
    public string partName;              // e.g. "Wood Staff", "Purple Quartz"
    public StaffPartType partType;       // Which slot this goes in
    public GameObject prefab;            // The 3D model prefab
}