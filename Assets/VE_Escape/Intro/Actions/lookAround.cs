using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAround : AbstractAction
{
    public GameObject marker;
    public AudioClip enter_room;

    bool flag = true;
    bool m_cuePlayed = false;

    public override bool check(Environment e)
    {
        if (flag)
        {
            e.getAudioController().playSound(enter_room);
            flag = false;
        }

        // If we surpass the given time, give hint to move to the office.
        if (e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            return true;
        }

        // Check if the player enters the office. If yes, play the voiceline and move on.
        PlayerTransformations pt = e.getPlayerPositions();

        Vector3 markerPosition = marker.transform.position;
        Vector3 hmdPosition = pt.HmdPosition;

        // set y for both to 0.0, as height is not relevant for this check
        markerPosition.y = 0.0f;
        hmdPosition.y = 0.0f;

        float distance = (hmdPosition - markerPosition).magnitude;
        // Debug.Log(distance);

        // trigger if distance is less than 5cm
        if (distance < 0.8f)
        {
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Look around";
        m_cueThreshold = 120;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
