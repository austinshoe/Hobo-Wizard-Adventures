using System.Collections;
using UnityEngine;

public class chibimovement : MonoBehaviour
{
    public GameObject chibi;
    public GameObject camera;
    public float speed;
    public float jumpforce;
    public bool isMoving;
    public bool isGrounded = true;

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
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!InteractionManager.interaction.LockPlayerControls && !InteractionManager.interaction.LocakAllInteraction)
            {
                InteractionManager.interaction.LockPlayerControls = true;
                chibi.GetComponent<Animator>().SetBool("Run", false);
                chibi.GetComponent<Animator>().SetBool("Stop", false);
                chibi.GetComponent<Animator>().SetBool("Attack", true);
                chibi.GetComponent<Animator>().SetBool("ReturnToIdle", false);
                StartCoroutine(WaitAttackEnd());
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
                isGrounded = false;
            }
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
                //camera.transform.localPosition = chibi.transform.localPosition + new Vector3(0, 1.5f, -1.5f);
                if (isGrounded)
                {
                    chibi.GetComponent<Animator>().SetBool("Run", true);
                    chibi.GetComponent<Animator>().SetBool("Stop", false);
                }
                else
                {
                    chibi.GetComponent<Animator>().SetBool("Run", false);
                    chibi.GetComponent<Animator>().SetBool("Stop", false);
                    chibi.GetComponent<Animator>().SetBool("Jump", true);
                }
            }
            else
            {
                chibi.transform.rotation = Quaternion.LookRotation(facedir);
                if (isGrounded)
                {
                    chibi.GetComponent<Animator>().SetBool("Run", false);
                    chibi.GetComponent<Animator>().SetBool("Stop", true);
                }
                else
                {
                    chibi.GetComponent<Animator>().SetBool("Run", false);
                    chibi.GetComponent<Animator>().SetBool("Stop", false);
                    chibi.GetComponent<Animator>().SetBool("Jump", true);
                }
            }
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (chibi.GetComponent<Animator>().GetBool("Jump"))
            {
                chibi.GetComponent<Animator>().SetBool("Jump", false);
                chibi.GetComponent<Animator>().SetBool("ReturnToIdle", true);
                chibi.GetComponent<Animator>().SetBool("Stop", false);
                chibi.GetComponent<Animator>().SetBool("Run", false);
            }
        }
    }


    IEnumerator WaitAttackEnd()
    {
        yield return new WaitForSeconds(0.75f);
        chibi.GetComponent<Animator>().SetBool("Attack", false);
        chibi.GetComponent<Animator>().SetBool("Stop", true);
        chibi.GetComponent<Animator>().SetBool("Run", false);
        chibi.GetComponent<Animator>().SetBool("ReturnToIdle", true);
        InteractionManager.interaction.LockPlayerControls = false;
    }
}
