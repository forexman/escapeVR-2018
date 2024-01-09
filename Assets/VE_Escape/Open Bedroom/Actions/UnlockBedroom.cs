using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBedroom : AbstractAction {

    public GameObject safe_key, bedroom_door;
    public AudioClip voiceLine, voiceLine2;

    public float m_maxDistrance, distance_Door;

    private bool m_cuePlayed = false;
    private bool picked_key = false;
    private bool asked = false;
    private bool bump = false;
    private PlayerTransformations pt;

    public override bool check(Environment e)
    {
        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }

        if (safe_key.activeSelf && !picked_key)
        {
            // Check if the player is at the Bedroom Door.
            pt = e.getPlayerPositions();
            //Debug.Log(Vector3.Distance(pt.RightControllerPosition, safe_key.transform.position));
            if (Vector3.Distance(pt.LeftControllerPosition, safe_key.transform.position) < m_maxDistrance ||
                Vector3.Distance(pt.RightControllerPosition, safe_key.transform.position) < m_maxDistrance)
            {

                e.getAudioController().playSound(voiceLine);
                picked_key = true;
            }
        }

        if (picked_key)
        {
            pt = e.getPlayerPositions();
            if (Vector3.Distance(pt.HmdPosition, bedroom_door.transform.position) < distance_Door)
            {
                e.getAudioController().playSound(voiceLine2);
                return true;
            }
            

        }


        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Going to the Bedroom";
        m_cueThreshold = 120;
    }
}
