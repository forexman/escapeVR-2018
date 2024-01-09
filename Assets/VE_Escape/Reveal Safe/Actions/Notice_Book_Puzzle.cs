using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice_Book_Puzzle : AbstractAction {

    public GameObject bookself;
    public AudioClip m_voiceLine;

    public float m_maxDistrance;

    private bool m_cuePlayed = false;

    public override bool check(Environment e)
    {

        // Check if the player looks at the painting. If yes, play the voiceline and move on.
        RaycastHit hit;
        PlayerTransformations pt = e.getPlayerPositions();

        if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, m_maxDistrance))
        {
            if (hit.collider.gameObject == bookself)
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
        m_actionName = "Looking for the Safe";
    }
}
