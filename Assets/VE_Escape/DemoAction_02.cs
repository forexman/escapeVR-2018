using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAction_02 : AbstractAction {

    public GameObject m_cube;

    private Quaternion m_initialRot;

    // Use this for initialization
    void Start()
    {
        m_actionName = "Cube rotation";
    }

    public override bool check(Environment e)
    {
        if (Quaternion.Angle(m_initialRot, m_cube.transform.rotation) >= 90)
            return true;
        else
            return false;
    }


    public override void activate(Environment e)
    {
        base.activate(e);
        m_initialRot = m_cube.transform.rotation;
    }
}
