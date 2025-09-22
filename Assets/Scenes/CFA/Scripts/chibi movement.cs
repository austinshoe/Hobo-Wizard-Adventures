using UnityEngine;

public class chibimovement : MonoBehaviour
{
    public GameObject chibi;
    public GameObject camera;
    public float speed;
    public float jumpforce;
    public bool isMoving;

    Vector3 facedir;
    void Start()
    {
        facedir = chibi.transform.forward;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        Vector3 temp = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            temp.z += 1.0f;

            isMoving = true;
            //Debug.Log("W key is held down");
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            temp.z -= 1.0f;

            isMoving = true;
            //Debug.Log("S key is held down");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            temp.x -= 1.0f;
            isMoving = true;
            //Debug.Log("A key is held down");
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            temp.x += 1.0f;
            isMoving = true;
            //Debug.Log("D key is held down");
        }
        if (isMoving && temp != Vector3.zero &&
        !InteractionManager.interaction.LockPlayerControls && !InteractionManager.interaction.LocakAllInteraction)
        {
            chibi.transform.rotation = Quaternion.LookRotation(temp);
            facedir = temp;
            transform.Translate(temp.normalized * speed * Time.deltaTime, Space.World);
            chibi.GetComponent<Animator>().SetBool("Run", true);
            chibi.GetComponent<Animator>().SetBool("Stop", false);

        }
        else
        {
            chibi.transform.rotation = Quaternion.LookRotation(facedir);
            chibi.GetComponent<Animator>().SetBool("Run", false);
            chibi.GetComponent<Animator>().SetBool("Stop", true);
        }

    }
}
