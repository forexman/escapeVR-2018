using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class StageController : MonoBehaviour
{
    #region VARIABLES
    //Instance
    public static StageController instance;

    public bool verbose = false;

    //List of Stages
    List<GameObject> m_stages;
    public int StageID { get; set; }
    AudioClip ac_normal, ac_shadow;
    bool m_stageTransitioning = false;
    float count;

    // Teleport Areas / Points
    public List<GameObject> tAreas, tPoints;
    GameObject teleportSystem;
    private bool bedroomActive, garageActive;
    private int cur_Floor;
    #endregion

    #region LIFECYCLE
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitLists();
        }
        else if (instance != this)
        {
            if(verbose) Debug.Log("It has a different instance, destroying...");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SteamVR_Fade.Start(Color.black, 0);
        ac_normal = Resources.Load<AudioClip>("Audio/BG/bg_normal");
        ac_shadow = Resources.Load<AudioClip>("Audio/BG/bg_shadow");
        ActivateStage(0);
    }
    #endregion

    #region teleportSystem FUNCS
    void InitteleportSystem()
    {
        garageActive = false;
        bedroomActive = false;
        cur_Floor = -1;
        tAreas = new List<GameObject>();
        tPoints = new List<GameObject>();

        //Populate
        foreach (Transform child in teleportSystem.transform)
        {
            if (child.name.Contains("Point"))
                tPoints.Add(child.gameObject);
            else
                tAreas.Add(child.gameObject);
        }

        //Sort
        if (tPoints.Count > 0)
        {
            tPoints.Sort(delegate (GameObject a, GameObject b)
            {
                return (a.GetComponent<TeleportPoint>().pointID).CompareTo(b.GetComponent<TeleportPoint>().pointID);
            });
        }

        if (tAreas.Count > 0)
        {
            tAreas.Sort(delegate (GameObject a, GameObject b)
            {
                return (a.GetComponent<TeleportArea>().areaID).CompareTo(b.GetComponent<TeleportArea>().areaID);
            });
        }

        //Disable
        foreach (GameObject child in tPoints)
            child.SetActive(false);

        foreach (GameObject child in tAreas)
            child.SetActive(false);

        teleportSystem.SetActive(false);
    }

    public void EditTeleportObject(GameObject obj, bool active)
    {
        obj.SetActive(active);
    }

    public void ActivateGroundFloorTeleports()
    {
        if (cur_Floor != 0)
        {
            cur_Floor = 0;
            for (int i = 0; i < 7; i++)
            {
                EditTeleportObject(tAreas[i], true);
            }
            for (int i = 7; i < 10; i++)
            {
                EditTeleportObject(tAreas[i], false);
            }
            EditTeleportObject(tPoints[1], false);
            EditTeleportObject(tPoints[2], true);
        
            //Garage
            if (StageID == 2)
            {
                //Dark World
                if (garageActive) EditTeleportObject(tAreas[7], true);
                else EditTeleportObject(tAreas[7], false);
            }
            else
            {
                //Normal World
                EditTeleportObject(tAreas[7], false);
            }
        }
        
    }
    public void ActivateUpperFloorTeleports()
    {
        if (cur_Floor != 1)
        {
            cur_Floor = 1;
            for (int i = 0; i < 8; i++)
            {
                EditTeleportObject(tAreas[i], false);
            }
            for (int i = 8; i < 10; i++)
            {
                EditTeleportObject(tAreas[i], true);
            }
            EditTeleportObject(tPoints[1], true);
            EditTeleportObject(tPoints[2], false);

            if (StageID == 2)
            {
                //Dark World
                if (bedroomActive) EditTeleportObject(tAreas[10], true);
                else EditTeleportObject(tAreas[10], false);
            }
            else
            {
                //Normal World
                EditTeleportObject(tAreas[10], true);
            }
        }
    }

    public void ActivateBedroomTeleport(bool flag)
    {
        bedroomActive = flag;
        EditTeleportObject(tAreas[10], flag);
    }
    public void ActivateGarageTeleport(bool flag)
    {
        garageActive = flag;
        EditTeleportObject(tAreas[7], flag);
    }
    #endregion

    #region HELPER FUNCTS
    void InitLists()
    {
        m_stages = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name == "TeleportAreas")
            {
                teleportSystem = child.gameObject;
                InitteleportSystem();
            }
            else
                m_stages.Add(child.gameObject);
        }
    }

    public void ActivateStage(int i)
    {
        SteamVR_Fade.Start(Color.clear, 2);
        //count = Time.realtimeSinceStartup;

        m_stages[0].SetActive(false);
        m_stages[1].SetActive(false);
        m_stages[2].SetActive(false);
        m_stages[3].SetActive(false);

        switch (i)
        {
            case 0:
                m_stages[0].SetActive(true);
                StageID = 0;
                GameDirector.instance.Bg_audio.clip = ac_normal;
                GameDirector.instance.Bg_audio.Play();
                break;
            case 1:
                m_stages[1].SetActive(true);
                StageID = 1;
                GameDirector.instance.Bg_audio.clip = ac_normal;
                GameDirector.instance.Bg_audio.Play();
                if (GameDirector.instance.m_currentActions.Count == 0) GameDirector.instance.ActivateAction(FindObjectOfType<Intro_MoveToDoor>());
                break;
            case 2:
                //Debug.Log("Disabling took: " + (Time.realtimeSinceStartup - count));
                //count = Time.realtimeSinceStartup;

                m_stages[2].SetActive(true);
                //Debug.Log("Activating took: " + (Time.realtimeSinceStartup - count));
                //count = Time.realtimeSinceStartup;
                StageID = 2;
                GameDirector.instance.Bg_audio.clip = ac_shadow;
                GameDirector.instance.Bg_audio.Play();
                //Debug.Log("BG took: " + (Time.realtimeSinceStartup - count));

                if (GameDirector.instance.m_currentActions.Count == 0) GameDirector.instance.ActivateAction(FindObjectOfType<Bed_GoToBed>());
                break;
            case 3:
                m_stages[1].SetActive(true);
                m_stages[3].SetActive(true);
                GameDirector.instance.Bg_audio.clip = ac_normal;
                GameDirector.instance.Bg_audio.Play();
                StageID = 3;
                GameDirector.instance.m_currentActions.Clear();
                if (GameDirector.instance.m_currentActions.Count == 0) GameDirector.instance.ActivateAction(FindObjectOfType<Outro_FindPhone>());
                break;
        }

        teleportSystem.SetActive(true);

    }


    IEnumerator LoadYourAsyncSceneSingle(int iLoad)
    {
        //Debug.Log("Starting Loading Scene");
        teleportSystem.SetActive(false);

        GameDirector.instance.Bg_audio.Stop();
        m_stageTransitioning = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(iLoad, LoadSceneMode.Single);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //Debug.Log("Loading Scene Incomplete");
            yield return null;
        }
        //Debug.Log("Loading Scene Done");
        m_stageTransitioning = false;
        //SteamVR_Fade.Start(Color.clear, 1);
        if (verbose) Debug.Log("Loading Scene took: " + (Time.realtimeSinceStartup - count));
        switch (StageID)
        {
            case 0:
                ActivateStage(1);
                break;
            case 1:
                ActivateStage(2);
                break;
            case 2:
                ActivateStage(3);
                break;
        }
    }

    /*
    IEnumerator LoadYourAsyncScene(int iLoad, int iUnload)
    {
        Debug.Log("Starting Loading Scene");

        m_stageTransitioning = true;
        GameDirector.instance.Bg_audio.Stop();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(iLoad, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading Scene Incomplete");
            yield return null;
        }
        Debug.Log("Loading Scene Done");
        StartCoroutine(UnloadYourAsyncScene(iUnload));
    }
    IEnumerator UnloadYourAsyncScene(int iUnload)
    {
        Debug.Log("Unloading Scene...");
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncUnload.isDone)
        {
            Debug.Log("Unloading Scene Incomplete");
            yield return null;
        }
        Debug.Log("Unloading Scene Done");
        m_stageTransitioning = false;
        //ActivateStage(1);
        SteamVR_Fade.Start(Color.clear, 1);
        Debug.Log("Execution took: " + (Time.realtimeSinceStartup - count));

    }
    */

    public void MoveToShadowRealm()
    {
        count = Time.realtimeSinceStartup;
        StartCoroutine(LoadYourAsyncSceneSingle(2));
    }

    public void MoveToNormalRealm()
    {
        count = Time.realtimeSinceStartup;
        StartCoroutine(LoadYourAsyncSceneSingle(1));
    }

    public void EndCredits()
    {
        GameDirector.instance.m_txtHint.text = "You solved the mystery and\n brought peace to a restless spirit...";
        GameDirector.instance.m_txtSub.text = "You are free to enjoy your new house!";
    }

    #endregion
}
