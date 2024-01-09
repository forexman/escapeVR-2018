using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnDs_Action_Candles : AbstractAction {

    private float m_puzzleStartedTime = 0;

    public Light m_candleLight_01;
    public Light m_candleLight_02;
    public Light m_candleLight_03;

    public override bool check(Environment e)
    {

        float passedTime = e.getCurrentTime() - m_puzzleStartedTime;

        
        if (passedTime < 60)
        {
            // first minute, nothing happens
            return false;
        }
        else if(passedTime < 120)
        {
            // second minute, crank up the lights!
            //float t = (passedTime - 60) / 60;

            m_candleLight_01.intensity = 10f;
            //m_candleLight_02.intensity = t;
            //m_candleLight_03.intensity = t;
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void activate(Environment e)
    {
        base.activate(e);
        m_puzzleStartedTime = e.getCurrentTime();
    }

    // Use this for initialization
    void Start () {
        m_actionName = "Lighting up candles";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
