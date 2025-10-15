using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Mob : MonoBehaviour
{
    public float maxHealth;
    public float currHealth;
    bool isDying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage(float Damage)
    {
        if (isDying)
        {
            return;
        }
        currHealth -= Damage;
        if (currHealth <= 0)
        {
            isDying = true;
            gameObject.GetComponent<MobMovement>().Die();

        }
        else
        {
            gameObject.GetComponent<MobMovement>().Hurt();
        }
    }
}
