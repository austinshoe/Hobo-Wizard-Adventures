using System.Collections;
using Cinemachine;
using Unity.Mathematics;



//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack : MonoBehaviour, GenericAttack
{
    public GameObject lightningboltPrefab;
    public GameObject sparkExplosionPrefab;
    public GameObject fireStormPrefab;
    private GameObject AttackOBJInstance; // used from now on to keep track of what would have been fire column instance, etc

    //public GameObject mainCamera;
    private GameObject fireColumnInstance;
    public GameObject fireColumnPrefab;
    private GameObject strongIceInstance;
    //Cam Shenanigans
    public CinemachineVirtualCamera camGameplay;
    public CinemachineVirtualCamera camFrogZoom;
    //public CinemachineVirtualCamera camFrogAttack;
    //public CinemachineVirtualCamera EnemyHitCam;
    // public CinemachineVirtualCamera MidIceCam;
    public CinemachineVirtualCamera camIceSpellCamTwo;
    private Animator animator;
    public GameObject icePrefab;
    public GameObject enemy;
    public float animLength = 1.5f;
    [SerializeField] float iceSpacing; //Spacing between ice blocks
    public GameObject iceTrailLead;

    public Move.Effect currentMoveType = Move.Effect.Weak;

    private Move.Type currentAttackType = Move.Type.Ice;
    Quaternion startRot;

    public GameObject[] MagicCirclePrefabs;
    private GameObject magicCircleInstance;
    public Vector3 CirclePos;
    public Quaternion CircleRot;
    public Vector3 CircleScale;
    public GameObject strongIce;

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
            SetMove(Move.Type.Ice, Move.Effect.Weak);
            PerformAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SetMove(Move.Type.Ice, Move.Effect.Strong);
            PerformAttack();

        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            SetMove(Move.Type.Fire, Move.Effect.Weak);
            PerformAttack();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SetMove(Move.Type.Fire, Move.Effect.Strong);
            PerformAttack();

        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            SetMove(Move.Type.Lightning, Move.Effect.Weak);
            PerformAttack();
        }
    }
    public void PerformAttack()
    {
        switch (currentAttackType)
        {
            case Move.Type.Ice:
                PerformAttackIce();
                break;
            // Add cases for other attack types as needed
            case Move.Type.Fire:
                PerformAttackFire();
                break;
            case Move.Type.Lightning:
                PerformAttackLightning();
                break;
            default:
                Debug.LogWarning("Attack type not implemented: " + currentAttackType);
                break;
        }
    }

    public void PerformAttackLightning()
    {
        switch (currentMoveType)
        {
            case Move.Effect.Weak:
                PerformAttackLightningWeak();
                break;
            case Move.Effect.Strong:
                //PerformAttackFireStrong();
                break;
            case Move.Effect.Status:
                // Implement status ice attack logic here
                break;
            case Move.Effect.Shield:
                // Implement shield ice attack logic here
                break;
            case Move.Effect.WildCard:
                // Implement wild card ice attack logic here
                break;
            default:
                Debug.LogWarning("Ice attack effect not implemented: " + currentMoveType);
                break;
        }
    }

    public void PerformAttackLightningWeak()
    {
        StartCoroutine(ZoomCamInSpellCast());
        StartCoroutine(AttackLightningStrong());
    }

    IEnumerator AttackLightningStrong()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Attack Trigger Set");
        animator.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(1f);
        AttackOBJInstance = Instantiate(lightningboltPrefab, magicCircleInstance.transform.position, Quaternion.identity);
        LightningBoltController bolt = AttackOBJInstance.GetComponent<LightningBoltController>();
        bolt.CreateBolt(magicCircleInstance.transform.position, new Vector3(enemy.transform.position.x, 1f, enemy.transform.position.z));
        yield return new WaitForSeconds(0.06f);
        GameObject AttackOBJInstance2 = Instantiate(lightningboltPrefab, magicCircleInstance.transform.position, Quaternion.identity);
        LightningBoltController bolt2 = AttackOBJInstance2.GetComponent<LightningBoltController>();
        bolt2.CreateBolt(magicCircleInstance.transform.position, new Vector3(enemy.transform.position.x, 1f, enemy.transform.position.z));
        yield return new WaitForSeconds(0.06f);
        GameObject AttackOBJInstance3 = Instantiate(lightningboltPrefab, magicCircleInstance.transform.position, Quaternion.identity);
        LightningBoltController bolt3 = AttackOBJInstance3.GetComponent<LightningBoltController>();
        bolt3.CreateBolt(magicCircleInstance.transform.position, new Vector3(enemy.transform.position.x, 1f, enemy.transform.position.z));
    }

    public void PerformAttackFire()
    {
        switch (currentMoveType)
        {
            case Move.Effect.Weak:
                PerformAttackFireWeak();
                break;
            case Move.Effect.Strong:
                PerformAttackFireStrong();
                break;
            case Move.Effect.Status:
                // Implement status ice attack logic here
                break;
            case Move.Effect.Shield:
                // Implement shield ice attack logic here
                break;
            case Move.Effect.WildCard:
                // Implement wild card ice attack logic here
                break;
            default:
                Debug.LogWarning("Ice attack effect not implemented: " + currentMoveType);
                break;
        }
    }


    public void PerformAttackFireStrong()
    {
        StartCoroutine(ZoomCamInSpellCast());
        StartCoroutine(AttackFireStrong());
    }

    IEnumerator AttackFireStrong()
    {
        Vector3 startPos = new Vector3(2, 1.2f, 0f);
        iceTrailLead.transform.position = startPos;
        Vector3 camStartPos = new Vector3(0.25f, 2f, -3.68f);
        camIceSpellCamTwo.transform.position = camStartPos;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Attack Trigger Set");
        animator.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(2f);
        StartCoroutine(DestroyMagicCircle());
        AttackOBJInstance = Instantiate(fireStormPrefab, iceTrailLead.transform.position - Vector3.up * 0.2f, Quaternion.Euler(0f, 0f, 0f));
        AttackOBJInstance.GetComponent<FireStormController>().Scale(Vector3.zero);
        camIceSpellCamTwo.Priority = 20;
        camFrogZoom.Priority = 10;
        float t = 0f;
        while (t < 0.6f)
        {
            float normT = t / 0.6f;
            AttackOBJInstance.GetComponent<FireStormController>().Scale(Vector3.one * 0.25f * normT);
            t += Time.deltaTime;
            yield return null;
        }
        AttackOBJInstance.GetComponent<FireStormController>().Scale(Vector3.one * 0.25f);
        //yield return new WaitForSeconds(0.5f);

        Vector3 stormStartPos = AttackOBJInstance.transform.position;
        t = 0f;
        Vector3 endCamPos = new Vector3(-2.42f, 6.6f, -7.49f);
        while (t < 0.5)
        {
            float normT = t / 0.5f;
            iceTrailLead.transform.position = Vector3.Lerp(startPos, enemy.transform.position + Vector3.up * 1.2f, normT);
            AttackOBJInstance.transform.position = Vector3.Lerp(stormStartPos, enemy.transform.position, normT);
            camIceSpellCamTwo.transform.position = Vector3.Lerp(camStartPos, endCamPos, normT);
            t += Time.deltaTime;
            yield return null;
        }
        iceTrailLead.transform.position = enemy.transform.position + Vector3.up * 1.2f;
        AttackOBJInstance.transform.position = enemy.transform.position;
        enemy.GetComponent<Animator>().SetTrigger("EnemyAttacked");
        camIceSpellCamTwo.transform.position = endCamPos;
        yield return new WaitForSeconds(0.5f);
        Vector3 FireStormCurrScale = Vector3.one * 0.25f;
        t = 0f;
        FireStormController stormctrl = AttackOBJInstance.GetComponent<FireStormController>();
        while (t < 0.5f)
        {
            float normT = t / 0.5f;
            float easeOutT = 1f - Mathf.Pow(1f - normT, 3);
            stormctrl.Scale(Vector3.Lerp(FireStormCurrScale, Vector3.one, easeOutT));
            t += Time.deltaTime;
            yield return null;

        }
        stormctrl.Scale(Vector3.one);
        yield return new WaitForSeconds(0.5f);
        fireColumnInstance = Instantiate(fireColumnPrefab, enemy.transform.position, Quaternion.identity);
        //fireColumnInstance.GetComponent<FireColumnController>().SpawnPenta(fireColumnInstance.transform.position, 1.5f);
        fireColumnInstance.GetComponent<FireColumnController>().SetEmittion(true);
        yield return new WaitForSeconds(0.75f);
        t = 0f;
        camStartPos = camIceSpellCamTwo.transform.position;
        endCamPos = new Vector3(5.36f, 1.43f, -1.62f);
        while (t < 0.75f)
        {
            float normT = t / 0.75f;
            stormctrl.Scale(Vector3.Lerp(Vector3.one, Vector3.one * 0.2f, normT));
            camIceSpellCamTwo.transform.position = Vector3.Lerp(camStartPos, endCamPos, normT);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        stormctrl.Scale(Vector3.one * 0.2f);
        camIceSpellCamTwo.transform.position = endCamPos;
        Vector3 originalPos = camIceSpellCamTwo.transform.localPosition;
        Vector3 originalPos2 = iceTrailLead.transform.localPosition;
        Vector3 originalPos3 = fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale;
        Vector3 originalPos4 = fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale;
        t = 0f;
        while (t < 0.25f)
        {
            float normT = t / 0.25f;
            float x = Random.Range(-1f, 1f) * 0.05f;
            float y = Random.Range(-1f, 1f) * 0.05f;

            camIceSpellCamTwo.transform.localPosition = originalPos + new Vector3(x, y, 0);
            iceTrailLead.transform.localPosition = originalPos2 + new Vector3(x, y, 0);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0f;
        endCamPos = new Vector3(4.36f, 2.63f, -2.36f);
        camStartPos = camIceSpellCamTwo.transform.position;
        Vector3 stormPos = stormctrl.transform.position;
        while (t < 2f)
        {
            float normT = t / 2f;
            stormctrl.Scale(Vector3.Lerp(Vector3.one * 0.2f, Vector3.zero, normT));
            stormctrl.transform.position = Vector3.Lerp(stormPos, stormPos + 1.2f * Vector3.up, normT);
            fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = originalPos3 * (1 + normT);
            fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = originalPos4 * (1 + normT);
            float x = Random.Range(-1f, 1f) * 0.05f;
            float y = Random.Range(-1f, 1f) * 0.05f;

            originalPos = Vector3.Lerp(camStartPos, endCamPos, normT);
            camIceSpellCamTwo.transform.localPosition = originalPos + new Vector3(x, y, 0);
            iceTrailLead.transform.localPosition = originalPos2 + new Vector3(x, y, 0);
            t += Time.deltaTime;
            yield return null;
        }
        fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = originalPos3 * 2f;
        fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = originalPos4 * 2f;
        stormctrl.Scale(Vector3.zero);
        camIceSpellCamTwo.transform.localPosition = endCamPos;
        iceTrailLead.transform.localPosition = originalPos2;
        yield return new WaitForSeconds(0.5f);
        t = 0;
        originalPos = fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale;
        originalPos2 = fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale;
        while (t < 0.75f)
        {
            float normT = (0.75f - t) / 0.75f;
            fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = originalPos2 * normT;
            fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = originalPos * normT;
            t += Time.deltaTime;
            yield return null;
        }
        fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.zero;
        fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = Vector3.zero;
        GameObject expl = Instantiate(sparkExplosionPrefab, enemy.transform.position + Vector3.up * 1.2f, Quaternion.identity);
        sparkExplosionPrefab.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        enemy.GetComponent<Animator>().ResetTrigger("EnemyAttacked");
        enemy.GetComponent<Animator>().SetTrigger("EnemyAttackedToIdle");
        //fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.one * 2.5f;
        //fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = Vector3.one * 2.5f;
        /* t = 0f;
         while (t < 0.0625f)
         {
             float normT = t / 0.0625f;
             stormctrl.Scale(Vector3.one * 5 * normT);
             t += Time.deltaTime;
             yield return null;
         }
         stormctrl.Scale(Vector3.one * 5);
         yield return null;
         stormctrl.FireStorm.GetComponent<ParticleSystem>().Stop();
         stormctrl.EmberEffect.GetComponent<ParticleSystem>().Stop();
         stormctrl.Smoke.GetComponent<ParticleSystem>().Stop();
         stormctrl.Ashes.GetComponent<ParticleSystem>().Stop();*/
        yield return new WaitForSeconds(0.5f);

        /*t = 0f;
        originalPos = camIceSpellCamTwo.transform.localPosition;
        originalPos2 = iceTrailLead.transform.localPosition;
        while (t < 0.125f)
        {
            float normT = (0.125f - t) / 0.125f;
            float x = Random.Range(-1f, 1f) * 0.1f;
            float y = Random.Range(-1f, 1f) * 0.1f;

            camIceSpellCamTwo.transform.localPosition = originalPos + new Vector3(x, y, 0);
            iceTrailLead.transform.localPosition = originalPos2 + new Vector3(x, y, 0);
            fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.one * 2f * normT;
            fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = Vector3.one * 2f * normT;
            t += Time.deltaTime;
            yield return null;
        }
        fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.zero;
        fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale = Vector3.zero;*/
        Destroy(expl);
        Destroy(fireColumnInstance);
        Destroy(AttackOBJInstance);
        yield return new WaitForSeconds(0.5f);
        camFrogZoom.transform.rotation = startRot;
        camGameplay.Priority = 20;
        camIceSpellCamTwo.Priority = 10;
        camFrogZoom.Priority = 10;
        
    }


    public void PerformAttackFireWeak()
    {
        StartCoroutine(ZoomCamInSpellCast());
        StartCoroutine(AttackFireWeak());
    }

    IEnumerator AttackFireWeak()
    {
        iceTrailLead.transform.position = new Vector3(3.75f, 1.2f, 0f);
        camIceSpellCamTwo.transform.position = new Vector3(-1f, 1.2f, -2f);

        //iceTrailLead.transform.position = new Vector3(4f, 1.2f, 0f);
        //camIceSpellCamTwo.transform.position = new Vector3(2.75f, 2.4f, -5f);

        // iceTrailLead.transform.position = new Vector3(3.75f, 1.2f, 0f);
        //iceTrailLead.transform.position = enemy.transform.position;
        //camIceSpellCamTwo.transform.position = new Vector3(3.75f, 2.4f, -4f);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Attack Trigger Set");
        animator.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(0.5f);

        camIceSpellCamTwo.Priority = 20;
        camFrogZoom.Priority = 10;
        camGameplay.Priority = 10;
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DestroyMagicCircle());
        Vector3 mCircleSpawnPos = enemy.transform.position + Vector3.up * 0.1f;
        GameObject floorCircle = Instantiate(MagicCirclePrefabs[(int)currentAttackType], mCircleSpawnPos, Quaternion.Euler(90f, 0f, 0f));
        floorCircle.transform.localScale = Vector3.one * 0.2f;
        fireColumnInstance = Instantiate(fireColumnPrefab, enemy.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.66f);
        fireColumnInstance.GetComponent<FireColumnController>().SpawnPenta(fireColumnInstance.transform.position, 1.5f);
        Vector3 currIceTrailPos = iceTrailLead.transform.position;
        Vector3 currCamPos = camIceSpellCamTwo.transform.position;
        float t = 0f;
        while (t < 0.5f)
        {
            float normT = t / 0.5f;
            iceTrailLead.transform.position = Vector3.Lerp(currIceTrailPos, enemy.transform.position + Vector3.up * 1.2f, normT);
            camIceSpellCamTwo.transform.position = Vector3.Lerp(currCamPos, currIceTrailPos, normT);
            t += Time.deltaTime;
            yield return null;
        }
        iceTrailLead.transform.position = enemy.transform.position + Vector3.up * 1.2f;
        camIceSpellCamTwo.transform.position = currIceTrailPos;
        yield return null;
        fireColumnInstance.GetComponent<FireColumnController>().ContractPenta();
        //2.25 for entire, 1.25 - 2.25 is nothing, big flame starts at 2.25
        // -1.92, 1.2, -1.98
        yield return new WaitForSeconds(0.5f);

        enemy.GetComponent<Animator>().SetTrigger("EnemyAttacked");
        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(1f);
        t = 0f;
        Vector3 endCamPos = new Vector3(-1.92f, 1.2f, -1.98f);
        currCamPos = camIceSpellCamTwo.transform.position;
        while (t < 0.75f)
        {
            float normT = t / 0.75f;
            camIceSpellCamTwo.transform.position = Vector3.Lerp(currCamPos, endCamPos, normT);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.66f);

        Vector3 originalPos = camIceSpellCamTwo.transform.localPosition;
        Vector3 originalPos2 = iceTrailLead.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < 0.5f)
        {
            float x = Random.Range(-1f, 1f) * 0.1f;
            float y = Random.Range(-1f, 1f) * 0.1f;

            camIceSpellCamTwo.transform.localPosition = originalPos + new Vector3(x, y, 0);
            iceTrailLead.transform.localPosition = originalPos2 + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        camIceSpellCamTwo.transform.localPosition = originalPos;
        iceTrailLead.transform.localPosition = originalPos2;
        yield return new WaitForSeconds(1.5f);
        enemy.GetComponent<Animator>().ResetTrigger("EnemyAttacked");
        t = 0f;
        originalPos = fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale;
        originalPos2 = fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale;
        while (t < 0.5f)
        {
            float normT = (0.5f - t) / 0.5f;
            fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = originalPos2 * normT;
            fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale =  originalPos * normT;
            t += Time.deltaTime;
            yield return null;
        }
        fireColumnInstance.GetComponent<FireColumnController>().FlameEmitter.transform.localScale = Vector3.zero;
        fireColumnInstance.GetComponent<FireColumnController>().EmberEmitter.transform.localScale =  Vector3.zero;
        yield return new WaitForSeconds(0.125f);
        floorCircle.GetComponent<Magic_Circle>().DestroyCircle();
        enemy.GetComponent<Animator>().SetTrigger("EnemyAttackedToIdle");
        yield return new WaitForSeconds(0.875f);
        Destroy(fireColumnInstance);
        Destroy(floorCircle);
        camFrogZoom.transform.rotation = startRot;

        camGameplay.Priority = 20;
        camIceSpellCamTwo.Priority = 10;
        camFrogZoom.Priority = 10;

        /*
        CinemachineImpulseSource ShakeEffDriver = gameObject.AddComponent<CinemachineImpulseSource>();
        CinemachineImpulseDefinition ShakeEffDef = new CinemachineImpulseDefinition();
        ShakeEffDef.m_ImpulseDuration = 2;
        ShakeEffDef.m_AmplitudeGain = 1f;
        ShakeEffDef.m_FrequencyGain = 20;
        ShakeEffDef.m_RawSignal = Resources.Load<SignalSourceAsset>("Noise/6D Shake");
        ShakeEffDef.m_PropagationSpeed = 343f;
        ShakeEffDef.m_DissipationMode = CinemachineImpulseManager.ImpulseEvent.DissipationMode.LinearDecay;
        ShakeEffDef.m_DissipationDistance = 30;
        ShakeEffDriver.m_ImpulseDefinition = ShakeEffDef;
        if (ShakeEffDef.m_RawSignal == null)
        {
            Debug.Log("Couldnt find ts brochacho");
        }

        ShakeEffDriver.GenerateImpulse(3f);
        yield return new WaitForSeconds(2f);
        */

    }

    public void PerformAttackIce()
    {
        switch (currentMoveType)
        {
            case Move.Effect.Weak:
                PerformAttackIceWeak();
                break;
            case Move.Effect.Strong:
                PerformAttackIceStrong();
                // Implement strong ice attack logic here
                break;
            case Move.Effect.Status:
                // Implement status ice attack logic here
                break;
            case Move.Effect.Shield:
                // Implement shield ice attack logic here
                break;
            case Move.Effect.WildCard:
                // Implement wild card ice attack logic here
                break;
            default:
                Debug.LogWarning("Ice attack effect not implemented: " + currentMoveType);
                break;
        }
    }

    private void PerformAttackIceStrong()
    {
        Vector3 startPos = new Vector3(2.27f, 5.46f, 0f);
        Vector3 endPos = enemy.transform.position;
        StartCoroutine(ZoomCamInSpellCast());
        StartCoroutine(AttackIceStrong(startPos, endPos));
    }

    IEnumerator AttackIceStrong(Vector3 startPos, Vector3 endPos)
    {
        iceTrailLead.transform.position = new Vector3(3.75f, 3f, 0f);
        //camIceSpellCamTwo.transform.position = new Vector3(4f, 2.4f, -7.5f);
        camIceSpellCamTwo.transform.position = new Vector3(8f, 1.2f, -5f);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Attack Trigger Set");
        animator.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(0.5f); //1.66s left
        Quaternion magicCircleRotTo = Quaternion.Euler(-45f, -90f, 0f);
        Vector3 magicCircleScale = new Vector3(0.153225f, 0.153225f, 0.153225f);
        magicCircleScale = magicCircleScale * 1.5f;

        Vector3 initialPosition = magicCircleInstance.transform.position;
        Quaternion initialRotation = magicCircleInstance.transform.rotation;
        Vector3 initialScale = magicCircleInstance.transform.localScale;
        camIceSpellCamTwo.Priority = 20;
        camFrogZoom.Priority = 10;
        camGameplay.Priority = 10;

        float d = 1f;
        float t = 0f;
        while (t < d)
        {
            float normalizedTime = t / d;

            magicCircleInstance.transform.position = Vector3.Lerp(initialPosition, startPos, normalizedTime);
            magicCircleInstance.transform.rotation = Quaternion.Lerp(initialRotation, magicCircleRotTo, normalizedTime);
            magicCircleInstance.transform.localScale = Vector3.Lerp(initialScale, magicCircleScale, normalizedTime);

            t += Time.deltaTime;
            yield return null;
        }

        magicCircleInstance.transform.position = startPos;
        magicCircleInstance.transform.rotation = magicCircleRotTo;
        magicCircleInstance.transform.localScale = magicCircleScale;
        yield return new WaitForSeconds(0.66f);
        strongIceInstance = Instantiate(strongIce, startPos, Quaternion.Euler(45f, 90f, 90f));
        strongIceInstance.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        strongIceInstance.GetComponent<StrongIceController>().ShootShard(endPos);
        yield return new WaitForSeconds(0.375f);
        //set trigger
        enemy.GetComponent<Animator>().SetTrigger("EnemyAttacked");
        yield return new WaitForSeconds(0.125f);

        StartCoroutine(DestroyMagicCircle());

        yield return new WaitForSeconds(0.25f);
        enemy.GetComponent<Animator>().ResetTrigger("EnemyAttacked");

        camGameplay.Priority = 20;
        camIceSpellCamTwo.Priority = 10;
        camFrogZoom.Priority = 10;
        camFrogZoom.transform.rotation = startRot;
        yield return new WaitForSeconds(1f);
        strongIceInstance.GetComponent<StrongIceController>().DestroyIce();
        yield return new WaitForSeconds(0.5f);
        enemy.GetComponent<Animator>().SetTrigger("EnemyAttackedToIdle");
        Destroy(strongIceInstance);
        yield return new WaitForSeconds(0.5f);
    }

    private void PerformAttackIceWeak()
    {
        camIceSpellCamTwo.transform.position = new Vector3(3f, 1.2f, -2.5f);
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
        //camFrogAttack.Priority = 10;
        camIceSpellCamTwo.Priority = 10;
        //EnemyHitCam.Priority = 10;
        // MidIceCam.Priority = 10;
        yield return new WaitForSeconds(0.75f);
    }

    IEnumerator ZoomCamInSpellCast()
    {
        camFrogZoom.Priority = 20;
        camGameplay.Priority = 10;
        startRot = camFrogZoom.transform.rotation;
        Quaternion endRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(-20f, 0f, 0f));
        StartCoroutine(SpawnMagicCircle());
        float t = 0f;
        while (t < 1f)
        {
            camFrogZoom.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SpawnMagicCircle()
    {
        yield return new WaitForSeconds(0.5f);
        magicCircleInstance = Instantiate(MagicCirclePrefabs[(int) currentAttackType], CirclePos, CircleRot);
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
        camIceSpellCamTwo.transform.position = new Vector3(3f, 1.2f, -2.5f);
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

        StartCoroutine(DestroyMagicCircle());

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
        //camFrogAttack.Priority = 10;
        camIceSpellCamTwo.Priority = 10;
        //EnemyHitCam.Priority = 10;
        //MidIceCam.Priority = 10;
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

    IEnumerator DestroyMagicCircle()
    {
        yield return new WaitForSeconds(0.5f);
        if (magicCircleInstance != null)
        {
            magicCircleInstance.GetComponent<Magic_Circle>().DestroyCircle();
            yield return new WaitForSeconds(1f);
            Destroy(magicCircleInstance, 0.125f);
        }
    }

    public void SetMove(Move.Type moveType, Move.Effect effect)
    {
        currentAttackType = moveType;
        currentMoveType = effect;
    }

    /*
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
}