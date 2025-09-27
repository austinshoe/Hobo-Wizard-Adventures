using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance;
    public List<StaffPart> inventory = new List<StaffPart>();
    public List<Staff> craftedStaffs = new List<Staff>();
    public Staff currentStaff;
    public StaffDatabase staffDatabase;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = new List<StaffPart>();
        craftedStaffs = new List<Staff>();
        inventory.Add(staffDatabase.GetPartByName("Wood_Staff"));
        inventory.Add(staffDatabase.GetPartByName("Purple_Quartz"));
        inventory.Add(staffDatabase.GetPartByName("Golden_Bubble"));
        //craftedStaffs.Add(new Staff(staffDatabase.GetPartByName("Wood_Staff"),
        //                            staffDatabase.GetPartByName("Purple_Quartz"),
        //                            null, null, null, null));
        //currentStaff = craftedStaffs[0];
        currentStaff = null;
        //CustomationManager.instance.UpdateModel();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public StaffPart[] GetAllPartsOfType(StaffPartType type)
    {
        List<StaffPart> partsOfType = new List<StaffPart>();
        foreach (StaffPart part in inventory)
        {
            if (part.partType == type)
            {
                partsOfType.Add(part);
            }
        }
        return partsOfType.ToArray();
    }
}
