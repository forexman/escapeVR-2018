using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Intro_GetEntranceKey : SimpleAction
{
    GameObject key;

    //Vars
    bool grabbedKey = false;
    int iVoice = 0;

    void Start()
    {
        //Making sure there are not nulls
        if (!key) key = GameObject.Find("Key_Entrance");

        //SimpleAction Setup
        m_actionName = "Get the Key";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_OpenEntranceDoor>());

        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Intro_GetKey"));
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Intro_Cue_GetKey"));
        }
        m_voiceTxt = "The agent told me he left the key under the pot. \n" +
            "I wonder why he didn't want to meet me here...";

        m_hintTxt = "HINT: Look under the pot.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Intro_GetKey_Hint");
        m_hintVoiceTxt = "I should take a closer look at the pot";
    }

    public override bool Check(Environment e)
    {
        //Playing action intro voice line
        if (iVoice == 0)
        {
            PlayVoiceLine(iVoice);
            iVoice++;
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

        // Check if key picked up
        if (key.GetComponent<Interactable>().hoveringHand && !grabbedKey)
        {
            grabbedKey = true;
            m_voiceTxt = "Ah, there you are!";
            PlayVoiceLine(iVoice);
            GameDirector.instance.ShowHint("");
            return true;
        }

        return false;
    }
}
