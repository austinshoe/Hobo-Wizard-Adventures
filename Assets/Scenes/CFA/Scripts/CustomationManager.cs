using UnityEngine;

public class CustomationManager : MonoBehaviour
{
    public static CustomationManager instance;
    public GameObject playerModel;
    public GameObject BaseSocket;
    public GameObject HeadSocket;
    public GameObject HiltSocket;
    public GameObject currentBase;
    public GameObject currentHead;
    public GameObject currentHilt;
    public GameObject currentBauble1;
    public GameObject currentBauble2;
    public GameObject currentHiltBauble1;
    public GameObject currentHiltBauble2;

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
        /*currentHead = null;
        currentBase = null;
        currentHilt = null;
        currentBauble1 = null;
        currentBauble2 = null;
        currentHiltBauble1 = null;
        currentHiltBauble2 = null;*/
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateModel()
    {
        if (currentBase != null)
        {
            Destroy(currentBase);
        }
        if (InventoryManager.instance.currentStaff.baseMaterial != null)
        {
            currentBase = Instantiate(InventoryManager.instance.currentStaff.baseMaterial.prefab, BaseSocket.transform);
        }

        if (currentHead != null)
        {
            Destroy(currentHead);
        }
        if (InventoryManager.instance.currentStaff.headMaterial != null)
        {
            currentHead = Instantiate(InventoryManager.instance.currentStaff.headMaterial.prefab, HeadSocket.transform);
        }

        if (currentBauble1 != null)
        {
            Destroy(currentBauble1);
        }
        if (InventoryManager.instance.currentStaff.baubleMaterial1 != null)
        {
            currentBauble1 = Instantiate(InventoryManager.instance.currentStaff.baubleMaterial1.prefab, HeadSocket.transform);
        }

        if (currentBauble2 != null)
        {
            Destroy(currentBauble2);
        }
        if (InventoryManager.instance.currentStaff.baubleMaterial2 != null)
        {
            currentBauble2 = Instantiate(InventoryManager.instance.currentStaff.baubleMaterial2.prefab, HeadSocket.transform);
        }

        if (currentHiltBauble1 != null)
        {
            Destroy(currentHiltBauble1);
        }
        if (InventoryManager.instance.currentStaff.hiltBaubleMaterial != null)
        {
            currentHiltBauble1 = Instantiate(InventoryManager.instance.currentStaff.hiltBaubleMaterial.prefab, HiltSocket.transform);
        }

        if (currentHiltBauble2 != null)
        {
            Destroy(currentHiltBauble2);
        }
        if (InventoryManager.instance.currentStaff.hiltBaubleMaterial2 != null)
        {
            currentHiltBauble2 = Instantiate(InventoryManager.instance.currentStaff.hiltBaubleMaterial2.prefab, HiltSocket.transform);
        }
    }
}
