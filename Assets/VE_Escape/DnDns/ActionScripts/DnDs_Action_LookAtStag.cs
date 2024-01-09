using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnDs_Action_LookAtStag : AbstractAction
{

    private GameObject m_painting;
    public AudioClip m_voiceLine;

    public float m_maxDistrance;

    private bool m_cuePlayed = false;

    public override bool check(Environment e)
    {
        // To avoid conflics with the car storyline, there is no initial cue anymore
        //if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        //{
        //    m_cuePlayed = true;
        //    e.getAudioController().playSound(m_audioCues[0]);
        //}

        // Check if the player looks at the painting. If yes, play the voiceline and move on.
        RaycastHit hit;
        PlayerTransformations pt = e.getPlayerPositions();

        if(Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, m_maxDistrance))
        {
            if (hit.collider.gameObject == m_painting)
            {
                e.getAudioController().playSound(m_voiceLine);
                return true;
            }
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Looking at Stag";
        m_cueThreshold = 120;
        m_painting = GameObject.Find("Painting_Deer"); 
    }

    private void Update()
    {
        if (m_painting == null)
        {
            m_painting = GameObject.Find("Painting_Deer");
        }
    }
}
