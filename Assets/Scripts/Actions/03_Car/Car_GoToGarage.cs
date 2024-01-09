using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Car_GoToGarage : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject door;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Go To Garage";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Car_FindTorch>());
        if (door == null) door = GameObject.Find("Door_Garage_S");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Car_GoToGarage"));
            voiceTexts.Add("It's way too dark! There might\n be a flashlight upstairs.");
        }

        // Hint Setup
        //m_hintTxt = "HINT: Unlock the Bedroom door!";

        //if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_UnlockBedroom");
        //m_hintVoiceTxt = "I should hurry upstairs.";
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        /*if (ApplicationSettings.instance.HasHints)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }*/

        switch (iVoice)
        {
            case 0:
                door.GetComponent<DoorController>().UnlockDoor();
                StageController.instance.ActivateGarageTeleport(true);
                iVoice++;
                break;
            case 1:
                //Check if we are in the entrance area
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[0].transform.position.y;
                if (StageController.instance.tAreas[0].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    iVoice++;
                }
                break;
            case 2:
                //Check if we are in the garage
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[7].transform.position.y;
                if (StageController.instance.tAreas[7].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    if (!crRunning) StartCoroutine(PlayVoiceAndWait(0));
                }
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
}

