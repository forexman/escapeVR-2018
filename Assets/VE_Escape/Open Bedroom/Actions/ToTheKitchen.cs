using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTheKitchen : AbstractAction
{
    public GameObject kitchen_area;
    public AudioSource playerVoice;
    public AudioClip voiceLine, voiceLine2;

    public float m_maxDistrance;

    private bool m_cuePlayed = false;
    private bool spoke_first = false;
    private bool asked = false;
    private bool bump = false;

    public override bool check(Environment e)
    {
        if (!spoke_first)
        {
            playerVoice.clip = voiceLine;
            playerVoice.Play();
            spoke_first = true;
        }

        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(m_audioCues[0]);
        }

        // Check if the player is at the Bedroom Door.
        PlayerTransformations pt = e.getPlayerPositions();
        //Debug.Log(Vector3.Distance(pt.HmdPosition, kitchen_area.transform.position));
        if (Vector3.Distance(pt.HmdPosition, kitchen_area.transform.position) < m_maxDistrance)
        {

            playerVoice.clip = voiceLine2;
            playerVoice.Play();
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Going to the Kitchen";
        m_cueThreshold = 120;
        playerVoice = GameObject.Find("Player Voice").GetComponent<AudioSource>();
        kitchen_area = GameObject.Find("TeleportArea Kitchen");
    }

    private void Update()
    {
        if (playerVoice == null)
        {
            playerVoice = GameObject.Find("Player Voice").GetComponent<AudioSource>();
        }
        if (kitchen_area == null)
        {
            kitchen_area = GameObject.Find("TeleportArea Kitchen");
        }

    }

}
