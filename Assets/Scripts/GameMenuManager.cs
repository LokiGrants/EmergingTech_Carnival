using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        Initialize();
    }
    #endregion

    public Transform playerTransform;
    public Transform minigameWhacka;
    public Transform minigameBasket;
    public Transform minigameTarget;
    public Transform minigameSimon;

    public TextAnimationCounter textAnimation;

    public List<GameObject> buttonsMinigames;

    private bool canBuyNextMinigame;
    private bool isPlayingMinigame;
    private ScoreManager sm;

    [ContextMenu("delete keys")]
    public void DeleteKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Initialize()
    {
        sm = ScoreManager.Instance;
    }

    public bool IsPlayingMinigame()
    {
        return isPlayingMinigame;
    }

    public void MinigameHasStarted()
    {
        isPlayingMinigame = true;
        textAnimation.MinigameStart();
    }

    public void MinigameHasEnded()
    {
        isPlayingMinigame = false;
        StartCoroutine(textAnimation.EndAnimation());
    }

    public void BuyOrTeleportMinigame(string playerPrefsUnlockedName)
    {
        switch (playerPrefsUnlockedName)
        {
            case "UnlockedWhacka":
                if (!sm.unlockedWhacka)
                {
                    var button = buttonsMinigames.Where(x => x.name == playerPrefsUnlockedName);
                    if (button != null)
                    {
                        MenuButtonController mbc = button.First().GetComponent<MenuButtonController>();
                        mbc.OnClick();
                        GameMenuOpener.Instance.confirmPurchaseButton.onClick.AddListener(() => {
                            sm.UnlockLevel(playerPrefsUnlockedName);
                            playerTransform.position = minigameWhacka.position;
                        });
                    }
                } else if (playerTransform.position == minigameWhacka.position)
                {
                    WhackMole_Manager.Instance.StartWhacka();
                    GameMenuOpener.Instance.CloseWindow();
                } else
                {
                    playerTransform.position = minigameWhacka.position;
                }
                break;
            case "UnlockedTarget":
                if (!sm.unlockedTarget)
                {
                    var button = buttonsMinigames.Where(x => x.name == playerPrefsUnlockedName);
                    if (button != null)
                    {
                        MenuButtonController mbc = button.First().GetComponent<MenuButtonController>();
                        mbc.OnClick();
                        GameMenuOpener.Instance.confirmPurchaseButton.onClick.AddListener(() => {
                            sm.UnlockLevel(playerPrefsUnlockedName);
                            playerTransform.position = minigameTarget.position;
                        });
                    }
                }
                else if (playerTransform.position == minigameTarget.position)
                {
                    TargetRangeManager.Instance.StartTargetRange();
                    GameMenuOpener.Instance.CloseWindow();
                }
                else
                {
                    playerTransform.position = minigameTarget.position;
                }
                break;
            case "UnlockedBasket":
                if (!sm.unlockedBasket)
                {
                    var button = buttonsMinigames.Where(x => x.name == playerPrefsUnlockedName);
                    if (button != null)
                    {
                        MenuButtonController mbc = button.First().GetComponent<MenuButtonController>();
                        mbc.OnClick();
                        GameMenuOpener.Instance.confirmPurchaseButton.onClick.AddListener(() => {
                            sm.UnlockLevel(playerPrefsUnlockedName);
                            playerTransform.position = minigameBasket.position;
                        });
                    }
                }
                else if (playerTransform.position == minigameBasket.position)
                {
                    BallThrowTestGame_Manager.Instance.StartBallThrow();
                    GameMenuOpener.Instance.CloseWindow();
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
                    var button = buttonsMinigames.Where(x => x.name == playerPrefsUnlockedName);
                    if (button != null)
                    {
                        MenuButtonController mbc = button.First().GetComponent<MenuButtonController>();
                        mbc.OnClick();
                        GameMenuOpener.Instance.confirmPurchaseButton.onClick.AddListener(() => {
                            sm.UnlockLevel(playerPrefsUnlockedName);
                            playerTransform.position = minigameSimon.position;
                        });
                    }
                }
                else if (playerTransform.position == minigameSimon.position)
                {
                    SimonSaysManager.Instance.StartSimon();
                    GameMenuOpener.Instance.CloseWindow();
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
            go.GetComponent<BoxCollider>().enabled = true;
            go.GetComponent<Image>().color = go.GetComponent<LaserButton>().defaultColor;
        }
    }

    public void CanNotBuyMinigame()
    {
        foreach (GameObject go in buttonsMinigames)
        {
            switch(go.GetComponent<LaserButton>().whichIsMe)
            {
                case LaserButton.WhichMinigame.WHACKA:
                    if (!sm.unlockedWhacka)
                    {
                        go.GetComponent<BoxCollider>().enabled = false;
                        go.GetComponent<Image>().color = go.GetComponent<LaserButton>().disabledColor;
                    }
                    break;
                case LaserButton.WhichMinigame.TARGET:
                    if (!sm.unlockedTarget)
                    {
                        go.GetComponent<BoxCollider>().enabled = false;
                        go.GetComponent<Image>().color = go.GetComponent<LaserButton>().disabledColor;
                    }
                    break;
                case LaserButton.WhichMinigame.BASKET:
                    if (!sm.unlockedBasket)
                    {
                        go.GetComponent<BoxCollider>().enabled = false;
                        go.GetComponent<Image>().color = go.GetComponent<LaserButton>().disabledColor;
                    }
                    break;
                case LaserButton.WhichMinigame.SIMON:
                default:
                    if (!sm.unlockedSimon)
                    {
                        go.GetComponent<BoxCollider>().enabled = false;
                        go.GetComponent<Image>().color = go.GetComponent<LaserButton>().disabledColor;
                    }
                    break;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
