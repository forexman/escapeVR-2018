using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Outro_FindPhone : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    bool sSetup = false;
    GameObject phone;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Outro Find Phone";
        if (!phone) phone = GameObject.Find("nokia-n80");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Outro_VL_01"));
            voiceTexts.Add("Huh, I'm back. I probably should call the police.");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Outro_VL_02"));
            voiceTexts.Add("Hello, I want to report a crime...");
        }

        //SteamVR listen for trigger
        SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);

        // Hint Setup
        m_hintTxt = "HINT: Look around for a phone.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Outro_Cue");
        m_hintVoiceTxt = "I remember seeing a phone around the computer.";
    }

    public override bool Check(Environment e)
    {
        if (!sSetup)
        {
            phone.GetComponent<BoxCollider>().enabled = true;
            phone.GetComponent<Rigidbody>().useGravity = true;
            phone.GetComponent<Interactable>().enabled = true;

            sSetup = true;
        }

        // Check the need for hint
        if (ApplicationSettings.instance.HasHints)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }

        switch (iVoice)
        {
            case 0:
                //Play returning voiceline
                if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                break;
            case 1:
                //waiting to pick up phone
                break;
            case 2:
                // Play police call
                if (!crRunning)
                {
                    phone.GetComponent<AudioSource>().Play();
                    StartCoroutine(Wait(10));
                }
                break;
            case 3:
                if (!crRunning) StartCoroutine(PlayVoiceAndWait(1));
                break;
            default:
                GameDirector.instance.ShowHint("");
                StageController.instance.EndCredits();
                return true;
        }
        return false;
    }

    IEnumerator Wait(float secs)
    {
        crRunning = true;
        yield return new WaitForSeconds(secs);
        iVoice++;
        crRunning = false;
    }

    IEnumerator PlayVoiceAndWait(int ivoice)
    {
        crRunning = true;
        m_voiceTxt = voiceTexts[ivoice];
        PlayVoiceLine(ivoice);
        yield return new WaitForSeconds(m_voice[ivoice].length + 2);
        iVoice++;
        crRunning = false;
    }

    private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (phone.GetComponent<Interactable>().hoveringHand && iVoice == 1)
        {
            iVoice++;
        }
    }
}
