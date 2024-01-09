using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAction_01 : AbstractAction {

    public GameObject m_cube;

    private Vector3 m_initialPos;
    private bool cuePlayed = false;

    // Use this for initialization
    void Start()
    {
        m_actionName = "Cube movement";
        m_cueThreshold = 5;
    }

    public override bool check(Environment e)
    {
        // check if we already played the cue and have a cue to play
        if (!cuePlayed && m_audioCues.Count > 0)
        {

            // check if we should play the cue
            if (e.getCurrentTime() > m_activatedTime + m_cueThreshold)
            {
                // play at players position
                //e.getAudioController().playSound(m_audioCues[0]);

                // play at specific position
                e.getAudioController().playSound(m_audioCues[0], new Vector3(5.0f, 1.0f, 0.0f));

                cuePlayed = true;
            }
        }

        // check if requirements are fulfilled
        if (m_initialPos.z - m_cube.transform.position.z >= 3)
            return true;
        else
            return false;
    }

    public override void activate(Environment e)
    {
        base.activate(e);
        m_initialPos = m_cube.transform.position;
    }

}
