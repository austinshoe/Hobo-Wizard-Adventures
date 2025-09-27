using UnityEngine;
using UnityEngine.UI;

public enum StaffPartType
{
    Base,
    Head,
    Bauble,
    HiltBauble,
    Hilt
}

[CreateAssetMenu(menuName = "Crafting/Staff Part")]
public class StaffPart : ScriptableObject
{
    public string partName;              // e.g. "Wood Staff", "Purple Quartz"
    public StaffPartType partType;       // Which slot this goes in
    public GameObject prefab;            // The 3D model prefab
    public Sprite icon;                // The icon to display in inventory
}