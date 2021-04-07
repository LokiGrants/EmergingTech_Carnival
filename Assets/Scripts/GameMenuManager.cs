using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    #region Singleton
    public bool persistOnSceneLoad;
    private static GameMenuManager _instance;

    public static GameMenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameMenuManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameMenuManager).Name);
                    _instance = singleton.AddComponent<GameMenuManager>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as GameMenuManager;
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

    public Transform playerTransform;
    public Transform minigameWhacka;
    public Transform minigameBasket;
    public Transform minigameTarget;
    public Transform minigameSimon;

    public List<GameObject> buttonsMinigames;

    private bool canBuyNextMinigame;
    private ScoreManager sm;

    private void Start()
    {
        sm = ScoreManager.Instance;
    }

    public void BuyOrTeleportMinigame(string playerPrefsUnlockedName)
    {
        switch (playerPrefsUnlockedName)
        {
            case "UnlockedWhacka":
                if (!sm.unlockedWhacka)
                {
                    sm.UnlockLevel(playerPrefsUnlockedName);
                    playerTransform.position = minigameWhacka.position;
                } else if (playerTransform.position == minigameWhacka.position)
                {
                    WhackMole_Manager.Instance.StartWhacka();
                } else
                {
                    playerTransform.position = minigameWhacka.position;
                }
                break;
            case "UnlockedTarget":
                if (!sm.unlockedTarget)
                {
                    sm.UnlockLevel(playerPrefsUnlockedName);
                    playerTransform.position = minigameTarget.position;
                }
                else if (playerTransform.position == minigameTarget.position)
                {
                    TargetRangeManager.Instance.StartTargetRange();
                }
                else
                {
                    playerTransform.position = minigameTarget.position;
                }
                break;
            case "UnlockedBasket":
                if (!sm.unlockedBasket)
                {
                    sm.UnlockLevel(playerPrefsUnlockedName);
                    playerTransform.position = minigameBasket.position;
                }
                else if (playerTransform.position == minigameBasket.position)
                {
                    BallThrowTestGame_Manager.Instance.StartBallThrow();
                }
                else
                {
                    playerTransform.position = minigameBasket.position;
                }
                break;
            case "UnlockedSimon":
            default:
                if (!sm.unlockedSimon)
                {
                    sm.UnlockLevel(playerPrefsUnlockedName);
                    playerTransform.position = minigameSimon.position;
                }
                else if (playerTransform.position == minigameSimon.position)
                {
                    SimonSaysManager.Instance.StartSimon();
                }
                else
                {
                    playerTransform.position = minigameSimon.position;
                }
                break;
        }
    }

    public void CanBuyMinigame()
    {
        foreach(GameObject go in buttonsMinigames)
        {
            go.GetComponent<Button>().enabled = true;
            go.GetComponent<Image>().color = go.GetComponent<LaserButton>().defaultColor;
        }
    }

    public void CanNotBuyMinigame()
    {
        foreach (GameObject go in buttonsMinigames)
        {
            go.GetComponent<Button>().enabled = false;
            go.GetComponent<Image>().color = go.GetComponent<LaserButton>().disabledColor;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
