using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Car_Action_EnterCarSafeCode : AbstractAction
{
    public AudioClip m_voiceLine;

    public GameObject safe, garageEntrance;

    public bool passEntered = false;

    private bool m_cuePlayed = false;
    private bool m_cue2Played = false;
    private bool voiceLinePlayed = false;
    private float m_cue2Threshold;


    // Use this for initialization
    void Start()
    {
        m_actionName = "Enter car safe code";
        m_cueThreshold = 60;
        m_cue2Threshold = 3;

        safe.GetComponent("BedRoom_Safe");
    }

    public override bool check(Environment e)
    {
        Debug.Log("Enter car safe code started");

        // Cue player to go and find a torch
        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }

        // Cue player to go and find a torch
        if (!m_cue2Played && e.getCurrentTime() > m_activatedTime + m_cue2Threshold)
        {
            m_cue2Played = true;
            e.getAudioController().playSound(m_audioCues[1]);
        }


        PlayerTransformations pt = e.getPlayerPositions();

        float headDistance = (pt.HmdPosition - garageEntrance.transform.position).magnitude;

        if (headDistance < 3.0f && voiceLinePlayed == false)
        {
            voiceLinePlayed = true;

            e.getAudioController().playSound(m_voiceLine);

        }

        passEntered = safe.GetComponent<BedRoom_Safe>().pass1done;

        //If user has entered correct password into safe
        return passEntered;
    }


}
