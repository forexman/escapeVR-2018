using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_CheckEmail : SimpleAction
{
    int iVoice = 0;
    MacPass pcPass;

    void Start()
    {
        pcPass = FindObjectOfType<MacPass>();

        //SimpleAction Setup
        m_actionName = "Check Email";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_GoToEntrance>());

        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Intro_CheckEmail"));
        }
        m_voiceTxt = "The agent said I should check my email.";

        m_hintTxt = "HINT: Examine the wedding photo.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Intro_Cue_EmailPassword");
        m_hintVoiceTxt = "That wedding picture looks so romantic.";
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

        //Playing action intro voice line
        if (iVoice == 0)
        {
            PlayVoiceLine(iVoice);
            iVoice++;
        }

        if (pcPass.IsUnlocked())
        {
            GameDirector.instance.ShowHint("");
            return true;
        }

        return false;
    }
}
