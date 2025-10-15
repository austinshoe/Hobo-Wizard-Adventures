using UnityEngine;

public class GameflowManager : MonoBehaviour
{
    public static GameflowManager instance;
    public enum MenuState
    {
        MainMenu,
        Customization,
        Staff,
        Headwear,
        Robe,
        Crafting,
        Playing,
        None
    }

    public MenuState currentState = MenuState.Customization;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
