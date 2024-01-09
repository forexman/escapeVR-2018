using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_LookAround : SimpleAction
{
    int iVoice = 0;
    PlayerTransformations pt;
    Vector3 hmdPosition, tpPosition;
    float distance;
    bool crRunning = false;

    void Start()
    {
        tpPosition = StageController.instance.tPoints[2].transform.position;

        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_CheckEmail>());
        m_actionName = "Look Around";

        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Intro_EnteringHouse"));
        }
        m_voiceTxt = "Wow... lounge, kitchen, reading room and a \n" +
            "first floor. Exactly as promised!";

        m_hintTxt = "HINT: Check Upstairs.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Intro_Cue_Upstairs");
        m_hintVoiceTxt = "Maybe I should check upstairs.";
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

        pt = e.getPlayerPositions();
        hmdPosition = pt.HmdPosition;
        if (iVoice == 0) {
            //Check if we entered house
            hmdPosition.y = StageController.instance.tAreas[0].transform.position.y;
            if (!StageController.instance.tAreas[0].GetComponent<Collider>().bounds.Contains(hmdPosition))
            {
                if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
            }
        }
        else{
            //Check if we went Upstairs
            hmdPosition.y = StageController.instance.tAreas[8].transform.position.y;
            if (StageController.instance.tAreas[8].GetComponent<Collider>().bounds.Contains(hmdPosition))
            {
                GameDirector.instance.ShowHint("");
                return true;
            }
        }

        return false;
    }

    IEnumerator PlayVoiceAndWait(int ivoice)
    {
        crRunning = true;
        PlayVoiceLine(ivoice);
        yield return new WaitForSeconds(m_voice[ivoice].length);
        iVoice++;
        crRunning = false;
    }
}
