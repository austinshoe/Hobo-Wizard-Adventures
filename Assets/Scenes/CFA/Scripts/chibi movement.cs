using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class chibimovement : MonoBehaviour
{
    public GameObject chibi;
    public GameObject camera;
    public GameObject sceneinfo;
    public float speed;
    public float jumpforce;
    public float swimSpeed;
    public float swimUpSpeed;
    public bool isMoving;
    public bool isGrounded = true;
    public bool canSwimUp = true;
    public bool inAboveWater = false;
    private bool isTouchingWater = false;
    private bool intentionallyJumped = false;

    Vector3 facedir;
    void Start()
    {
        facedir = chibi.transform.forward;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAboveWater)
        {
            LandMovement();
        }
        else
        {
            altMovementSwim();
        }


    }

    void LandMovement() {
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
                intentionallyJumped = true;
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
                else if (intentionallyJumped)
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
                else if (intentionallyJumped)
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
            intentionallyJumped = false;
            if (inAboveWater && !isTouchingWater)
            {
                inAboveWater = false;
                chibi.GetComponent<Animator>().SetBool("ReturnToIdle", true);
                chibi.GetComponent<Animator>().SetBool("IdleSwim", false);
                chibi.GetComponent<Animator>().SetBool("Swim", false);

            }
            if (chibi.GetComponent<Animator>().GetBool("Jump"))
            {
                chibi.GetComponent<Animator>().SetBool("Jump", false);
                chibi.GetComponent<Animator>().SetBool("ReturnToIdle", true);
                chibi.GetComponent<Animator>().SetBool("Stop", false);
                chibi.GetComponent<Animator>().SetBool("Run", false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isTouchingWater = true;
            inAboveWater = true;
            isGrounded = false;
            if (isMoving)
            {
                chibi.GetComponent<Animator>().SetBool("IdleSwim", false);
                chibi.GetComponent<Animator>().SetBool("Swim", true);

            }
            else
            {
                chibi.GetComponent<Animator>().SetBool("IdleSwim", true);
                chibi.GetComponent<Animator>().SetBool("Swim", false);
            }
            chibi.GetComponent<Animator>().SetBool("Jump", false);
            chibi.GetComponent<Animator>().SetBool("ReturnToIdle", false);
            chibi.GetComponent<Animator>().SetBool("Stop", false);
            chibi.GetComponent<Animator>().SetBool("Run", false);
            canSwimUp = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isTouchingWater = false;
            if (isGrounded)
            {
                inAboveWater = false;
                chibi.GetComponent<Animator>().SetBool("ReturnToIdle", true);
                chibi.GetComponent<Animator>().SetBool("IdleSwim", false);
                chibi.GetComponent<Animator>().SetBool("Swim", false);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
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

    public void altMovementSwim()
    {
        isMoving = false;
        Vector3 temp = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space) && isUnderwater())
        {
            GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0.0f, GetComponent<Rigidbody>().linearVelocity.z);
            GetComponent<Rigidbody>().AddForce(Vector3.up * swimUpSpeed, ForceMode.Impulse);
            canSwimUp = false;
            StartCoroutine(SwimUpCoolDown());

        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            temp.z += 1.0f;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            temp.z -= 1.0f;

            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            temp.x -= 1.0f;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            temp.x += 1.0f;
            isMoving = true;
        }
        if (isMoving && temp != Vector3.zero)
        {
            chibi.transform.rotation = Quaternion.LookRotation(temp);
            facedir = temp;
            transform.Translate(temp.normalized * swimSpeed * Time.deltaTime, Space.World);
            if (!canSwimUp)
            {
                chibi.GetComponent<Animator>().SetBool("IdleSwim", true);
                chibi.GetComponent<Animator>().SetBool("Swim", false);
            }
            else
            {
                chibi.GetComponent<Animator>().SetBool("IdleSwim", false);
                chibi.GetComponent<Animator>().SetBool("Swim", true);
            }
        }
        else
        {
            chibi.transform.rotation = Quaternion.LookRotation(facedir);
            chibi.GetComponent<Animator>().SetBool("Swim", false);
            chibi.GetComponent<Animator>().SetBool("IdleSwim", true);
        }
    }

    IEnumerator SwimUpCoolDown() {
        yield return new WaitForSeconds(0.5f);
        canSwimUp = true;
    }

    public bool isUnderwater()
    {
        //return chibi.transform.position.y < sceneinfo.GetComponent<SceneInfo>().waterlevel;
        return transform.position.y < sceneinfo.GetComponent<SceneInfo>().waterlevel;
    }
}
