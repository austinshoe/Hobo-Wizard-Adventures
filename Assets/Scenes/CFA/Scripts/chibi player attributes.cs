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
        HP = 10;
        Atk = 5;
        Def = 5;
        Mana = 10;
        Agility = 5;
        Luck = 1;
        playerName = "Chibbster";
        elementType = SystemInfo.ElementType.Ice;
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
