using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameMenuOpener : MonoBehaviour
{
    #region Singleton
    public bool persistOnSceneLoad;
    private static GameMenuOpener _instance;

    public static GameMenuOpener Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameMenuOpener>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameMenuOpener).Name);
                    _instance = singleton.AddComponent<GameMenuOpener>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as GameMenuOpener;
        }
        else
        {
            Destroy(gameObject);
        }

        if (persistOnSceneLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public GameObject canvas;
    public Button confirmPurchaseButton;
    public GameObject laserPointer;
    public Transform rightHand;
    public Text textScore;
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
