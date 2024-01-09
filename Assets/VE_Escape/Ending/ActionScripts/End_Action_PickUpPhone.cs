using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Action_PickUpPhone : AbstractAction
{
    public GameObject m_phone;

    public AudioClip m_phoneCall;



    private bool m_phonePickedUp = false;
    private float m_voiceLineStartedTime = 0;
    private bool m_voiceLineStarted = false;
       
    public override bool check(Environment e)
    {
        if (m_phonePickedUp && !m_voiceLineStarted)
        {
            e.getAudioController().playSound(m_phoneCall, m_phone);
            m_voiceLineStarted = true;
            m_voiceLineStartedTime = e.getCurrentTime();
        }

        // wait 9 seconds for line to play before continuing
        if (m_voiceLineStarted && e.getCurrentTime() > m_voiceLineStartedTime + 9.0f)
            return true;

        return false;
    }


    public void phonePickedUp()
    {
        m_phonePickedUp = true;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "picking up phone";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
