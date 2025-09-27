using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public GameObject FadePanel;
    public RenderTexture rt;

    //Stores the current staff being crafted
    public StaffPart baseMaterial;
    public StaffPart headMaterial;
    public StaffPart baubleMaterial1;
    public StaffPart baubleMaterial2;
    public StaffPart hiltMaterial;
    public StaffPart hiltBaubleMaterial;
    public StaffPart hiltBaubleMaterial2;

    //Sockets for displaying the staff parts in the crafting scene
    public GameObject BaseSocket;
    public GameObject HeadSocket;
    public GameObject HiltSocket;
    public GameObject Bauble1Socket;
    public GameObject Bauble2Socket;
    public GameObject HiltBaubleSocket;
    public GameObject HiltBauble2Socket;

    //Main Crafting interface display
    public GameObject baseMaterialDisplay;
    public GameObject headMaterialDisplay;
    public GameObject baubleMaterial1Display;
    public GameObject baubleMaterial2Display;
    public GameObject hiltMaterialDisplay;
    public GameObject hiltBaubleMaterialDisplay;
    public GameObject hiltBaubleMaterial2Display;

    // Material Selection Scroll View
    public GameObject MaterialSelectionPanel;
    public TMP_Text MaterialSelectionTitle;
    public GameObject MaterialSelectionContent;
    public GameObject MaterialButtonPrefab;

    public Sprite CONST_TRANSPARENT;
    private bool canInteract;
    private bool onMaterialSelection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        canInteract = false;
        onMaterialSelection = false;
        FadePanel.SetActive(true);
        Color color = FadePanel.GetComponent<Image>().color;
        color.a = 1f;
        FadePanel.GetComponent<Image>().color = color;
        MaterialSelectionPanel.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(PanelActivitiesGlobal.FadeOutPanel(FadePanel, 1f));
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveStaff()
    {
        if (canInteract)
        {
            canInteract = false;
            if (baseMaterial == null || headMaterial == null)
            {
                Debug.Log("Cannot craft staff without base or head material!");
                canInteract = true;
                return;
            }
            InventoryManager.instance.inventory.Remove(baseMaterial);
            InventoryManager.instance.inventory.Remove(headMaterial);
            if (baubleMaterial1 != null) InventoryManager.instance.inventory.Remove(baubleMaterial1);
            if (baubleMaterial2 != null) InventoryManager.instance.inventory.Remove(baubleMaterial2);
            if (hiltBaubleMaterial != null) InventoryManager.instance.inventory.Remove(hiltBaubleMaterial);
            if (hiltBaubleMaterial2 != null) InventoryManager.instance.inventory.Remove(hiltBaubleMaterial2);
            Staff newStaff = new Staff(baseMaterial, headMaterial, baubleMaterial1, baubleMaterial2, hiltBaubleMaterial, hiltBaubleMaterial2, hiltMaterial);
            InventoryManager.instance.craftedStaffs.Add(newStaff);
            //RawImage icon = CreateIcon();
            //newStaff.AssignIcon(icon);
            //Sprite iconSprite = CaptureIcon(rt);
            //newStaff.AssignIcon(iconSprite);
            Texture2D iconTex = SaveRenderTextureToTexture2D(rt);
            newStaff.AssignIcon(iconTex);
            InventoryManager.instance.currentStaff = newStaff;
            SceneManager.LoadScene("Player Customization");
        }
    }

     public void Quit()
    {
        if (canInteract)
        {
            canInteract = false;
            SceneManager.LoadScene("Player Customization");
        }
    }

    public RawImage CreateIcon()
    {
        RawImage icon = new GameObject("StaffIcon").AddComponent<RawImage>();
        icon.texture = rt;
        return icon;
    }
    public Sprite CaptureIcon(RenderTexture rt)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public Texture2D SaveRenderTextureToTexture2D(RenderTexture rt)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false, false);

        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentRT;
        return tex;
    }

    public void ExitMaterialSelection()
    {
        if (onMaterialSelection)
        {
            MaterialSelectionPanel.SetActive(false);
            onMaterialSelection = false;
            canInteract = true;
        }

    }

    public void SelectHeadMaterial()
    {
        if (!canInteract && !onMaterialSelection)
        {
            return;
        }
        foreach (Transform child in MaterialSelectionContent.transform)
        {
            Destroy(child.gameObject);
        }
        onMaterialSelection = true;
        MaterialSelectionTitle.text = "Head Materials";
        StaffPart[] headMaterials = InventoryManager.instance.GetAllPartsOfType(StaffPartType.Head);
        foreach (StaffPart part in headMaterials)
        {
            GameObject buttonObj = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
            buttonObj.GetComponentInChildren<TMP_Text>().text = part.partName;
            Transform panelTransform = buttonObj.transform.Find("MaterialIcon");
            GameObject panel = panelTransform.gameObject;
            panel.GetComponent<Image>().sprite = part.icon;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectedHeadMaterial(part));
        }
        GameObject removeButton = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
        removeButton.GetComponentInChildren<TMP_Text>().text = "None";
        removeButton.GetComponent<Button>().onClick.AddListener(() => SelectedHeadMaterial(null));
        MaterialSelectionPanel.SetActive(true);
    }

    public void SelectedHeadMaterial(StaffPart part)
    {
        headMaterial = part;
        RefreshDisplayHead();
        return;
    }

    public void RefreshDisplay()
    {
        RefreshDisplayHead();
        RefreshDisplayBase();
        RefreshDisplayBauble1();
        //RefreshDisplayBauble2();
        // RefreshDisplayHilt();
        // RefreshDisplayHiltBauble1();
        // RefreshDisplayHiltBauble2();
    }

    public void RefreshDisplayHead()
    {
        foreach (Transform child in HeadSocket.transform)
        {
            Destroy(child.gameObject);
        }
        headMaterialDisplay.GetComponent<Image>().sprite = CONST_TRANSPARENT;
        if (headMaterial == null)
        {
            return;
        }
        GameObject headObj = Instantiate(headMaterial.prefab, HeadSocket.transform);
        headMaterialDisplay.GetComponent<Image>().sprite = headMaterial.icon;
    }


    public void SelectBaseMaterial()
    {
        if (!canInteract && !onMaterialSelection)
        {
            return;
        }
        foreach (Transform child in MaterialSelectionContent.transform)
        {
            Destroy(child.gameObject);
        }
        onMaterialSelection = true;
        MaterialSelectionTitle.text = "Base Materials";
        StaffPart[] baseMaterials = InventoryManager.instance.GetAllPartsOfType(StaffPartType.Base);
        foreach (StaffPart part in baseMaterials)
        {
            GameObject buttonObj = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
            buttonObj.GetComponentInChildren<TMP_Text>().text = part.partName;
            Transform panelTransform = buttonObj.transform.Find("MaterialIcon");
            GameObject panel = panelTransform.gameObject;
            panel.GetComponent<Image>().sprite = part.icon;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectedBaseMaterial(part));
        }
        GameObject removeButton = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
        removeButton.GetComponentInChildren<TMP_Text>().text = "None";
        removeButton.GetComponent<Button>().onClick.AddListener(() => SelectedBaseMaterial(null));
        MaterialSelectionPanel.SetActive(true);
    }

    public void SelectedBaseMaterial(StaffPart part)
    {
        baseMaterial = part;
        RefreshDisplayBase();
        return;
    }


    public void RefreshDisplayBase()
    {
        foreach (Transform child in BaseSocket.transform)
        {
            Destroy(child.gameObject);
        }
        baseMaterialDisplay.GetComponent<Image>().sprite = CONST_TRANSPARENT;
        if (baseMaterial == null)
        {
            return;
        }
        GameObject baseObj = Instantiate(baseMaterial.prefab, BaseSocket.transform);
        baseMaterialDisplay.GetComponent<Image>().sprite = baseMaterial.icon;
    }


    public void SelectBaubleMaterial()
    {
        if (!canInteract && !onMaterialSelection)
        {
            return;
        }
        foreach (Transform child in MaterialSelectionContent.transform)
        {
            Destroy(child.gameObject);
        }
        onMaterialSelection = true;
        MaterialSelectionTitle.text = "Bauble Materials";
        StaffPart[] baubleMaterials = InventoryManager.instance.GetAllPartsOfType(StaffPartType.Bauble);
        foreach (StaffPart part in baubleMaterials)
        {
            GameObject buttonObj = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
            buttonObj.GetComponentInChildren<TMP_Text>().text = part.partName;
            Transform panelTransform = buttonObj.transform.Find("MaterialIcon");
            GameObject panel = panelTransform.gameObject;
            panel.GetComponent<Image>().sprite = part.icon;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectedBaubleMaterial(part));
        }
        GameObject removeButton = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
        removeButton.GetComponentInChildren<TMP_Text>().text = "None";
        removeButton.GetComponent<Button>().onClick.AddListener(() => SelectedBaubleMaterial(null));
        MaterialSelectionPanel.SetActive(true);
    }

    public void SelectedBaubleMaterial(StaffPart part)
    {
        if (part == baubleMaterial2 && part != null)
        {
            return;
        }
        baubleMaterial1 = part;
        RefreshDisplayBauble1();
        return;
    }


    public void RefreshDisplayBauble1()
    {
        foreach (Transform child in Bauble1Socket.transform)
        {
            Destroy(child.gameObject);
        }
        baubleMaterial1Display.GetComponent<Image>().sprite = CONST_TRANSPARENT;
        if (baubleMaterial1 == null)
        {
            return;
        }
        GameObject baubleObj = Instantiate(baubleMaterial1.prefab, Bauble1Socket.transform);
        baubleMaterial1Display.GetComponent<Image>().sprite = baubleMaterial1.icon;
    }
    

    public void SelectBaubleMaterial2()
    {
        if (!canInteract && !onMaterialSelection)
        {
            return;
        }
        foreach (Transform child in MaterialSelectionContent.transform)
        {
            Destroy(child.gameObject);
        }
        onMaterialSelection = true;
        MaterialSelectionTitle.text = "Bauble Materials";
        StaffPart[] baubleMaterials = InventoryManager.instance.GetAllPartsOfType(StaffPartType.Bauble);
        foreach (StaffPart part in baubleMaterials)
        {
            GameObject buttonObj = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
            buttonObj.GetComponentInChildren<TMP_Text>().text = part.partName;
            Transform panelTransform = buttonObj.transform.Find("MaterialIcon");
            GameObject panel = panelTransform.gameObject;
            panel.GetComponent<Image>().sprite = part.icon;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectedBaubleMaterial2(part));
        }
        GameObject removeButton = Instantiate(MaterialButtonPrefab, MaterialSelectionContent.transform);
        removeButton.GetComponentInChildren<TMP_Text>().text = "None";
        removeButton.GetComponent<Button>().onClick.AddListener(() => SelectedBaubleMaterial2(null));
        MaterialSelectionPanel.SetActive(true);
    }

    public void SelectedBaubleMaterial2(StaffPart part)
    {
        if (part == baubleMaterial1 && part != null)
        {
            return;
        }
        baubleMaterial2 = part;
        RefreshDisplayBauble2();
        return;
    }


    public void RefreshDisplayBauble2()
    {
        foreach (Transform child in Bauble2Socket.transform)
        {
            Destroy(child.gameObject);
        }
        baubleMaterial2Display.GetComponent<Image>().sprite = CONST_TRANSPARENT;
        if (baubleMaterial2 == null)
        {
            return;
        }
        GameObject baubleObj = Instantiate(baubleMaterial2.prefab, Bauble2Socket.transform);
        baubleMaterial2Display.GetComponent<Image>().sprite = baubleMaterial2.icon;
    }
}
