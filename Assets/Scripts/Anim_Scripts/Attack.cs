using System.Collections;
using Cinemachine;
using Unity.Mathematics;



//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //Cam Shenanigans
    public CinemachineVirtualCamera camGameplay;
    public CinemachineVirtualCamera camFrogZoom;
    public CinemachineVirtualCamera camFrogAttack;
    public CinemachineVirtualCamera EnemyHitCam;
    public CinemachineVirtualCamera MidIceCam;
    public CinemachineVirtualCamera camIceSpellCamTwo;
    private Animator animator;
    public GameObject icePrefab;
    public GameObject enemy;
    public float animLength = 1.5f;
    [SerializeField] float iceSpacing; //Spacing between ice blocks
    public GameObject iceTrailLead;
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
    Quaternion startRot;

    void Start()
    {
        animator = GetComponent<Animator>();
        //iceTrailLead.GetComponent<Renderer>().enabled = false; //make invisible
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
        //StartCoroutine(SpawnIce(startPos, endPos, direction));
        StartCoroutine(SpawnIceVerTwo(startPos, endPos, direction));
        //StartCoroutine(CameraAnimIce());
        StartCoroutine(ZoomCamInSpellCast());

    }

    IEnumerator SpawnIce(Vector3 startPos, Vector3 endPos, Vector3 direction)
    {
        iceTrailLead.transform.position = gameObject.transform.position;
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
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(CamFollowIceSpell());
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < numSteps; i++)
        {
            Quaternion tilt;
            float xTilt;
            UnityEngine.Vector3 spawnPos = startPos + (direction / numSteps) * i;
            iceTrailLead.transform.position = new Vector3(spawnPos.x, 1.2f, spawnPos.z);

            float yTilt = UnityEngine.Random.Range(-7.5f, -7.5f);
            float zTilt = UnityEngine.Random.Range(-7.5f, -7.5f);
            switch (i % 4)
            {
                case 0:
                    tilt = Quaternion.Euler(0f, yTilt, zTilt);
                    break;
                case 1:
                    xTilt = UnityEngine.Random.Range(10f, 20f);
                    tilt = Quaternion.Euler(xTilt, yTilt, zTilt);
                    spawnPos -= new Vector3(0f, 0f, UnityEngine.Random.Range(0.5f, 0.75f));
                    break;
                case 2:
                    xTilt = UnityEngine.Random.Range(-10f, -20f);
                    tilt = Quaternion.Euler(xTilt, yTilt, zTilt);
                    spawnPos += new Vector3(0f, 0f, UnityEngine.Random.Range(0.5f, 0.75f));
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
        yield return new WaitForSeconds(2f);
        for (int i = iceObjects.Count - 1; i >= 0; i--)
        {
            GameObject ice = (GameObject)iceObjects[i];
            if (ice != null)
            {
                ice.GetComponent<IceController>().DestroyIce();
            }
        }
        yield return new WaitForSeconds(1f);
        camGameplay.Priority = 20;
        camFrogZoom.Priority = 10;
        camFrogAttack.Priority = 10;
        camIceSpellCamTwo.Priority = 10;
        EnemyHitCam.Priority = 10;
        MidIceCam.Priority = 10;
        yield return new WaitForSeconds(0.75f);
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

        camFrogAttack.Priority = 30;
        camGameplay.Priority = 20;
        camFrogZoom.Priority = 10;
        yield return null;

        yield return new WaitForSeconds(1f);

        Quaternion frogAttackStart = camFrogAttack.transform.rotation;
        Quaternion frogAttackEnd = MidIceCam.transform.rotation;
        Vector3 frogAttackPos = camFrogAttack.transform.position;
        Vector3 frogAttackEndPos = MidIceCam.transform.position;

        camFrogAttack.transform.rotation = frogAttackStart;
        camFrogAttack.transform.position = frogAttackPos;

        t = 0f;

        float duration = 0.5f;
        while (t < duration)
        {
            float easedT = Mathf.SmoothStep(0f, 1f, Mathf.Min(t / duration, 1f));
            camFrogAttack.transform.rotation = Quaternion.Lerp(frogAttackStart, frogAttackEnd, easedT);
            camFrogAttack.transform.position = Vector3.Lerp(frogAttackPos, frogAttackEndPos, easedT);
            t += Time.deltaTime;
            yield return null;
        }
        /*
        camFrogAttack.Priority = 10;
        camGameplay.Priority = 20;
        camFrogZoom.Priority = 10;
        MidIceCam.Priority = 30;
        */
        camFrogAttack.transform.rotation = frogAttackEnd;
        camFrogAttack.transform.position = frogAttackEndPos;

        Quaternion IceStart = camFrogAttack.transform.rotation;
        Vector3 IcePos = camFrogAttack.transform.position;
        // Quaternion IceStart = MidIceCam.transform.rotation;
        frogAttackEnd = EnemyHitCam.transform.rotation;
        // Vector3 IcePos = MidIceCam.transform.position;
        frogAttackEndPos = EnemyHitCam.transform.position;

        t = 0f;

        duration = 0.5f;
        while (t < duration)
        {
            float easedT = Mathf.SmoothStep(0f, 1f, Mathf.Min(t / duration, 1f));
            camFrogAttack.transform.rotation = Quaternion.Lerp(IceStart, frogAttackEnd, easedT);
            camFrogAttack.transform.position = Vector3.Lerp(IcePos, frogAttackEndPos, easedT);
            t += Time.deltaTime;
            yield return null;
        }

        camFrogZoom.transform.rotation = startRot;

        yield return new WaitForSeconds(0.5f);

        /*
        EnemyHitCam.Priority = 30;
        camFrogAttack.Priority = 10;
        camGameplay.Priority = 20;
        camFrogZoom.Priority = 10;
        MidIceCam.Priority = 10;
        */

        yield return new WaitForSeconds(0.6f);
        MidIceCam.transform.position = IcePos;
        MidIceCam.transform.rotation = IceStart;


        yield return new WaitForSeconds(1.5f); // Reset camera rotation

        EnemyHitCam.Priority = 10;
        camFrogAttack.Priority = 10;
        camGameplay.Priority = 20;
        camFrogZoom.Priority = 10;
        MidIceCam.Priority = 10;
        yield return new WaitForSeconds(0.5f);

        camFrogAttack.transform.position = frogAttackPos;
        camFrogAttack.transform.rotation = frogAttackStart;

    }
    /* THIS IS EXACTLY 100 LINES OF CODE SO IM SAVING IT DONT YOU DARE DELETE IT OR TOUCH IT ISTG
        IEnumerator CameraAnimIceTwo()
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

            camFrogAttack.Priority = 30;
            camGameplay.Priority = 20;
            camFrogZoom.Priority = 10;
            yield return null;

            yield return new WaitForSeconds(1f);

            Quaternion frogAttackStart = camFrogAttack.transform.rotation;
            Quaternion frogAttackEnd = MidIceCam.transform.rotation;
            Vector3 frogAttackPos = camFrogAttack.transform.position;
            Vector3 frogAttackEndPos = MidIceCam.transform.position;

            camFrogAttack.transform.rotation = frogAttackStart;
            camFrogAttack.transform.position = frogAttackPos;

            t = 0f;

            float duration = 0.5f;
            while (t < duration)
            {
                float easedT = Mathf.SmoothStep(0f, 1f, Mathf.Min(t / duration, 1f));
                camFrogAttack.transform.rotation = Quaternion.Lerp(frogAttackStart, frogAttackEnd, easedT);
                camFrogAttack.transform.position = Vector3.Lerp(frogAttackPos, frogAttackEndPos, easedT);
                t += Time.deltaTime;
                yield return null;
            }

            //camFrogAttack.Priority = 10;
            //camGameplay.Priority = 20;
            //camFrogZoom.Priority = 10;
            //MidIceCam.Priority = 30;

            camFrogAttack.transform.rotation = frogAttackEnd;
            camFrogAttack.transform.position = frogAttackEndPos;

            Quaternion IceStart = camFrogAttack.transform.rotation;
            Vector3 IcePos = camFrogAttack.transform.position;
            // Quaternion IceStart = MidIceCam.transform.rotation;
            frogAttackEnd = EnemyHitCam.transform.rotation;
            // Vector3 IcePos = MidIceCam.transform.position;
            frogAttackEndPos = EnemyHitCam.transform.position;

            t = 0f;

            duration = 0.5f;
            while (t < duration)
            {
                float easedT = Mathf.SmoothStep(0f, 1f, Mathf.Min(t / duration, 1f));
                camFrogAttack.transform.rotation = Quaternion.Lerp(IceStart, frogAttackEnd, easedT);
                camFrogAttack.transform.position = Vector3.Lerp(IcePos, frogAttackEndPos, easedT);
                t += Time.deltaTime;
                yield return null;
            }

            camFrogZoom.transform.rotation = startRot;

            yield return new WaitForSeconds(0.5f);


            //EnemyHitCam.Priority = 30;
            //camFrogAttack.Priority = 10;
            //camGameplay.Priority = 20;
            //camFrogZoom.Priority = 10;
            //MidIceCam.Priority = 10;


            yield return new WaitForSeconds(0.6f);
            MidIceCam.transform.position = IcePos;
            MidIceCam.transform.rotation = IceStart;


            yield return new WaitForSeconds(1.5f); // Reset camera rotation

            EnemyHitCam.Priority = 10;
            camFrogAttack.Priority = 10;
            camGameplay.Priority = 20;
            camFrogZoom.Priority = 10;
            MidIceCam.Priority = 10;
            yield return new WaitForSeconds(0.5f);

            camFrogAttack.transform.position = frogAttackPos;
            camFrogAttack.transform.rotation = frogAttackStart;

        }
        */
    IEnumerator ZoomCamInSpellCast()
    {
        camFrogZoom.Priority = 20;
        camGameplay.Priority = 10;
        startRot = camFrogZoom.transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(-20f, 0f, 0f));
        float t = 0f;
        while (t < 1f)
        {
            camFrogZoom.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CamFollowIceSpell()
    {
        camIceSpellCamTwo.Priority = 30;
        camFrogZoom.Priority = 10;
        camGameplay.Priority = 20;
        camFrogZoom.transform.rotation = startRot;
        yield return null;

    }


    IEnumerator SpawnIceVerTwo(Vector3 startPos, Vector3 endPos, Vector3 direction)
    {
        iceTrailLead.transform.position = gameObject.transform.position + new Vector3(0f, 1.2f, 0f);
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
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(CamFollowIceSpell());
        yield return new WaitForSeconds(0.66f);
        float totalTime = numSteps * 0.05f;
        StartCoroutine(IceFollowTrail(startPos, direction, totalTime));
        for (int i = 0; i < numSteps - 1; i++)
        {
            Quaternion tilt;
            float xTilt;
            UnityEngine.Vector3 spawnPos = startPos + (direction / numSteps) * i;

            float yTilt = UnityEngine.Random.Range(-7.5f, -7.5f);
            float zTilt = UnityEngine.Random.Range(-7.5f, -7.5f);
            switch (i % 4)
            {
                case 0:
                    tilt = Quaternion.Euler(0f, yTilt, zTilt);
                    break;
                case 1:
                    xTilt = UnityEngine.Random.Range(10f, 20f);
                    tilt = Quaternion.Euler(xTilt, yTilt, zTilt);
                    spawnPos -= new Vector3(0f, 0f, UnityEngine.Random.Range(0.5f, 0.75f));
                    break;
                case 2:
                    xTilt = UnityEngine.Random.Range(-10f, -20f);
                    tilt = Quaternion.Euler(xTilt, yTilt, zTilt);
                    spawnPos += new Vector3(0f, 0f, UnityEngine.Random.Range(0.5f, 0.75f));
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
            yield return new WaitForSeconds(0.05f);
        }
        ArrayList iceObjectsBig = new ArrayList();
        for (int i = 0; i < 3; i++)
        {
            Quaternion tilt = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(25f, 35f));
            Quaternion rot = Quaternion.Euler(0f, i / 3f * 360 + UnityEngine.Random.Range(-5f, 5f), 0f);
            tilt = rot * tilt;
            Vector3 spawnPos = new Vector3(enemy.transform.position.x, 0f, enemy.transform.position.z);
            spawnPos.y -= scale.y * 2f;
            //spawnPos -= transform.right * 2f;
            GameObject BigIce = Instantiate(icePrefab, spawnPos, tilt);
            BigIce.transform.position += BigIce.transform.right * 2f;
            BigIce.transform.localScale = new Vector3(2f, 2f, 2f);
            iceObjectsBig.Add(BigIce);
            BigIce.GetComponent<IceController>().RegisterInitHeight(spawnPos.y, scale.y /** 2*/);
        }
        enemy.GetComponent<Animator>().SetTrigger("EnemyAttacked");
        yield return new WaitForSeconds(0.075f);

        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(1.8f);
        enemy.GetComponent<Animator>().ResetTrigger("EnemyAttacked");
        for (int i = iceObjects.Count - 1; i >= 0; i--)
        {
            GameObject ice = (GameObject)iceObjects[i];
            if (ice != null)
            {
                ice.GetComponent<IceController>().DestroyIce();
            }
        }

        yield return new WaitForSeconds(1f);

        for (int i = iceObjectsBig.Count - 1; i >= 0; i--)
        {
            GameObject ice = (GameObject)iceObjectsBig[i];
            if (ice != null)
            {
                ice.GetComponent<IceController>().DestroyIce();
            }
        }
        yield return new WaitForSeconds(0.5f);
        enemy.GetComponent<Animator>().SetTrigger("EnemyAttackedToIdle");
        yield return new WaitForSeconds(0.5f);
        camGameplay.Priority = 20;
        camFrogZoom.Priority = 10;
        camFrogAttack.Priority = 10;
        camIceSpellCamTwo.Priority = 10;
        EnemyHitCam.Priority = 10;
        MidIceCam.Priority = 10;
        //enemy.GetComponent<Animator>().ResetTrigger("EnemyAttackedToIdle");
        yield return new WaitForSeconds(0.75f);
    }

    IEnumerator IceFollowTrail(Vector3 startPos, Vector3 direction, float totalTime)
    {
        Vector3 targetPos = startPos + direction;
        Vector3 start = iceTrailLead.transform.position;
        float elapsed = 0f;

        while (elapsed < totalTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / totalTime;
            // Smooth movement
            iceTrailLead.transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        // Snap to exact position at the end
        iceTrailLead.transform.position = targetPos;
    }
}