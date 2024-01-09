using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnDs_Action_DoeSwitch : AbstractAction
{

    public GameObject m_doe;

    public AudioClip m_click;

    private bool m_cuePlayed = false;

    public override bool check(Environment e)
    {
        if(!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }


        // check whether one of the hands touches the doe or not
        PlayerTransformations pt = e.getPlayerPositions();

        Vector3 doePosition = m_doe.transform.position;

        float leftHandDistance = (pt.LeftControllerPosition - doePosition).magnitude;
        float rightHandDistance = (pt.RightControllerPosition - doePosition).magnitude;

        // trigger if distance is less than 5cm
        if (leftHandDistance < 0.15f || rightHandDistance < 0.15f)
        {
            e.getAudioController().playSound(m_click, doePosition);
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Doe Switch";
        m_cueThreshold = 120;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
