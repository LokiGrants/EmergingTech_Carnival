using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameMenuOpener : MonoBehaviour
{
    public GameObject canvas;
    public GameObject laserPointer;
    public Transform rightHand;
    public float countdownTotalTime = 1f;
    public float cubeSize;

    private bool turnedOn;
    private Coroutine countdownCoroutine;

    public void StartCountDown()
    {
        if (!turnedOn)
        {
            countdownCoroutine = StartCoroutine(CountToTurnOn());
        }
    }

    public void StopCountDown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = null;
    }
    IEnumerator CountToTurnOn()
    {
        yield return new WaitForSecondsRealtime(countdownTotalTime);
        turnedOn = true;
        canvas.SetActive(true);
        laserPointer.SetActive(true);
    }

    public void CloseWindow()
    {
        StopCountDown();
        turnedOn = false;
        canvas.SetActive(false);
        laserPointer.SetActive(false);
    }
}
