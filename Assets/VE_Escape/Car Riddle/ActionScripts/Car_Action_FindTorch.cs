using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Action_FindTorch : AbstractAction
{

    public AudioClip m_voiceLine;

    public GameObject m_torch;

    public float m_maxDistance;

    private bool m_cuePlayed = false;


    public override bool check(Environment e)
    {
        Debug.Log("Find Torch started");

        // Cue player to go and find a torch
        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }

        // Check if the player reaches for the torch
        PlayerTransformations pt = e.getPlayerPositions();

        Vector3 torchPosition = m_torch.transform.position;

        float leftHandDistance = (pt.LeftControllerPosition - torchPosition).magnitude;
        float rightHandDistance = (pt.RightControllerPosition - torchPosition).magnitude;

        //If distance from torch is less than 5cm
        if (leftHandDistance < 0.2f || rightHandDistance < 0.2f)
        {
            Debug.Log("Find Torch Finished");
            e.getAudioController().playSound(m_voiceLine);

            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Find Torch";
        m_cueThreshold = 60;
    }

}
