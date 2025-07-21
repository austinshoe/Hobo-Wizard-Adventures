using System.Collections;
using Cinemachine;


//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //Cam Shenanigans
    public CinemachineVirtualCamera camGameplay;
    public CinemachineVirtualCamera camFrogZoom;
    private Animator animator;
    public GameObject icePrefab;
    public GameObject enemy;
    public float animLength = 1.5f;
    [SerializeField] float iceSpacing; //Spacing between ice blocks
    public enum AttackType
    {
        Water,
        Ice,
        Fire,
        Lightning,
        Earth,
        Air,
        Shadow,
        Light,
    }
    private AttackType currentAttackType = AttackType.Ice;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Replace KeyCode.Space with your preferred input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();
        }
    }
    private void PerformAttack()
    {
        switch (currentAttackType)
        {
            case AttackType.Ice:
                PerformAttackIce();
                break;
            // Add cases for other attack types as needed
            default:
                Debug.LogWarning("Attack type not implemented: " + currentAttackType);
                break;
        }
    }
    private void PerformAttackIce()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = enemy.transform.position;
        Vector3 direction = (endPos - startPos);
        startPos += direction / 8f;
        endPos -= direction / 16f;
        direction = endPos - startPos;
        StartCoroutine(SpawnIce(startPos, endPos, direction));
        StartCoroutine(CameraAnimIce());

    }

    IEnumerator SpawnIce(Vector3 startPos, Vector3 endPos, Vector3 direction)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Attack Trigger Set");
        animator.SetTrigger("AttackTrigger");
        Renderer rend = icePrefab.GetComponent<Renderer>();
        Vector3 scale = rend.bounds.size;
        float slope = direction.y / direction.x;
        float dist;
        /*if (slope > 1f)
        { //y is larger than x
            dist = scale.y * scale.y + (scale.x / slope) * (scale.x / slope);
            dist = Mathf.Sqrt(dist);
        }
        else
        { //x is larger than y
            dist = scale.x * scale.x + (scale.y * slope) * (scale.y * slope);
            dist = Mathf.Sqrt(dist);
        }*/
        dist = iceSpacing;
        ArrayList iceObjects = new ArrayList();
        int numSteps = (int)(direction.magnitude / dist);
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < numSteps; i++)
        {
            Quaternion tilt;
            float xTilt;
            UnityEngine.Vector3 spawnPos = startPos + (direction / numSteps) * i;

            float yTilt = Random.Range(-7.5f, -7.5f);
            float zTilt = Random.Range(-7.5f, -7.5f);
            switch (i % 4)
            {
                case 0:
                    tilt = Quaternion.Euler(0f, yTilt, zTilt);
                    break;
                case 1:
                    xTilt = Random.Range(10f, 20f);
                    tilt = Quaternion.Euler(xTilt, yTilt, zTilt);
                    spawnPos -= new Vector3(0f, 0f, Random.Range(0.5f, 0.75f));
                    break;
                case 2:
                    xTilt = Random.Range(-10f, -20f);
                    tilt = Quaternion.Euler(xTilt, yTilt, zTilt);
                    spawnPos += new Vector3(0f, 0f, Random.Range(0.5f, 0.75f));
                    break;
                case 3:
                    tilt = Quaternion.Euler(0f, yTilt, zTilt);
                    break;
                default:
                    tilt = Quaternion.Euler(0f, yTilt, zTilt);
                    break;

            }
            UnityEngine.Quaternion rotation = UnityEngine.Quaternion.LookRotation(direction);
            tilt *= rotation;
            spawnPos.y -= scale.y;

            GameObject ice = Instantiate(icePrefab, spawnPos, tilt);
            ice.GetComponent<IceController>().RegisterInitHeight(spawnPos.y, scale.y);
            iceObjects.Add(ice);
            yield return new WaitForSeconds(0.075f);
        }

    }

    IEnumerator CameraAnimIce()
    {
        camFrogZoom.Priority = 20;
        camGameplay.Priority = 10;
        Quaternion startRot = camFrogZoom.transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(-20f, 0f, 0f));
        float t = 0f;
        while (t < 1f)
        {
            camFrogZoom.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        camGameplay.Priority = 50;
        camFrogZoom.Priority = 10;

        yield return new WaitForSeconds(0.5f);
        camFrogZoom.transform.rotation = startRot; // Reset camera rotation

    }
}