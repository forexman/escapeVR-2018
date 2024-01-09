using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Action_FindPhone : AbstractAction
{

    public GameObject m_phone;

    public AudioClip m_findPhone;

    public float m_angle = 0.5f;
    public float m_distance = 2.0f;

    private bool m_cuePlayed = false;

    public override bool check(Environment e)
    {
        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }


        PlayerTransformations pt = e.getPlayerPositions();

        Vector3 phonePos = m_phone.transform.position;
        Vector3 phoneDir = phonePos - pt.HmdPosition;

        float angle = Vector3.Dot(phoneDir.normalized, pt.HmdForwardVector.normalized);

        if(angle > m_angle && phoneDir.magnitude < m_distance)
        {
            // ray cast if object is not occluded by any other
            RaycastHit hit;
            if(Physics.Raycast(pt.HmdPosition, phoneDir, out hit, m_distance))
            {
                if(hit.collider.gameObject == m_phone)
                {
                    e.getAudioController().playSound(m_findPhone);
                    return true;
                }
            }
        }

        return false;

    }


    // Use this for initialization
    void Start()
    {
        m_actionName = "Looking for phone";
        m_cueThreshold = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
