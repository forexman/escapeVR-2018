using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    #region VARIABLES
    //Instance for sharing
    public static ApplicationSettings instance;

    //Vars
    public bool HasSubtitles = true;
    public bool HasHints = true;
    public float SfxVol = 0.5f;
    public float BgVol = 0.5f;
    public int HintThres = 1;
    #endregion

    #region LIFECYCLE
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
