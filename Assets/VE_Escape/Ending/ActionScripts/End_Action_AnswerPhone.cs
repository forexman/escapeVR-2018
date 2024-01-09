using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Action_AnswerPhone : AbstractAction
{


    public GameObject m_phone;
    public AudioClip m_voiceLine;

    private bool m_cuePlayed = false;
    private float m_timeToFade = 1;

    private bool m_voicePlayed = false;
    private float m_voiceStartedTime;


    public override bool check(Environment e)
    {
        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }

        PlayerTransformations pt = e.getPlayerPositions();

        Vector3 phonePos = m_phone.transform.position;
        Vector3 headPos = pt.HmdPosition;
        

        if (!m_voicePlayed)
        {
            // check if player is holding phone close (closer than 20cm) to his head
            if ((phonePos - headPos).magnitude < 0.2f)
            {
                e.getAudioController().playSound(m_voiceLine);
                m_voiceStartedTime = e.getCurrentTime();
                m_voicePlayed = true;
            }
        }
        else
        {
            if (e.getCurrentTime() > m_voiceStartedTime + m_timeToFade)
            {
                Valve.VR.SteamVR_Fade.Start(Color.clear, 0);
                Valve.VR.SteamVR_Fade.Start(Color.black, 3);
                return true;
            }

        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_cueThreshold = 30;
        m_actionName = "Answering the phone";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
