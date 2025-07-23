using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Dictionary<Move.Type, GenericAttack> attackDictionary;

    void Awake()
    {
        attackDictionary = new Dictionary<Move.Type, GenericAttack>();
        attackDictionary[Move.Type.Ice] = GetComponent<Attack>(); // example
        // add other attack types similarly
    }

    public void AttackControl(Move.Type type)
    {
        if (attackDictionary.TryGetValue(type, out GenericAttack attack))
        {
            attack.PerformAttack();
        }
        else
        {
            Debug.LogWarning($"No attack found for type {type}");
        }
    }
}