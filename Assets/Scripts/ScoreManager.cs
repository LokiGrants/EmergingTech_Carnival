﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    public bool persistOnSceneLoad;
    private static ScoreManager _instance;

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(ScoreManager).Name);
                    _instance = singleton.AddComponent<ScoreManager>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as ScoreManager;
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

    public int firstLevelPrice;

    public int currentScore;
    public int highScoreWhacka;
    public int highScoreTarget;
    public int highScoreBasket;
    public int highScoreSimon;

    public bool unlockedWhacka = false;
    public bool unlockedTarget = false;
    public bool unlockedBasket = false;
    public bool unlockedSimon = false;

    private float lastPriceAsked;
    private GameMenuManager gmm;

    void Start()
    {
        gmm = GameMenuManager.Instance;

        if (PlayerPrefs.HasKey("CurrentScore"))
        {
            currentScore = PlayerPrefs.GetInt("CurrentScore");
        }
        GameMenuOpener.Instance.textScore.text = currentScore.ToString();

        if (PlayerPrefs.HasKey("HighScoreWhacka"))
        {
            highScoreWhacka = PlayerPrefs.GetInt("HighScoreWhacka");
        }

        if (PlayerPrefs.HasKey("HighScoreTarget"))
        {
            highScoreTarget = PlayerPrefs.GetInt("HighScoreTarget");
        }

        if (PlayerPrefs.HasKey("HighScoreBasket"))
        {
            highScoreBasket = PlayerPrefs.GetInt("HighScoreBasket");
        }

        if (PlayerPrefs.HasKey("HighScoreSimon"))
        {
            highScoreSimon = PlayerPrefs.GetInt("HighScoreSimon");
        }

        if (PlayerPrefs.HasKey("UnlockedWhacka"))
        {
            unlockedWhacka = PlayerPrefs.GetString("UnlockedWhacka") == "true";
        }

        if (PlayerPrefs.HasKey("UnlockedTarget"))
        {
            unlockedTarget = PlayerPrefs.GetString("UnlockedTarget") == "true";
        }

        if (PlayerPrefs.HasKey("UnlockedBasket"))
        {
            unlockedBasket = PlayerPrefs.GetString("UnlockedBasket") == "true";
        }

        if (PlayerPrefs.HasKey("UnlockedSimon"))
        {
            unlockedSimon = PlayerPrefs.GetString("UnlockedSimon") == "true";
        }

        NextLevelPrice();

        if (currentScore >= firstLevelPrice * lastPriceAsked)
        {
            gmm.CanBuyMinigame();
        } else
        {
            gmm.CanNotBuyMinigame();
        }
    }

    public void UnlockLevel(string playerPrefsUnlockedName)
    {
        switch (playerPrefsUnlockedName)
        {
            case "UnlockedWhacka":
                unlockedWhacka = true;
                break;
            case "UnlockedTarget":
                unlockedTarget = true;
                break;
            case "UnlockedBasket":
                unlockedBasket = true;
                break;
            case "UnlockedSimon":
            default:
                unlockedSimon = true;
                break;
        }

        currentScore -= (int)(firstLevelPrice * lastPriceAsked);
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        GameMenuOpener.Instance.textScore.text = currentScore.ToString();
        PlayerPrefs.SetString(playerPrefsUnlockedName, "true");

        if (firstLevelPrice * NextLevelPrice() >= currentScore)
        {
            gmm.CanNotBuyMinigame();
        }
    }

    public void AddCurrentScore(int score)
    {
        currentScore += score;
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        GameMenuOpener.Instance.textScore.text = currentScore.ToString();

        if (currentScore >= firstLevelPrice * NextLevelPrice())
        {
            gmm.CanBuyMinigame();
        }
    }

    public float NextLevelPrice()
    {
        float times = 1;

        if (unlockedWhacka)
            times *= 1.5f;

        if (unlockedTarget)
            times *= 1.5f;

        if (unlockedBasket)
            times *= 1.5f;

        if (unlockedSimon)
            times *= 1.5f;

        lastPriceAsked = times;
        return times;
    }
}
