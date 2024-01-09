using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnDs_Action_PerspectivePuzzle : AbstractAction
{
    public GameObject m_cross_Window;
    public GameObject m_cross_Wall;

    public Light m_light_01;
    public Light m_light_02;
    public Light m_light_03;

    public float m_degreeThreshold;

    public AudioClip m_voiceLine;


    private bool m_cuePlayed = false;

    // Use this for initialization
    void Start()
    {
        m_actionName = "Perspective Puzzle";
        m_cueThreshold = 60;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool check(Environment e)
    {
        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }

        // Do a raycast and check if we hit both crosses
        // TODO: might be better to do raycast with forward vector?
        PlayerTransformations pt = e.getPlayerPositions();
        Vector3 position = pt.HmdPosition;
        
        Vector3 posToCross_Wall = m_cross_Wall.transform.position - position;

        RaycastHit[] hits = Physics.RaycastAll(pt.HmdPosition, posToCross_Wall, 20);

        bool wallCrossHit = false;
        bool windowCrossHit = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == m_cross_Wall)
                wallCrossHit = true;

            if (hit.collider.gameObject == m_cross_Window)
                windowCrossHit = true;
        }

        if (wallCrossHit && windowCrossHit)
        {
            float angleView = Vector3.Angle(posToCross_Wall, pt.HmdForwardVector);
            Debug.Log("Hit, angle " + angleView);

            if (angleView < m_degreeThreshold)
            {
                e.getAudioController().playSound(m_voiceLine);
                m_light_01.intensity = 2.0f;
                m_light_02.intensity = 2.0f;
                m_light_03.intensity = 2.0f;
                return true;
            }
        }

        return false;
    }
}
