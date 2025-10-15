using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int HP, Atk, Def, Mana, Agility, Luck;
    public SystemInfo.ElementType elementType;
    public String playerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            HP = 10;
            Atk = 5;
            Def = 5;
            Mana = 10;
            Agility = 5;
            Luck = 1;
            playerName = "Chibbster";
            elementType = SystemInfo.ElementType.Ice;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
