using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MainMenu : MonoBehaviour
{
    //STEAMVR Setup
    private GameObject m_rightController, m_leftController, m_hmd;
    private SteamVR_PlayArea m_steamVRPlayArea;

    //Audio Sources
    private AudioSource m_bg_audio, m_sfx_audio;
    //Text Feedback
    private Text m_txtSub, m_txtHint;

    private AudioClip m_laugh;
    private bool m_starting;
    private Slider m_mSlider, m_sfxSlider;
    private Text m_tHints, m_tSubtitles, m_tHintsTime;
    private Sprite m_oculus, m_vive;
    float count;

    #region LIFECYCLE
    void Awake()
    {
        SetupSteamVR2();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_laugh = Resources.Load<AudioClip>("Audio/SFX/woman_laugh");

        m_oculus = Resources.Load<Sprite>("Images/oculus");
        m_vive = Resources.Load<Sprite>("Images/vive");

        //Sliders
        m_mSlider = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        m_sfxSlider = transform.GetChild(1).GetChild(1).GetComponent<Slider>();

        //Texts
        m_tSubtitles = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>();
        m_tHints = transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>();
        m_tHintsTime = transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>();
        SetSettingsTexts();

        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        m_starting = false;
        StartCoroutine(SetControls());
    }
    #endregion

    #region SETUP FUNC
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
                    m_bg_audio = child.GetComponent<AudioSource>();
                    ApplicationSettings.instance.BgVol = m_bg_audio.volume;
                }
                else if (child.name == "SFX_Audio")
                {
                    m_sfx_audio = child.GetComponent<AudioSource>();
                    ApplicationSettings.instance.SfxVol = m_sfx_audio.volume;
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

    #region HELPER FUNCS
    IEnumerator SetControls()
    {
        yield return new WaitForSeconds(0.3f);
        SteamVR_RenderModel rm;
        rm = FindObjectOfType<SteamVR_RenderModel>();
        //Debug.Log(rm);

        if (!rm)
        {
            //Debug.Log("Loading model...");
            StartCoroutine(SetControls());
        }
        else
        {
            string model = rm.renderModelName;
            //Debug.Log(model);
            if (model!=null)
            {
                if (model.Contains("oculus"))
                {
                    //Debug.Log("It is an oculus");
                    transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = m_oculus;
                }
                else if (model.Contains("vive"))
                {
                    //Debug.Log("It is a vive");
                    transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = m_vive;
                }
            }
            else
            {
                //Debug.Log("Loading model name...");
                StartCoroutine(SetControls());
            }
        }
    }

    public void SetSettingsTexts()
    {
        if (ApplicationSettings.instance.HasHints)
        {
            m_tHints.text = "ON";
            m_tHints.color = Color.green;
        }
        else
        {
            m_tHints.text = "OFF";
            m_tHints.color = Color.red;
        }

        if (ApplicationSettings.instance.HasSubtitles)
        {
            m_tSubtitles.text = "ON";
            m_tSubtitles.color = Color.green;
        }
        else
        {
            m_tSubtitles.text = "OFF";
            m_tSubtitles.color = Color.red;
        }

        m_tHintsTime.text = ApplicationSettings.instance.HintThres.ToString();
    }
    #endregion

    #region BUTTONS
    public void OnClickStart()
    {
        if (!m_starting)
        {
            m_starting = true;
            m_bg_audio.Stop();
            m_sfx_audio.Stop();
            m_sfx_audio.clip = m_laugh;
            m_sfx_audio.loop = false;
            m_sfx_audio.Play();
            StartCoroutine(BlankToLoad());
            //GetComponent<SteamVR_LoadLevel>().Trigger();
        }
    }

    IEnumerator BlankToLoad()
    {
        SteamVR_Fade.Start(Color.black, 2);
        yield return new WaitForSeconds(3);
        count = Time.realtimeSinceStartup;
        StageController.instance.MoveToNormalRealm();
    }

    IEnumerator LoadYourAsyncSceneSingle(int iLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(iLoad, LoadSceneMode.Single);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //Debug.Log("Loading Scene Incomplete");
            yield return null;
            //Debug.Log("Loading Scene took: " + (Time.realtimeSinceStartup - count));
        }
    }

    IEnumerator LoadScene()
    {
        //yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            //Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //Debug.Log("Press the space bar to continue");
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    IEnumerator ReturnVision()
    {
        yield return new WaitForSeconds(4);
        SteamVR_Fade.Start(Color.clear, 2);
        SceneManager.LoadScene(1);
    }

    public void OnClickSubtitles()
    {
        ApplicationSettings.instance.HasSubtitles = !ApplicationSettings.instance.HasSubtitles;
        SetSettingsTexts();

        //Preview
        if (ApplicationSettings.instance.HasSubtitles)
        {
            m_txtSub.text = "Morning! Nice day for fishing ain't it!";
            StartCoroutine(ExpirePreviewSub());
        }
    }
    IEnumerator ExpirePreviewSub()
    {
        yield return new WaitForSeconds(3);
        m_txtSub.text = "";
    }

    public void OnClickHints()
    {
        ApplicationSettings.instance.HasHints = !ApplicationSettings.instance.HasHints;
        SetSettingsTexts();

        //Preview
        if (ApplicationSettings.instance.HasHints)
        {
            m_txtHint.text = "HINT: The weather is rather droll, is it not?";
            StartCoroutine(ExpirePreviewHint());
        }
    }
    IEnumerator ExpirePreviewHint()
    {
        yield return new WaitForSeconds(3);
        m_txtHint.text = "";
    }

    public void IncreaseHintMinutes()
    {
        if (ApplicationSettings.instance.HintThres != 9)
        {
            ApplicationSettings.instance.HintThres++;
            m_tHintsTime.text = ApplicationSettings.instance.HintThres.ToString();
        }
    }
    public void DecreaseHintMinutes()
    {
        if (ApplicationSettings.instance.HintThres != 0)
        {
            ApplicationSettings.instance.HintThres--;
            m_tHintsTime.text = ApplicationSettings.instance.HintThres.ToString();
        }
    }

    public void SliderMusicVolume()
    {
        if (m_mSlider.value >= 0.01f)
        {
            m_bg_audio.volume = m_mSlider.value / 2;
            ApplicationSettings.instance.BgVol = m_mSlider.value / 2;
        }
        else
        {
            m_bg_audio.volume = 0.01f;
            ApplicationSettings.instance.BgVol = 0.01f;
        }
    }
    public void SliderSFXVolume()
    {
        if(!m_sfx_audio.isPlaying) m_sfx_audio.Play();
        if (m_sfxSlider.value >= 0.05f)
        {
            m_sfx_audio.volume = m_sfxSlider.value / 2;
            ApplicationSettings.instance.SfxVol = m_sfxSlider.value / 2;
        }
        else
        {
            m_sfx_audio.volume = 0.05f;
            ApplicationSettings.instance.SfxVol = 0.05f;
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
    #endregion
}
