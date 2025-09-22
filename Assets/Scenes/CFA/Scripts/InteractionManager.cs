using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    
    public static InteractionManager interaction;
    public bool LockPlayerControls;
    public bool LocakAllInteraction;
    public GameObject OutsideFade;
    public GameObject InnerFade;
    public GameObject EdgeDecor;
    public GameObject StatsScreen;
    public GameObject player;

    void Awake()
    {
        if (interaction == null)
        {
            interaction = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        OutsideFade.SetActive(false);
        InnerFade.SetActive(false);
        EdgeDecor.SetActive(false);
        StatsScreen.SetActive(false);
        LockPlayerControls = false;
        LocakAllInteraction = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!LockPlayerControls && !LocakAllInteraction)
            {
                LocakAllInteraction = true;
                LockPlayerControls = true;
                OutsideFade.SetActive(false);
                StartCoroutine(FadeUIIn());
            }
        }

    }

    IEnumerator FadeUIIn()
    {
        InnerFade.transform.localScale = new Vector3(0.66f, 0, 0.5f);
        //Color innerColor = InnerFade.GetComponent<Image>().color;
        //innerColor.a = 0;
        //InnerFade.GetComponent<Image>().color = innerColor;
        EdgeDecor.SetActive(false);
        StatsScreen.SetActive(false);
        Color outsideColor = OutsideFade.GetComponent<Image>().color;
        outsideColor.a = 0;
        OutsideFade.GetComponent<Image>().color = outsideColor;
        OutsideFade.SetActive(true);
        float timer = 0f;
        StartCoroutine(ExpandInnerPanel());
        while (timer < 0.25f)
        {
            timer += Time.deltaTime;
            outsideColor.a = Mathf.Lerp(0, 206.0f / 255.0f, timer / 0.25f);
            OutsideFade.GetComponent<Image>().color = outsideColor;
            yield return null;
        }
        outsideColor.a = 206.0f / 255.0f;
        OutsideFade.GetComponent<Image>().color = outsideColor;

    }

    public TMP_Text nameText;
    public TMP_Text HPText;
    public TMP_Text AtkText;
    public TMP_Text DefText;
    public TMP_Text ManaText;
    public TMP_Text AgiText;
    public TMP_Text LuckText;
    public TMP_Text TypeText;

    IEnumerator ExpandInnerPanel()
    {
        InnerFade.transform.localScale = new Vector3(0.66f, 0, 0.5f);
        InnerFade.SetActive(true);
        yield return new WaitForSeconds(0.125f);
        float t = 0f;
        while (t < 0.25f)
        {
            t += Time.deltaTime;
            InnerFade.transform.localScale = new Vector3(0.66f, Mathf.Lerp(0, 0.66f, t / 0.25f), 0.5f);
            yield return null;

        }
        InnerFade.transform.localScale = new Vector3(0.66f, 0.66f, 0.5f);
        nameText.text = "";
        HPText.text = "";
        AtkText.text = "";
        DefText.text = "";
        ManaText.text = "";
        AgiText.text = "";
        LuckText.text = "";
        TypeText.text = "";
        StatsScreen.SetActive(true);
        StartCoroutine(Typewriter(nameText, player.GetComponent<chibiplayerattributes>().GetName(), 0.05f));
        int[] stats = player.GetComponent<chibiplayerattributes>().ReturnVisualStats();
        StartCoroutine(Typewriter(HPText, "HP: " + stats[0], 0.05f));
        StartCoroutine(Typewriter(AtkText, "Atk: " + stats[1], 0.05f));
        StartCoroutine(Typewriter(DefText, "Def: " + stats[2], 0.05f));
        StartCoroutine(Typewriter(ManaText, "Mana: " + stats[3], 0.05f));
        StartCoroutine(Typewriter(AgiText, "Agility: " + stats[4], 0.05f));
        StartCoroutine(Typewriter(LuckText, "Luck: " + stats[5], 0.05f));
        StartCoroutine(Typewriter(TypeText, "Type: " + SystemInfo.ElementTypeToString[player.GetComponent<chibiplayerattributes>().GetElementType()], 0.05f));
        yield return new WaitForSeconds(1.0f);
        LocakAllInteraction = false;
    }

    IEnumerator Typewriter(TMP_Text textComponent, string fullText, float delay)
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
