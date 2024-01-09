using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_MoveToDoor : SimpleAction
{
    GameObject TA_Entrance, TP_Entrance;

    //Vars
    Vector3 tpPos;
    int iVoice = 0;
    bool funcSetup = false;

    void Start()
    {
        //Making sure there are not nulls
        if (!TA_Entrance) TA_Entrance = StageController.instance.tAreas[0];
        if (!TP_Entrance) TP_Entrance = StageController.instance.tPoints[0];
        tpPos = TP_Entrance.transform.position;

        //SimpleAction Setup
        m_actionName = "Move To The Start Point";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_GetEntranceKey>());
        if (m_voice.Count == 0) m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Intro_MoveToDoor"));
        m_voiceTxt = "Wow, this house really looks just like in the pictures.";
        m_hintTxt = "HINT: Teleport in front of the door.";
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

        if (!funcSetup)
        {
            TP_Entrance.SetActive(true);
            StageController.instance.EditTeleportObject(StageController.instance.tPoints[0], true);
            funcSetup = true;
        }

        //Playing action intro voice line
        if (iVoice == 0)
        {
            PlayVoiceLine(iVoice);
            iVoice++;
        }

        // check whether the player reaches the marker
        PlayerTransformations pt = e.getPlayerPositions();
        Vector3 playerPos = pt.PlayerPosition;

        float distance = (playerPos - tpPos).magnitude;
        // trigger if distance is less than
        if (distance < 0.8f)
        {
            StageController.instance.EditTeleportObject(StageController.instance.tAreas[0], true);
            StageController.instance.EditTeleportObject(StageController.instance.tPoints[0], false);
            GameDirector.instance.ShowHint("");
            return true;
        }

        return false;
    }
}
