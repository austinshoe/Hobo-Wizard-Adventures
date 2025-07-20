using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Replace KeyCode.Space with your preferred input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Attack Trigger Set");
            animator.SetTrigger("AttackTrigger");
        }
    }
}