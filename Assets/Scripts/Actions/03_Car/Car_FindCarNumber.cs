using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_FindCarNumber : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject gProps;
    int cClue;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Find car number";
        if (gProps == null) gProps = GameObject.Find("Garage_Props");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Car_FindCarNumber"));
            voiceTexts.Add("Let's go find some cars.");
        }

        // Hint Setup
        m_hintTxt = "HINT: The oldest plate is the most worn out.";

        //if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_UnlockBedroom");
        //m_hintVoiceTxt = "I should hurry upstairs.";
        cClue = 0;
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        if (ApplicationSettings.instance.HasHints)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime && cClue == 0)
            {
                ShowHint();
                cClue++;
                m_activatedTime = e.getCurrentTime();
            }
            else if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime && cClue == 1)
            {
                gProps.transform.GetChild(0).GetChild(0).GetComponent<Light>().intensity = 2f;
                cClue++;
            }
            
        }

        switch (iVoice)
        {
            case 0:
                if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                break;
            case 1:
                //If we go close to plate, light it up
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = gProps.transform.GetChild(0).GetChild(1).position.y;
                if (Vector3.Distance(gProps.transform.GetChild(0).GetChild(1).position, hmdPosition) <= 1.5f)
                {
                    gProps.transform.GetChild(0).GetChild(0).GetComponent<Light>().intensity = 5f;
                    iVoice++;
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