using System.Collections;
using UnityEngine;

public class IceController : MonoBehaviour
{
    private const float SLIDE_TIME = 0.20f; // Time for the ice block to slide up and out
    private const int SLIDE_FRAMES = 10;
    float initHeight;
    float heightDisplaced;
    public GameObject particlePrefab;

    public GameObject iceShardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void RegisterInitHeight(float initHeight, float heighDisplaced)
    {
        this.initHeight = initHeight;
        this.heightDisplaced = heighDisplaced;
        BirthAndDeath();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void BirthAndDeath()
    {
        StartCoroutine(SlideUpAndOut());
    }
    IEnumerator SlideUpAndOut()
    {

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0.1f);
        GameObject particleObj = Instantiate(particlePrefab, pos, Quaternion.identity);

        ParticleSystem ps = particleObj.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }

        for (int i = 0; i < SLIDE_FRAMES; i++)
        {
            float yBlock = heightDisplaced / SLIDE_FRAMES;
            transform.position += yBlock / transform.up.y * transform.up;
            yield return new WaitForSeconds(SLIDE_TIME / SLIDE_FRAMES);
        }

        yield return new WaitForSeconds(0.5f); //wait before initializing fadeout

        // FADE OUT GLOW

        Renderer rend = GetComponent<Renderer>();
        Material mat = rend.material;
        Color emissionCol = mat.GetColor("_EmissionColor");
        float redStep = emissionCol.r / SLIDE_FRAMES;
        float greenStep = emissionCol.g / SLIDE_FRAMES;
        float blueStep = emissionCol.b / SLIDE_FRAMES;
        for (int i = 0; i < SLIDE_FRAMES - 2; i++)
        {
            Color emissionColor = mat.GetColor("_EmissionColor");
            emissionColor.r = Mathf.Clamp(emissionColor.r, 0f, 1f);
            emissionColor.g = Mathf.Clamp(emissionColor.g, 0f, 1f);
            emissionColor.b = Mathf.Clamp(emissionColor.b, 0f, 1f);
            emissionColor.r -= redStep;
            emissionColor.g -= greenStep;
            emissionColor.b -= blueStep;
            mat.SetColor("_EmissionColor", emissionColor);
            yield return new WaitForSeconds(0.75f / SLIDE_FRAMES);
        }
        yield return new WaitForSeconds(2 * 0.75f / SLIDE_FRAMES);
        // mat.DisableKeyword("_EMISSION");
    }

    public void DestroyIce()
    {
        Vector3 createPos = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        GameObject ShardEff = Instantiate(iceShardPrefab, transform.position, Quaternion.identity);
        ShardEff.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DieInAHoleBrochacho());
    }
    
    IEnumerator DieInAHoleBrochacho()
    {
        yield return new WaitForSeconds(0.125f);
        Destroy(gameObject);
    }
}
