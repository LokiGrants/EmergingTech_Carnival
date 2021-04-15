using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationDataAndController : MonoBehaviour
{
    public float animationTime;
    public GameObject parentRoot;
    public AudioSource soundToPlay;
    private TextMeshProUGUI text;
    void Awake()
    {
        Invoke("FinishHim", animationTime);
        text = GetComponent<TextMeshProUGUI>();
        transform.LookAt(Camera.main.transform);
        //transform.LookAt(GameMenuManager.Instance.playerTransform);
    }

    public void ScoreValueChange(string value)
    {
        text.text = value;
    }

    public void FinishHim()
    {
        Destroy(parentRoot);
    }

    public void PlayAudio()
    {
        soundToPlay.Play();
    }
}
