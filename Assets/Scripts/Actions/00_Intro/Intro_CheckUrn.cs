using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Intro_CheckUrn : SimpleAction
{
    int iVoice = 0;
    bool transitioning = false;

    //Urn Vars
    GameObject urn, body, lid, mist;
    int urnStage = 0;
    float currentLerp = 0.0f;
    float lerpTime = 10.0f;
    Vector3 targetUrnPos = new Vector3(6.69f, 2.94f, -6.18f);

    void Start()
    {
        //GameObjects Setup
        if (!mist) mist = GameObject.Find("Normal_World_Props").transform.GetChild(0).GetChild(1).gameObject;
        if (!urn) urn = GameObject.Find("Normal_World_Props").transform.GetChild(0).GetChild(0).gameObject;

        body = urn.transform.GetChild(0).gameObject;
        lid = urn.transform.GetChild(1).gameObject;

        mist.SetActive(false);
        urn.SetActive(false);

        //SimpleAction Setup
        m_actionName = "Check Urn";
        //if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_GetEntranceKey>());

        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Intro_Urn"));
        }
        m_voiceTxt = "This... wasn't here a minute ago?";

        m_hintTxt = "HINT: Touch it?";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Intro_Cue_Urn");
        m_hintVoiceTxt = "Should I touch it?";
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        if (ApplicationSettings.instance.HasHints && !transitioning)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }

        if (iVoice == 0)
        {
            if (!urn.activeSelf) urn.SetActive(true);
            urn.GetComponent<AudioSource>().volume = ApplicationSettings.instance.SfxVol;
            urn.GetComponent<AudioSource>().Play();
            urn.GetComponent<BoxCollider>().enabled = false;
            PlayVoiceLine(iVoice);
            iVoice++;
        }
        else
        {
            // Check if urn is touched
            if (urn.GetComponent<Interactable>().hoveringHand && !transitioning)
            {
                transitioning = true;
            }

            if(transitioning)
            {
                //Urn is touched, we are doing our hoodoo magic...
                switch (urnStage)
                {
                    case 0:
                        LiftUrn();
                        break;
                    case 1:
                        LiftLid();
                        break;
                    case 2:
                        GameDirector.instance.ShowHint("");
                        return true;
                }
            }
        }

        return false;
    }


    void LiftUrn()
    {
        if (!body.transform.GetChild(0).gameObject.activeSelf) body.transform.GetChild(0).gameObject.SetActive(true);
        currentLerp += Time.deltaTime;
        if (currentLerp >= lerpTime) currentLerp = lerpTime;
        urn.transform.Rotate(Vector3.forward * Time.deltaTime * 250f);
        urn.transform.position = Vector3.Lerp(urn.transform.position, targetUrnPos, 0.05f * currentLerp / lerpTime);
        if (Vector3.Distance(urn.transform.position, targetUrnPos) <= 0.1f || urn.transform.position.y > 1.7f)
        {
            urnStage++;
            mist.SetActive(true);
            StartCoroutine(MoveToShadow());
        }
    }

    IEnumerator MoveToShadow()
    {
        SteamVR_Fade.Start(Color.gray, 9);
        yield return new WaitForSeconds(10);
        mist.SetActive(false);
        urn.SetActive(false);
        StageController.instance.MoveToShadowRealm();
        urnStage++;
    }

    void LiftLid()
    {
        if (!body.transform.GetChild(1).gameObject.activeSelf) body.transform.GetChild(1).gameObject.SetActive(true);
        if (lid.transform.position.y < 2.7f)
        {
            lid.transform.Translate(Vector3.forward * Time.deltaTime);
        }
    }

    
}
