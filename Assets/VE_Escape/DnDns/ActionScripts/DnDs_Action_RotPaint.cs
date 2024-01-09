using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnDs_Action_RotPaint : AbstractAction
{

    public GameObject m_painting;

    public AudioClip m_squeakingHinges;

    private bool m_turned = false;
    private int m_angleTurned = 0;
    private Vector3 m_rotationCenter;

    // Use this for initialization
    void Start()
    {
        m_actionName = "Painting Rotation";
        m_painting = GameObject.Find("Deer_Painting");
    }

    private void FixedUpdate()
    {
        if(!m_turned && m_activated)
        {
            m_painting.transform.RotateAround(m_rotationCenter, Vector3.up, 1);
            m_angleTurned++;
        }

    }

    private void Update()
    {
        if (m_painting == null)
        {
            m_painting = GameObject.Find("Deer_Painting");
        }
    }

    public override bool check(Environment e)
    {        
        if (m_angleTurned > 90)
            m_turned = true;


        MeshRenderer mr = m_painting.GetComponent<MeshRenderer>();
        Vector3 boundSizes = mr.bounds.size;

        return m_turned;
    }

    public override void activate(Environment e)
    {
        base.activate(e);


        Vector3 centerPos = m_painting.transform.position;

        //Component[] c = m_painting.GetComponents<Component>();

        MeshRenderer mr = m_painting.GetComponent<MeshRenderer>();

        m_rotationCenter = centerPos;
        if (mr != null)
        {
            Vector3 rotationOffset = mr.bounds.extents;

            m_rotationCenter = centerPos + rotationOffset;
        
        }

        // play sound
        e.getAudioController().playSound(m_squeakingHinges, m_rotationCenter);
    }
}
