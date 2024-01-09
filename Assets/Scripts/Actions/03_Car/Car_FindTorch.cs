using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Car_FindTorch : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject torch, hint_light;
    bool actionActive;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Find Torch";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Car_PerspectivePuzzle>());
        if (torch == null) torch = GameObject.Find("Garage_Props").transform.GetChild(1).GetChild(1).gameObject;
        if (hint_light == null) hint_light = GameObject.Find("Garage_Props").transform.GetChild(1).GetChild(0).GetChild(0).gameObject;

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Car_FindTorch"));
            voiceTexts.Add("This feels way too light. The batteries\n have to be somewhere around here.");
        }

        // Hint Setup
        m_hintTxt = "HINT: Search for a torch upstairs.";

        //if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_UnlockBedroom");
        //m_hintVoiceTxt = "I should hurry upstairs.";

        //SteamVR listen for trigger
        SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        if (ApplicationSettings.instance.HasHints)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                hint_light.GetComponent<Light>().intensity = 10f;
                m_activatedTime = e.getCurrentTime();
            }
        }

        switch (iVoice)
        {
            case 0:
                //Activate Torch
                torch.SetActive(true);
                actionActive = true;
                iVoice++;
                break;
            case 1:
                //Waiting to grab torch
                break;
            default:
                actionActive = false;
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

    private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (torch.GetComponent<Interactable>().hoveringHand && actionActive)
        {
            hint_light.SetActive(false);
            if (!crRunning) StartCoroutine(PlayVoiceAndWait(0));
        }
    }
}