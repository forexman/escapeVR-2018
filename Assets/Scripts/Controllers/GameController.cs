using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameController : MonoBehaviour
{
    //Instance
    public static GameController instance;

    // Steam VR objects
    //Support Variables - handheld controllers
    GameObject m_rightController, m_leftController, m_hmd;
    private SteamVR_PlayArea steamVRPlayArea;
    private bool subtitles, hints;
    private Text txtSub, txtHint;    

    // Audio Sources
    private AudioSource bg_audio, sfx_audio;
    private float bg_vol, sfx_vol;

    // When enabled the Action name will be displayed everytime one action is satisfied
    public bool debugOutput;

    // Current actions - public so following actions can be added directly in Unity's UI
    public List<AbstractAction> m_currentActions;

    Environment m_e;
    

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

    public GameController()
    {
        m_currentActions = new List<AbstractAction>();
    }

    void Start()
    {
        m_e = gameObject.AddComponent<Environment>();

        m_e.initEnvironment(m_leftController, m_rightController, m_hmd);

        foreach (AbstractAction action in m_currentActions)
            action.activate(m_e);
    }

    // Update is called once per frame
    void Update()
    {
        // check all current actions of they are satisfied and queue them if they are
        List<AbstractAction> queue = new List<AbstractAction>();
        foreach (AbstractAction action in m_currentActions)
        {
            if (action.check(m_e))
                queue.Add(action);
        }

        // remove all queued actions and add their successors
        activateNextActions(queue);
    }
    #endregion

    #region CONTROLLER FUNCS
    void SetupSteamVR2()
    {

        if (steamVRPlayArea == null)
            steamVRPlayArea = FindObjectOfType<SteamVR_PlayArea>();

        if (steamVRPlayArea != null)
        {
            // HMD
            m_hmd = steamVRPlayArea.GetComponentInChildren<Camera>().gameObject;

            // AUDIO
            foreach(Transform child in m_hmd.transform)
            {
                if (child.name == "BG_Audio")
                {
                    bg_audio = child.GetComponent<AudioSource>();
                    ApplicationSettings.instance.BgVol = bg_audio.volume;
                }
                else if (child.name == "SFX_Audio")
                {
                    sfx_audio = child.GetComponent<AudioSource>();
                    ApplicationSettings.instance.SfxVol = sfx_audio.volume;
                }
                else if (child.name == "HelmentCanvas")
                {
                    foreach (Transform chil in child)
                    {
                        if (chil.name == "Subtitles")
                        {
                            txtSub = chil.GetComponent<Text>();
                        }
                        else if (chil.name == "Hints")
                        {
                            txtHint = chil.GetComponent<Text>();
                        }
                    }
                }
            }

            // CONTROLLERS
            foreach (SteamVR_Behaviour_Pose poseComp in steamVRPlayArea.GetComponentsInChildren<SteamVR_Behaviour_Pose>(true))
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

    #region ACTIONS
    private void activateNextActions(List<AbstractAction> queue)
    {
        foreach (AbstractAction action in queue)
        {
            if (debugOutput) Debug.Log("Action " + action.getActionName() + " is completed!");

            // remove from current actions
            m_currentActions.Remove(action);

            // add all new actions and activate them
            foreach (AbstractAction newAction in action.getNextActions())
            {
                m_currentActions.Add(newAction);
                newAction.activate(m_e);
            }
        }
    }
    #endregion

}
