using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Action_FindGarage : AbstractAction
{

    public AudioClip m_voiceLine;

    public GameObject garageDoor;
    public Transform garageEntrance;

    public float m_maxDistance;

    private bool m_cuePlayed = false;


    public override bool check(Environment e)
    {

        // Cue player to go and find the oldest car in the garage
        // To prevent collisions with the dnd storyline, there is no initial cue anymore
        //if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        //{
        //    m_cuePlayed = true;
        //    e.getAudioController().playSound(m_audioCues[0]);
        //}

        // Check if the player looks at the garage door. If yes, play the voiceline and move on.
        RaycastHit hit;
        PlayerTransformations pt = e.getPlayerPositions();

        if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, m_maxDistance))
        {
            if (hit.collider.gameObject == garageDoor)
            {
                e.getAudioController().playSound(m_audioCues[1]); //Play 'Found garage' audioline
            }
        }

        float headDistance = (pt.HmdPosition - garageEntrance.transform.position).magnitude;

        if (headDistance < 3.0f)
        {
            Debug.Log("FindGarage Complete");

            e.getAudioController().playSound(m_voiceLine);

            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Find Garage";
        m_cueThreshold = 60;
    }

}
