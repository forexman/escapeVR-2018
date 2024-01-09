using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DnD_DoeSwitch : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject doe, painting;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Doe Switch";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<DnD_PerspectivePuzzle>());
        if (doe == null) doe = GameObject.Find("Switch_Doe");
        if (painting == null) painting = GameObject.Find("Painting_Deer");

        // Voicelines Setup
        /*if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/DnD_LookAtStag"));
            voiceTexts.Add("There is the stag. Just\n need to find its doe now.");
        }*/

        //SteamVR listen for trigger
        SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);

        // Hint Setup
        m_hintTxt = "HINT: Search for the doe in the room.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/DnD_Cue_DoeSwitch");
        m_hintVoiceTxt = "I found the stag, the doe\n has to be around here somewhere...";
    }

    public override bool Check(Environment e)
    {
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
                //Waiting for Doe Switch Interaction
                break;
            case 1:
                //Playing Switch sound
                if (!crRunning) StartCoroutine(PlaySwitchAndWait());
                break;
            case 2:
                //Rotating Painting / Sound
                if (!crRunning) StartCoroutine(RotatePaintingAndWait());
                break;
            default:
                GameDirector.instance.ShowHint("");
                return true;
        }
        return false;
    }

    IEnumerator WaitAndPlayVoice(int ivoice, float secs)
    {
        crRunning = true;
        m_voiceTxt = voiceTexts[ivoice];
        yield return new WaitForSeconds(secs);
        PlayVoiceLine(ivoice);
        iVoice++;
        crRunning = false;
    }

    IEnumerator PlayVoiceAndWait(int ivoice)
    {
        crRunning = true;
        m_voiceTxt = voiceTexts[ivoice];
        PlayVoiceLine(ivoice);
        yield return new WaitForSeconds(m_voice[ivoice].length);
        iVoice++;
        crRunning = false;
    }

    IEnumerator PlaySwitchAndWait()
    {
        crRunning = true;
        doe.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.2f);
        iVoice++;
        crRunning = false;
    }
    IEnumerator RotatePaintingAndWait()
    {
        crRunning = true;
        painting.GetComponent<AudioSource>().Play();
        painting.transform.localPosition = new Vector3(3.947f, 2.300546f, 0.108f);
        painting.transform.eulerAngles = new Vector3(-180f, 45f, 0f);
                  
        yield return new WaitForSeconds(1.5f);
        iVoice++;
        crRunning = false;
    }

    private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (doe.GetComponent<Interactable>().hoveringHand)
        {
            //Debug.Log("Trigger pressed at " + this);
            if (iVoice == 0)
            {
                iVoice++;
            }
        }
    }
}