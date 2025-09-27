using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject EntireAssPanel;
    public GameObject CustomizationMenu;
    public GameObject StaffMenu;
    public GameObject HeadwearMenu;
    public GameObject RobeMenu;
    public GameObject CustomizingModel;
    GameObject CurrentMenu;

    public GameObject StaffScrollViewContent;
    public GameObject StaffButtonPrefab;
    public Sprite CONST_TRANSPARENT;
    public Sprite CONST_CREATE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EntireAssPanel.SetActive(false);
        StartCoroutine(SetUpScreen());
    }

    IEnumerator SetUpScreen()
    {
        yield return new WaitForSeconds(0.01f);
        CustomizationMenu.SetActive(false);
        StaffMenu.SetActive(false);
        HeadwearMenu.SetActive(false);
        RobeMenu.SetActive(false);
        switch (GameflowManager.instance.currentState)
        {
            case GameflowManager.MenuState.Customization:
                CurrentMenu = CustomizationMenu;
                break;
            case GameflowManager.MenuState.Staff:
                CurrentMenu = StaffMenu;
                SetUpStaffScreen();
                break;
            case GameflowManager.MenuState.Headwear:
                CurrentMenu = HeadwearMenu;
                break;
            case GameflowManager.MenuState.Robe:
                CurrentMenu = RobeMenu;
                break;
            default:
                CurrentMenu = CustomizationMenu;
                break;
        }
        CurrentMenu.SetActive(true);
        CanvasGroup canvasGroup = EntireAssPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = EntireAssPanel.AddComponent<CanvasGroup>();
        }
        float t = 0f;
        canvasGroup.alpha = 0;
        EntireAssPanel.SetActive(true);
        while (t < 1f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / 1f);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchToCustomization()
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.SetActive(false);
        }
        CurrentMenu = CustomizationMenu;
        CurrentMenu.SetActive(true);
        GameflowManager.instance.currentState = GameflowManager.MenuState.Customization;
    }

    public void SwitchToStaff()
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.SetActive(false);
        }
        CurrentMenu = StaffMenu;
        CurrentMenu.SetActive(true);
        GameflowManager.instance.currentState = GameflowManager.MenuState.Staff;
        SetUpStaffScreen();
    }

    public void SwitchToHeadwear()
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.SetActive(false);
        }
        CurrentMenu = HeadwearMenu;
        CurrentMenu.SetActive(true);
        GameflowManager.instance.currentState = GameflowManager.MenuState.Headwear;
    }

    public void SwitchToRobe()
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.SetActive(false);
        }
        CurrentMenu = RobeMenu;
        CurrentMenu.SetActive(true);
        GameflowManager.instance.currentState = GameflowManager.MenuState.Robe;
    }

    public void SetUpStaffScreen()
    {
        foreach (Transform child in StaffScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Staff staff in InventoryManager.instance.craftedStaffs)
        {
            GameObject staffButton = Instantiate(StaffButtonPrefab, StaffScrollViewContent.transform);
            GameObject buttonPanel = staffButton.transform.Find("IconPanel").gameObject;
            //buttonPanel.GetComponent<Image>().sprite = staff.icon;
            buttonPanel.GetComponent<Image>().sprite = Sprite.Create(
                staff.icon,
                new Rect(0, 0, staff.icon.width, staff.icon.height),
                new Vector2(0.5f, 0.5f)
            );
            staffButton.GetComponent<Button>().onClick.AddListener(() => SelectStaff(staff));
        }
        GameObject createButton = Instantiate(StaffButtonPrefab, StaffScrollViewContent.transform);
        GameObject createButtonPanel = createButton.transform.Find("IconPanel").gameObject;
        createButtonPanel.GetComponent<Image>().sprite = CONST_CREATE;
        createButton.GetComponent<Button>().onClick.AddListener(() => CreateStaff());
    }

    public void CreateStaff()
    {
        InventoryManager.instance.currentStaff = null;
        SceneManager.LoadScene("Crafting Scene");
    }

    public void SelectStaff(Staff staff)
    {
        InventoryManager.instance.currentStaff = staff;
        CustomizingModel.GetComponent<CustomationManager>().UpdateModel();
    }

    public void PlayGame()
    {
        GameflowManager.instance.currentState = GameflowManager.MenuState.Playing;
        SceneManager.LoadScene("Chibi Frog Adventures");
    }
}
