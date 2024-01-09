using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Action_BackFromTheShadows : AbstractAction
{
    public AudioClip m_voiceLine;

    public override bool check(Environment e)
    {
        e.getAudioController().playSound(m_voiceLine);
        return true;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Return from the Shadows";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
