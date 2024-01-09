using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Action_PerspectivePuzzle : AbstractAction
{

    public AudioClip m_voiceLine;

    public GameObject drawer, torch, battery, oldTorchBattery, newTorchBattery;
    public Light torchLight;


    public GameObject m_candleNear;
    public GameObject m_circle;
    public Light m_spotlightVase;
    public float m_degreeThreshold;
    public AudioClip m_voiceReaction;
    public bool m_perspectiveSolved = false;

    public float m_maxDistance;
    private float cue2Threshold;

    private bool m_cuePlayed = false;
    private bool m_cue2Played = false;

    // Use this for initialization
    void Start()
    {
        m_actionName = "Perspective Puzzle";
        m_cueThreshold = 100;
        cue2Threshold = 160;
        torchLight.intensity = 0;
        m_spotlightVase.intensity = 0;
    }

    public override bool check(Environment e)
    {
        Debug.Log("Perspective puzzle has started");

        //Cue player to take notice of the circle on the window
        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }

        if (!m_cue2Played && e.getCurrentTime() > m_activatedTime + cue2Threshold)
        {
            m_cue2Played = true;
            e.getAudioController().playSound(m_audioCues[1]);
        }


        // do a raycast and check if the player aligns the candle with the circle on the window
        PlayerTransformations pt = e.getPlayerPositions();
        Vector3 position = pt.HmdPosition;

        Vector3 posToCircle = m_circle.transform.position - position;
        Vector3 candleNearToCircle = m_circle.transform.position - m_candleNear.transform.position;


        float anglePuzzle = Vector3.Angle(posToCircle, candleNearToCircle);
        
        if(anglePuzzle < m_degreeThreshold)
        {
            float angleView = Vector3.Angle(posToCircle, pt.HmdForwardVector);

            // if the player aligns and looks in the direction
            if (angleView < m_degreeThreshold)
            {

                if(!m_perspectiveSolved)
                {
                    // crank up the lights
                    m_spotlightVase.intensity = 90;

                    // play reaction
                    e.getAudioController().playSound(m_voiceReaction);

                    // no more cues needed
                    m_cuePlayed = true;
                    m_cue2Played = true;

                    m_perspectiveSolved = true;
                }
                
            }
        }



        Vector3 torchPosition = torch.transform.position;
        Vector3 batteryPosition = battery.transform.position;

        float batteryDistance = (torchPosition - batteryPosition).magnitude;

        if (batteryDistance <= 0.15f)
        {
            Debug.Log("Perspective Puzzle Complete");

            e.getAudioController().playSound(m_voiceLine); //Let there be light

            torchLight.intensity = 10;
            newTorchBattery.SetActive(true);

            Destroy(battery);
            battery = null;
            Destroy(oldTorchBattery);
            oldTorchBattery = null;

            return true;
        }
        return false;
    }


}
