using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager<T> : MonoBehaviour where T : MiniGameManager<T>
{
    #region Singleton
    public bool persistOnSceneLoad;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    _instance = singleton.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
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

    public float timeOfGameplayInSeconds = 30;
    public float stepTime = 0.1f;

    protected void StartMinigame()
    {
        StartCoroutine(MiniGameController(timeOfGameplayInSeconds));
    }

    IEnumerator MiniGameController(float totalGameTime)
    {
        while (totalGameTime > 0)
        {
            BeforeYield(totalGameTime);
            yield return new WaitForSeconds(stepTime);
            totalGameTime -= stepTime;
            AfterYield(totalGameTime);
        }

        AfterWhile(totalGameTime);
    }

    protected virtual void AfterWhile(float totalGameTime)
    {

    }


    protected virtual void BeforeYield(float totalGameTime)
    {

    }
    

    protected virtual void AfterYield(float totalGameTime)
    {

    }
}
