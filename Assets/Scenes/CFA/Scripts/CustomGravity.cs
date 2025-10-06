using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public float gravityMultiplier = 2f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // turn off default gravity
    }

    void FixedUpdate()
    {
        
        Vector3 customGravity = Physics.gravity * gravityMultiplier;
        if (gameObject.GetComponent<chibimovement>().isUnderwater())
        {
            customGravity *= 0.167f;
            if (gameObject.GetComponent<chibimovement>().isMoving && gameObject.GetComponent<chibimovement>().canSwimUp)
            {
                rb.linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
                customGravity *= 0.0f;
            }
        }
        rb.AddForce(customGravity * 25, ForceMode.Acceleration);
    }
}