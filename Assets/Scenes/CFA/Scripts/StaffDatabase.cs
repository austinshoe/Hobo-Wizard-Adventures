using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Crafting/Staff Database")]
public class StaffDatabase : ScriptableObject
{
    public List<StaffPart> allParts;

    public List<StaffPart> GetPartsOfType(StaffPartType type)
    {
        return allParts.FindAll(p => p.partType == type);
    }

    public StaffPart GetPartByName(string name)
    {
        foreach (StaffPart part in allParts)
        {
            if (part.partName == name)
            {
                return part;
            }
        }
        return null;
    }
}