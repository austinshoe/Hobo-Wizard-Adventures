using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PanelActivitiesGlobal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Start of Scene
    public static IEnumerator FadeOutPanel(GameObject panel, float duration)
    {
        Image image = panel.GetComponent<Image>();
        float t = 0f;
        Color currCol = image.color;
        float startAlpha = currCol.a;
        panel.SetActive(true);
        while (t < duration)
        {
            t += Time.deltaTime;
            currCol.a = Mathf.Lerp(startAlpha, 0, t / duration);
            image.color = currCol;
            yield return null;
        }
        currCol.a = 0;
        image.color = currCol;
        panel.SetActive(false);
    }
}
