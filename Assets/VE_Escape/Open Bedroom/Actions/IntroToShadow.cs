using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroToShadow : AbstractAction
{
    public GameObject bedroom_door, blood, ghost;
    public AudioSource audioSource, playerVoice, doorAudio;
    public AudioClip audioCue, voiceLine, voiceLine2, bloodVoice, bumps, wonder;

    public float m_maxDistrance;

    private bool m_cuePlayed = false;
    private bool transitioned = false;
    private bool wondered = false;
    private bool wondered2 = false;
    private bool saw_blood = false;
    private bool screamed = false;
    private bool running = false;
    private bool asked = false;
    private bool bump = false;
    private PlayerTransformations pt;

    public override bool check(Environment e)
    {
        if (SceneManager.GetActiveScene().name != "Normal World Low Res")
        {
            Animator animator = ghost.GetComponent<Animator>();
            animator.SetBool("Dead", true);
            transitioned = true;
        }

        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            playerVoice.clip = audioCue;
            playerVoice.Play();
        }


        if (transitioned & !wondered && !running)
        {
            StartCoroutine(Wonder());
        }

        if (wondered && !running && !screamed)
        {
            StartCoroutine(Scream());
        }

        if (!wondered2 && screamed)
            {
                playerVoice.clip = voiceLine2;
                playerVoice.Play();
                wondered2 = true;
            }

        if (!saw_blood)
        {
            RaycastHit hit;
            pt = e.getPlayerPositions();

            if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, 4f))
            {
                if (hit.collider.gameObject == blood)
                {
                    playerVoice.clip = bloodVoice;
                    playerVoice.Play();
                    Destroy(blood);
                    blood = null;
                    saw_blood = true;
                }
            }
        }

        // Check if the player is at the Bedroom Door.
        pt = e.getPlayerPositions();

            if (Vector3.Distance(pt.HmdPosition, bedroom_door.transform.position) < m_maxDistrance)
            {
                if (!asked && !bump && !running)
                    StartCoroutine(PlayVoiceLine());
            }

            if (asked && !bump && !running)
                StartCoroutine(PlayBumps());
            if (asked && bump && !running)
            {
                if (!saw_blood)
                {
                    Destroy(blood);
                    blood = null;
                }
                (audioSource.GetComponent("Bedroom_Crying") as Bedroom_Crying).Sound_Stop();
                return true;
            }

        
        return false;
            
    }

    IEnumerator Wonder()
    {
        PlayVoiceLine();
        yield return new WaitForSeconds(wonder.length);
        wondered = true;
        running = false;
    }

    IEnumerator Scream()
    {
        running = true;
        (audioSource.GetComponent("Bedroom_Crying") as Bedroom_Crying).Sound_Scream();
        yield return new WaitForSeconds(2);
        screamed = true;
        running = false;
    }

    IEnumerator PlayVoiceLine()
    {
        running = true;
        playerVoice.clip = voiceLine;
        playerVoice.Play();
        yield return new WaitForSeconds(voiceLine.length+3);
        asked = true;
        running = false;
    }

    IEnumerator PlayBumps()
    {
        running = true;
        doorAudio.clip = bumps;
        doorAudio.Play();
        yield return new WaitForSeconds(bumps.length);
        bump = true;
        running = false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Intro to the Shadow World";
        m_cueThreshold = 120;
    }

    void Update()
    {
        if(playerVoice == null)
        {
            playerVoice = GameObject.Find("Player Voice").GetComponent<AudioSource>();
        }
        if (audioSource)
        {
            if (!audioSource.isPlaying && !asked && !transitioned)
                (audioSource.GetComponent("Bedroom_Crying") as Bedroom_Crying).Sound_Crying();
        }
    }


}
