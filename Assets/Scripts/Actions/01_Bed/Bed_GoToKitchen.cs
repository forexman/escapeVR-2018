using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bed_GoToKitchen : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    public GameObject safe;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Go to Kitchen";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Bed_UnlockBedroom>());
        if (safe == null) safe = GameObject.Find("Kitchen_Safe");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_GoToKitchen_01"));
            voiceTexts.Add("I have to open this door!");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_GoToKitchen_02"));
            voiceTexts.Add("Where could the keys be?");
        }

        // Hint Setup
        m_hintTxt = "HINT: The fog seems thicker at the kitchen.";
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
                // I have to open this door
                if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                break;
            case 1:
                // Kitchen
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[4].transform.position.y;
                if (StageController.instance.tAreas[4].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                    safe.GetComponent<Kitchen_Safe>().ActivateSafePuzzle();
                }
                break;
            default:
                if (safe.GetComponent<Kitchen_Safe>().IsSafeOpen())
                {
                    GameDirector.instance.ShowHint("");
                    return true;
                }
                break;
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
        yield return new WaitForSeconds(m_voice[ivoice].length+1);
        iVoice++;
        crRunning = false;
    }
}
