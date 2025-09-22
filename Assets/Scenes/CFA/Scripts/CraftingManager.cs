using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public GameObject FadePanel;
    public RenderTexture rt;

    public StaffPart baseMaterial;
    public StaffPart headMaterial;
    public StaffPart baubleMaterial1;
    public StaffPart baubleMaterial2;
    public StaffPart hiltBaubleMaterial;
    public StaffPart hiltBaubleMaterial2;

    public GameObject BaseSocket;
    public GameObject HeadSocket;
    public GameObject HiltSocket;
    private bool canInteract;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        canInteract = false;
        FadePanel.SetActive(true);
        Color color = FadePanel.GetComponent<Image>().color;
        color.a = 1f;
        FadePanel.GetComponent<Image>().color = color;
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
            Staff newStaff = new Staff(baseMaterial, headMaterial, baubleMaterial1, baubleMaterial2, hiltBaubleMaterial, hiltBaubleMaterial2);
            InventoryManager.instance.craftedStaffs.Add(newStaff);
            //RawImage icon = CreateIcon();
            //newStaff.AssignIcon(icon);
            Sprite iconSprite = CaptureIcon(rt);
            newStaff.AssignIcon(iconSprite);
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
}
