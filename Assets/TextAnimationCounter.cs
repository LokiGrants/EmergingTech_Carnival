using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimationCounter : MonoBehaviour
{
    public TextMeshPro textCounter;
    public TextMeshPro textStart;
    public Color originalColor;

    private Vector3 originalScale;

    private void Start()
    {
        textCounter.text = "";
        textStart.text = "";

        originalScale = new Vector3(textStart.transform.localScale.x, textStart.transform.localScale.y, textStart.transform.localScale.z);
    }

    public void MinigameStart()
    {
        textStart.color = originalColor;
        textCounter.color = originalColor;
        textStart.transform.localScale = originalScale;
        textStart.text = " START!!";
        textCounter.text = "30";

        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        //StartCoroutine(SecondAnimation());
        float initialValue = 1f;
        float target = 0f;
        float duration = 1f;
        float timer = 0f;

        Color color = textStart.color;

        while (timer < duration)
        {
            color.a = Mathf.Lerp(initialValue, target, timer / duration);
            textStart.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = target;
        textStart.color = color;
    }

    IEnumerator SecondAnimation()
    {
        float initialValue = textStart.transform.localScale.x;
        float target = textStart.transform.localScale.x*1.5f;
        float duration = 0.8f;
        float timer = 0f;

        float value = 0f;

        while (timer < duration)
        {
            value = Mathf.Lerp(initialValue, target, timer / duration);
            textStart.transform.localScale = new Vector3(value, value, 1f);
            timer += Time.deltaTime;
            yield return null;
        }

        value = target;
        textStart.transform.localScale = new Vector3(value, value, 1f);
    }

    public IEnumerator EndAnimation()
    {
        textCounter.text = "00";
        float initialValue = 1f;
        float target = 0f;
        float duration = 0.5f;
        float timer = 0f;

        Color color = textCounter.color;

        while (timer < duration)
        {
            color.a = Mathf.Lerp(initialValue, target, timer / duration);
            textCounter.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = target;
        textCounter.color = color;
    }

    public void ChangeText(string value)
    {
        textCounter.text = value;
    }
}
