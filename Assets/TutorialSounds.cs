using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSounds : MonoBehaviour
{
    public float waitTime; //total time for first audio
    public float repeatingAudioTime;
    public float waitTimeBetweenRepeating; //time between audios

    public AudioClip repeatingAudio;

    public Transform entrancePosition;
    public Transform playerTransform;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        InvokeRepeating("RepeatAudioUntil", waitTime, waitTimeBetweenRepeating + repeatingAudioTime);
    }

    // Update is called once per frame
    void RepeatAudioUntil()
    {
        if (playerTransform.position == entrancePosition.position)
        {
            if (source.clip != repeatingAudio)
                source.clip = repeatingAudio;

            source.Play();
        } else
        {
            CancelInvoke();
        }
    }
}
