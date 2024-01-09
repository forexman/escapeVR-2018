using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using System.Linq;
using System;
using Valve.VR.InteractionSystem;

public class GameDirector : MonoBehaviour
{
    #region VARIABLES
    //Instance
    public static GameDirector instance;

    //STEAMVR
    GameObject m_rightController, m_leftController, m_hmd;
    SteamVR_PlayArea m_steamVRPlayArea;

    //UI/SFX
    public Text m_txtSub, m_txtHint;
    public AudioSource Bg_audio, Sfx_audio;
    private bool subActive = false;
    private IEnumerator coroutine;
    
    // When enabled the Action name will be displayed everytime one action is satisfied
    public bool verbose;

    // Actions Setup
    Environment m_e;
    public List<SimpleAction> m_currentActions;
    #endregion

    #region LIFECYCLE
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetupSteamVR2();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Player[] myPlayers = FindObjectsOfType(typeof(Player)) as Player[];
        m_e = gameObject.AddComponent<Environment>();
        m_e.initEnvironment(m_leftController, m_rightController, m_hmd, myPlayers[0].gameObject);

        /*foreach (SimpleAction action in m_currentActions) {
            if (verbose) Debug.Log("Activating: " + action.GetActionName());
            action.Activate(m_e);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        ///Action Controlls
        // check all current actions of they are satisfied and queue them if they are
        List<SimpleAction> queue = new List<SimpleAction>();
        foreach (SimpleAction action in m_currentActions)
        {
            if (action.Check(m_e))
                queue.Add(action);
        }
        // remove all queued actions and add their successors
        ActivateNextActions(queue);

        //Floor controls
        CheckFloor();
    }
    #endregion

    #region STEAMVR FUNCS
    void SetupSteamVR2()
    {

        if (m_steamVRPlayArea == null)
            m_steamVRPlayArea = FindObjectOfType<SteamVR_PlayArea>();

        if (m_steamVRPlayArea != null)
        {
            // HMD
            m_hmd = m_steamVRPlayArea.GetComponentInChildren<Camera>().gameObject;

            // AUDIO
            foreach (Transform child in m_hmd.transform)
            {
                if (child.name == "BG_Audio")
                {
                    Bg_audio = child.GetComponent<AudioSource>();
                    //Debug.Log("Setting BG Vol of: " + Bg_audio.volume);
                    Bg_audio.volume = ApplicationSettings.instance.BgVol;
                }
                else if (child.name == "SFX_Audio")
                {
                    Sfx_audio = child.GetComponent<AudioSource>();
                    Sfx_audio.volume = ApplicationSettings.instance.SfxVol;
                }
                else if (child.name == "HelmentCanvas")
                {
                    foreach (Transform chil in child)
                    {
                        if (chil.name == "Subtitles")
                        {
                            m_txtSub = chil.GetComponent<Text>();
                        }
                        else if (chil.name == "Hints")
                        {
                            m_txtHint = chil.GetComponent<Text>();
                        }
                    }
                }
            }

            // CONTROLLERS
            foreach (SteamVR_Behaviour_Pose poseComp in m_steamVRPlayArea.GetComponentsInChildren<SteamVR_Behaviour_Pose>(true))
            {
                if (poseComp.inputSource == SteamVR_Input_Sources.RightHand)
                    m_rightController = poseComp.gameObject;
                else if (poseComp.inputSource == SteamVR_Input_Sources.LeftHand)
                    m_leftController = poseComp.gameObject;
            }
        }
        else
        {
            Debug.LogError("Can't find SteamVR_PlayArea component or InteractionSystem.Player component on the scene. One of these is required. Add a reference to it manually to CurvedUIInputModule on EventSystem gameobject.", this.gameObject);
        }
    }
    #endregion

    #region ACTIONS FUNCS

    public void ActivateNextActions(List<SimpleAction> queue)
    {
        foreach (SimpleAction action in queue)
        {
            if (verbose) Debug.Log("Action " + action.GetActionName() + " is completed!");

            // remove from current actions
            m_currentActions.Remove(action);

            // add all new actions and activate them
            foreach (SimpleAction newAction in action.GetNextActions())
            {
                m_currentActions.Add(newAction);
                if (verbose) Debug.Log("Activating: " + newAction.GetActionName());
                newAction.Activate(m_e);
            }
        }
    }

    public void ActivateAction(SimpleAction action)
    {
        m_currentActions.Add(action);
        if (verbose) Debug.Log("Activating: " + action.GetActionName());
        action.Activate(m_e);
    }
    #endregion

    #region HELPER FUNCS
    public void ShowSubtitle(string txt, float dur)
    {
        if (subActive) StopCoroutine(coroutine);
        m_txtSub.text = txt;
        coroutine = HideSubtitle(dur + 1);
        StartCoroutine(coroutine);
    }
    IEnumerator HideSubtitle(float dur)
    {
        subActive = true;
        yield return new WaitForSeconds(dur);
        subActive = false;
        m_txtSub.text = "";
    }

    public void ShowHint(string txt)
    {
        m_txtHint.text = txt;
    }

    public void CheckFloor()
    {
        if (!StageController.instance.tPoints[1].GetComponent<TeleportPoint>().ShouldActivate(m_e.getPlayerPositions().PlayerPosition))
        {
            //Debug.Log("Used TP Ground Floor");
            StageController.instance.ActivateGroundFloorTeleports();
        }
        else if (!StageController.instance.tPoints[2].GetComponent<TeleportPoint>().ShouldActivate(m_e.getPlayerPositions().PlayerPosition))
        {
            //Debug.Log("Used TP Upper Floor");
            StageController.instance.ActivateUpperFloorTeleports();
        }
    }
    #endregion
}
