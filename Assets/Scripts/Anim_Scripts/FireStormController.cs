
using UnityEngine;

public class FireStormController : MonoBehaviour
{
    public GameObject EmberEffect;
    public GameObject FireStorm;
    public GameObject Smoke;
    public GameObject Ashes; //on the fire!?!?!?
    Vector3 origEmbScale;
    Vector3 origFireScale;
    Vector3 origSmokebScale;
    Vector3 origAshesScale;

    void Awake()
    {
        origEmbScale = EmberEffect.transform.localScale;
        origFireScale = FireStorm.transform.localScale;
        origSmokebScale = Smoke.transform.localScale;
        origAshesScale = Ashes.transform.localScale;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Scale(Vector3 scale)
    {
        EmberEffect.transform.localScale = new Vector3(scale.x * origEmbScale.x, scale.y * origEmbScale.y, scale.z * origEmbScale.z);
        FireStorm.transform.localScale = new Vector3(scale.x * origFireScale.x, scale.y * origFireScale.y, scale.z * origFireScale.z);
        Smoke.transform.localScale = new Vector3(scale.x * origSmokebScale.x, scale.y * origSmokebScale.y, scale.z * origSmokebScale.z);
        Ashes.transform.localScale = new Vector3(scale.x * origAshesScale.x, scale.y * origAshesScale.y, scale.z * origAshesScale.z);
        
    }
}
