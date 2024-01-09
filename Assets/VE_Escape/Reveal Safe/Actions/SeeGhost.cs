using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeGhost : AbstractAction {

    public GameObject bedroom_area, ghostdead, ghostwalking;
    public AudioClip voiceLine, whatThe, iKnow;
    public AudioSource playerVoice, womanSrc;
    public Material ghostMat;

    public float m_maxDistrance;

    private bool picked_key = false;
    private bool running = false;
    private bool asked = false;
    private bool ghostwalk = false;
    private bool iknow = false;
    private PlayerTransformations pt;
    private Animator animator;
    private Vector3 ghost_pos;
    private Color myColor;

    public override bool check(Environment e)
    {

        if (!asked && !running)
        {
            RaycastHit hit;
            pt = e.getPlayerPositions();

            if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, 10f))
            {
                if (hit.collider.gameObject == ghostdead)
                {
                    StartCoroutine(WhatThe());
                }
            }
        }

        if (asked && !iknow && !running)
        {
            ghostwalking.SetActive(true);
            StartCoroutine(Iknow());
        }

        if (ghostwalk)
        {
            playerVoice.clip = voiceLine;
            playerVoice.Play();
            return true;
        }

        return false;

    }

    IEnumerator WhatThe()
    {
        running = true;
        playerVoice.clip = whatThe;
        playerVoice.Play();
        yield return new WaitForSeconds(whatThe.length);
        ghostwalking.SetActive(true);
        asked = true;
        running = false;
    }

    IEnumerator Iknow()
    {
        running = true;
        womanSrc.clip = iKnow;
        womanSrc.Play();
        yield return new WaitForSeconds(iKnow.length);
        animator.SetBool("GhostWalk", true);
        iknow = true;
        running = false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Going to the Bedroom";
        ghost_pos = ghostwalking.transform.position;
        animator = ghostwalking.GetComponent<Animator>();
        myColor = ghostMat.color;
        playerVoice = GameObject.Find("Player Voice").GetComponent<AudioSource>();
        bedroom_area = GameObject.Find("TeleportArea Bedroom");
    }

    void Update()
    {
        if (playerVoice == null)
        {
            playerVoice = GameObject.Find("Player Voice").GetComponent<AudioSource>();
        }
        if (bedroom_area == null)
        {
            bedroom_area = GameObject.Find("TeleportArea Bedroom");
        }

        if (asked)
        {
            if (ghostdead)
            {
                myColor.a = myColor.a - 0.002f;
                ghostMat.color = myColor;
                if (myColor.a <= 0.5f)
                {
                    ParticleSystem ps = ghostdead.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                    var main = ps.main;
                    main.loop = false;
                }

                if (myColor.a <= 0f)
                {
                    myColor.a = 1f;
                    ghostMat.color = myColor;
                    Destroy(ghostdead);
                    ghostdead = null;
                }
            }
        }

        if (iknow && !ghostwalk)
        {

            ghostwalking.transform.Translate(Vector3.forward * Time.deltaTime * 0.7f);
            if (Vector3.Distance(ghostwalking.transform.position, ghost_pos + 6f * Vector3.right)<=1.5f)
            {
                ghostwalk = true;
                Destroy(ghostwalking);
                ghostwalking = null;
            }
        }
    }
}
