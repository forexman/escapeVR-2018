using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkEmail : AbstractAction
{
    public GameObject Pc;
    public AudioClip check_email;

    bool flag = true;

    public override bool check(Environment e)
    {
        if (flag)
        {
            e.getAudioController().playSound(check_email);
            flag = false;
        }

        if (Pc.GetComponent<MacPass>().IsUnlocked())
        {
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start () {
        m_actionName = "Check the Email";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
