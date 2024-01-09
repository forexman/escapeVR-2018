using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bed_GoToBed : SimpleAction
{
    int iVoice = 0;
    GameObject woman, door;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();
    AudioClip bumps;
    bool sSetup = false;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Go to Bedroom";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Bed_GoToKitchen>());
        if (woman == null) woman = GameObject.Find("Woman_Dead");
        if (door == null) door = GameObject.Find("Door_Bedroom");
        bumps = Resources.Load<AudioClip>("Audio/SFX/Slamming_Door");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_GoToBed_01"));
            voiceTexts.Add("Where am I?");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_GoToBed_02"));
            voiceTexts.Add("What was that?");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_GoToBed_03"));
            voiceTexts.Add("What happened here?");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_GoToBed_04"));
            voiceTexts.Add("Who is in there? Are you ok?");
        }

        // Hint Setup
        m_hintTxt = "HINT: Investigate the noise upstairs.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_GoToBed");
        m_hintVoiceTxt = "I should hurry upstairs!";
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        if (ApplicationSettings.instance.HasHints)
        {
            //Debug.Log("Checking: " + m_cueThreshold + " / " + e.getCurrentTime() + " / " + m_activatedTime);
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                //Debug.Log("Showing hint@" + e.getCurrentTime() +"/"+ m_activatedTime);
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }

        if (!sSetup)
        {
            //Disabling teleport areas for Bedroom, Garage
            StageController.instance.ActivateGroundFloorTeleports();
            StageController.instance.ActivateBedroomTeleport(false);
            StageController.instance.ActivateGarageTeleport(false);

            woman.GetComponent<Animator>().SetBool("Dead", true);
            sSetup = true;
        }

        switch (iVoice)
        {
            case 0:
                // Where am I?
                if(!crRunning) StartCoroutine(WaitAndPlayVoice(0, 2));
                break;
            case 1:
                // Scream SFX + Cry
                if(!crRunning) StartCoroutine(WomanSoundControl(0, m_voice[0].length+1));
                break;
            case 2:
                // What was that?
                if (!crRunning) StartCoroutine(WaitAndPlayVoice(1, 4));
                break;
            case 3:
                // Sees Blood - What happened here?
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[0].transform.position.y;
                if (!StageController.instance.tAreas[0].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    if (!crRunning) StartCoroutine(WaitAndPlayVoice(2, 0));
                }
                break;
            case 4:
                // At the Door - Who is in there?
                pt = e.getPlayerPositions();
                if (Vector3.Distance(pt.HmdPosition, door.transform.position) < 1)
                {
                    if (!crRunning) StartCoroutine(WaitAndPlayVoice(3, 0));
                }
                break;
            case 5:
                // Cry Stop
                if (!crRunning) StartCoroutine(WomanSoundControl(1, m_voice[3].length + 1));
                break;
            case 6:     
                // Bumps
                if (!crRunning) StartCoroutine(PlayBumps());
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
        //Debug.Log("Waiting to play: " + m_voiceTxt);
        yield return new WaitForSeconds(secs);
        PlayVoiceLine(ivoice);
        iVoice++;
        crRunning = false;
    }

    IEnumerator WomanSoundControl(int mode, float secs)
    {
        // mode 0: start, 1: stop
        crRunning = true;
        yield return new WaitForSeconds(secs);
        switch (mode)
        {
            case 0:
                woman.GetComponent<GhostSoundController>().PlayScream();
                break;
            case 1:
                woman.GetComponent<GhostSoundController>().StopCry();
                break;
        }
        iVoice++;
        crRunning = false;
    }


    IEnumerator PlayBumps()
    {
        crRunning = true;
        door.GetComponent<DoorController>().Door_Audio.clip = bumps;
        door.GetComponent<DoorController>().Door_Audio.Play();
        yield return new WaitForSeconds(bumps.length+1f);
        iVoice++;
        crRunning = false;
    }
}
