using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bed_UnlockBedroom : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject bedDoor;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Unlock Bedroom";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Bed_SeeGhost>());
        if (bedDoor == null) bedDoor = GameObject.Find("Door_Bedroom");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_UnlockBedroom_01"));
            voiceTexts.Add("At last, I have to hurry!");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_UnlockBedroom_02"));
            voiceTexts.Add("Hold on, I'm coming in!");
        }

        // Hint Setup
        m_hintTxt = "HINT: Unlock the Bedroom door!";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_UnlockBedroom");
        m_hintVoiceTxt = "I should hurry upstairs.";
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
                // At last, I have to hurry
                if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                break;
            case 1:
                // Go Upstairs
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[8].transform.position.y;
                if (StageController.instance.tAreas[8].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                }
                break;
            default:
                if (!bedDoor.GetComponent<DoorController>().DoorLocked)
                {
                    StageController.instance.ActivateBedroomTeleport(true);
                    GameDirector.instance.ShowHint("");
                    return true;
                }
                else break;
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
        yield return new WaitForSeconds(m_voice[ivoice].length + 1);
        iVoice++;
        crRunning = false;
    }
}

