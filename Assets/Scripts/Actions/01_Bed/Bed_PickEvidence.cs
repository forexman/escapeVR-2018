using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed_PickEvidence : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject bookshelf;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Pick Evidence";
        //if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_CheckUrn>());
        if (bookshelf == null) bookshelf = GameObject.Find("Bedroom_Props");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_SeeGhost_01"));
            voiceTexts.Add("What the");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_SeeGhost_02"));
            voiceTexts.Add("I can't believe this is really happening...");
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

                break;
            case 1:

                break;
            case 2:

                break;

            default:
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
