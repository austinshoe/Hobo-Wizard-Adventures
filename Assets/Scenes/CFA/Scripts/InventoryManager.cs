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
        craftedStaffs.Add(new Staff(staffDatabase.GetPartByName("Wood_Staff"),
                                    staffDatabase.GetPartByName("Purple_Quartz"),
                                    null, null, null, null));
        currentStaff = craftedStaffs[0];
        CustomationManager.instance.UpdateModel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
