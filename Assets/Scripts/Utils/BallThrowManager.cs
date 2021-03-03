﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowManager<T> : MonoBehaviour where T : BallThrowManager<T>
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

    public int livesCount;
}
