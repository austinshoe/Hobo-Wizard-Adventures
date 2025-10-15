using System;
using Unity.Mathematics;
using UnityEngine;

public class chibiplayerattributes : MonoBehaviour
{
    int HP, Atk, Def, Mana, Agility, Luck;
    SystemInfo.ElementType elementType;
    String playerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerManager.instance == null)
        {
            HP = 10;
            Atk = 5;
            Def = 5;
            Mana = 10;
            Agility = 5;
            Luck = 1;
            playerName = "Chibbster";
            elementType = SystemInfo.ElementType.Ice;
        }
        else {
            HP = PlayerManager.instance.HP;
            Atk = PlayerManager.instance.Atk;
            Def = PlayerManager.instance.Def;
            Mana = PlayerManager.instance.Mana;
            Agility = PlayerManager.instance.Agility;
            Luck = PlayerManager.instance.Luck;
            playerName = PlayerManager.instance.playerName;
            elementType = PlayerManager.instance.elementType;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int[] ReturnVisualStats()
    {
        return new int[] { HP, Atk, Def, Mana, Agility, Luck };
    }

    public float[] calcInternalStats()
    {
        float internalHP = HP * 10;
        float internalAgility = math.sqrt((Agility - 5) / 35.0f) + 1;
        return new float[] { internalHP, Atk, Def, Mana, internalAgility, Luck };


    }
    public String GetName()
    {
        return playerName;
    }

    public SystemInfo.ElementType GetElementType()
    {
        return elementType;
    }
}
