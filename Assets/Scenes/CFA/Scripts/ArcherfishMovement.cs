using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArcherfishMovement : MonoBehaviour
{
    public bool isMoving = false;
    public float swimSpeed = 25;
    public GameObject sceneInfo;
    public SpawnZone spawnZone;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("IdleSwim", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneInfo == null || spawnZone == null)
        {
            return;
        }
        if (!sceneInfo.GetComponent<SceneInfo>().archerfishHostile)
        {
            if (!isMoving)
            {
                int action = Random.Range(0, 10);
                if (action < 5)
                {
                    isMoving = true;
                    StartCoroutine(IdleAround());
                }
                else
                {
                    isMoving = true;
                    SwimtoRandomPlace();
                }
            }
        }
    }
    IEnumerator IdleAround()
    {
        Debug.Log("Pausing");
        float pause = Random.Range(1.0f, 2.5f);
        float T = 0;
        while (T < pause)
        {
            T += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
        Debug.Log("Done Pausing");
    }
    void SwimtoRandomPlace()
    {
        Debug.Log("Swimming someplace random");
        Vector3 endPos = new Vector3(Random.Range(spawnZone.minX, spawnZone.maxX),
        Random.Range(spawnZone.minY, spawnZone.maxY - 5), Random.Range(spawnZone.minZ, spawnZone.maxZ));
        StopAllCoroutines();
        StartCoroutine(SwimTo(endPos));
    }

    IEnumerator SwimTo(Vector3 endPos)
    {
        Vector3 currPos = transform.position;
        Vector3 dir = (endPos - currPos).normalized;
        float tripDist = Vector3.Distance(currPos, endPos);
        float elapsedDist = 0f;

        float t = 0f;
        Quaternion currRot = transform.rotation;
        Quaternion destRot = Quaternion.LookRotation(dir);
        while (t < 0.5f)
        {
            transform.rotation = Quaternion.Slerp(currRot, destRot, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("IdleSwim", false);
        anim.SetBool("FastSwim", true);

        while (elapsedDist < tripDist /*&& transform.position.x > spawnZone.minX && transform.position.x < spawnZone.maxX
         && transform.position.y > spawnZone.minY && transform.position.y < spawnZone.maxY
         && transform.position.z > spawnZone.minZ && transform.position.z < spawnZone.maxZ*/)
        {
            Vector3 moveDir = dir;
            Vector3 escapeVector = Vector3.zero;

            // Check for nearby mobs
            foreach (GameObject mob in spawnZone.actualMobs)
            {
                if (mob == gameObject) continue;

                Vector3 offset = transform.position - mob.transform.position;
                float distSqr = offset.sqrMagnitude;
                if (distSqr < 150f) // threshold of about 12
                {
                    escapeVector += offset / distSqr;
                }
            }

            if (escapeVector != Vector3.zero)
            {
                moveDir = escapeVector.normalized;
            }

            // Smoothly rotate toward movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 5f);
            //transform.rotation = Quaternion.LookRotation(moveDir); // Snap

            transform.position += moveDir * swimSpeed * Time.deltaTime;
            elapsedDist += swimSpeed * Time.deltaTime;

            yield return null;
        }

        //transform.position = endPos;
        anim.SetBool("FastSwim", false);
        anim.SetBool("IdleSwim", true);
        yield return new WaitForSeconds(1f);
        isMoving = false;
    }

    /*IEnumerator SwimTo(Vector3 endPos)
    {
        float t = 0;
        Vector3 currPos = gameObject.transform.position;
        float tripDist = (endPos - currPos).magnitude;
        float elapsedDist = 0.0f;
        Vector3 dir = (endPos - currPos).normalized;
        Quaternion currRot = gameObject.transform.rotation;
        Quaternion destRot = Quaternion.LookRotation(dir);
        while (t < 0.5f)
        {
            transform.rotation = Quaternion.Lerp(currRot, destRot, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        anim.SetBool("IdleSwim", false);
        anim.SetBool("FastSwim", true);
        bool breakBool = false;
        while (elapsedDist < tripDist)
        {
            Vector3 escapeVector = Vector3.zero;
            foreach (GameObject mob in spawnZone.actualMobs)
            {
                float d = (transform.position - mob.transform.position).magnitude;
                if (d < 0.5f && mob != gameObject) //threshold dist
                {
                    Debug.Log("Sensed nearby other: " + d);
                    escapeVector += -((mob.transform.position - transform.position) / d) / d;
                }
            }
            if (escapeVector != Vector3.zero)
            {
                escapeVector = escapeVector.normalized;
                StartCoroutine(BeginEvasive(escapeVector));
                breakBool = true;
                break;
            }
            transform.Translate(dir * swimSpeed * Time.deltaTime, Space.World);
            elapsedDist += swimSpeed * Time.deltaTime;
            yield return null;
        }
        if (!breakBool)
        {
            transform.position = endPos;
            anim.SetBool("FastSwim", false);
            anim.SetBool("IdleSwim", true);
            yield return new WaitForSeconds(1f);
            isMoving = false;
            Debug.Log("Done swimming");
        }

    }

    IEnumerator BeginEvasive(Vector3 esc)
    {

        Debug.Log("Evasive");
        Quaternion currRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(esc);
        float t = 0;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(currRot, endRot, t / 0.25f);
            yield return null;
        }
        ExitEvasive(esc);
    }
    public void ExitEvasive(Vector3 esc)
    {
        StopAllCoroutines();
        StartCoroutine(SwimTo(ClampToBounds(transform.position, esc, Random.Range(3f, 5f))));
    }

    Vector3 ClampToBounds(Vector3 start, Vector3 direction, float distance)
    {
        Vector3 end = start + direction.normalized * distance;
        float maxT = 1f;

        if (direction.x > 0)
            maxT = Mathf.Min(maxT, (spawnZone.maxX - start.x) / direction.x);
        else if (direction.x < 0)
            maxT = Mathf.Min(maxT, (spawnZone.minX - start.x) / direction.x);

        if (direction.y > 0)
            maxT = Mathf.Min(maxT, (spawnZone.maxY - start.y) / direction.y);
        else if (direction.y < 0)
            maxT = Mathf.Min(maxT, (spawnZone.minY - start.y) / direction.y);

        if (direction.z > 0)
            maxT = Mathf.Min(maxT, (spawnZone.maxZ - start.z) / direction.z);
        else if (direction.z < 0)
            maxT = Mathf.Min(maxT, (spawnZone.minZ - start.z) / direction.z);
            
        return start + direction.normalized * distance * Mathf.Clamp01(maxT);
    }*/

}
