using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Intro_OpenEntranceDoor : SimpleAction
{
    GameObject door_entrance;

    void Start()
    {
        //Making sure there are not nulls
        if (!door_entrance) door_entrance = GameObject.Find("Door_Entrance");

        //SimpleAction Setup
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_LookAround>());

        m_actionName = "Open the Entrance Door";
        m_hintTxt = "HINT: Place the key in the lock.";
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

        // Check if door unlocked
        if (!door_entrance.GetComponent<DoorController>().DoorLocked)
        {
            StageController.instance.ActivateGroundFloorTeleports();
            GameDirector.instance.ShowHint(m_hintTxt);
            return true;
        }

        return false;
    }
}
