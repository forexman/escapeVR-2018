using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed_SeeGhost : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject ghostLying, ghostWalking;
    public Material ghostMat;
    Vector3 ghostInitPos;
    Color myColor;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "See Ghost";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Bed_Bookshelf>());
        if (ghostLying == null) ghostLying = GameObject.Find("Woman_Dead");
        if (ghostWalking == null) ghostWalking = GameObject.Find("Bedroom_Props").transform.GetChild(0).gameObject;

        myColor = ghostMat.color;

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_SeeGhost_01"));
            voiceTexts.Add("What the");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_SeeGhost_02"));
            voiceTexts.Add("I can't believe this is really happening...");
        }

        // Hint Setup
        /*m_hintTxt = "HINT: Unlock the Bedroom door!";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_UnlockBedroom");
        m_hintVoiceTxt = "I should hurry upstairs.";*/
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        /*if (ApplicationSettings.instance.HasHints)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }*/

        switch (iVoice)
        {
            case 0:
                // See Ghost, What the
                //Check if we are in the bedroom
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[10].transform.position.y;
                if (StageController.instance.tAreas[10].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    // Check if the player looks at the ghost.
                    if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out RaycastHit hit, 10f, ~(1 << LayerMask.NameToLayer("Helmet"))))
                    {
                        if (hit.collider.gameObject == ghostLying)
                        {
                            hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                            if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                        }
                    }
                }
                break;
            case 1:
                // Activate ghost walking - I know things, noone knows
                ghostWalking.SetActive(true);
                ghostInitPos = ghostWalking.transform.position;
                if (!crRunning) StartCoroutine(GhostIknow());
                break;
            case 2:
                // Make walking ghost, walk
                ghostWalking.transform.Translate(Vector3.forward * Time.deltaTime * 0.7f);
                if (Vector3.Distance(ghostWalking.transform.position, ghostInitPos + 6f * Vector3.right) <= 1.5f)
                {
                    Destroy(ghostWalking);
                    ghostWalking = null;
                    iVoice++;
                }
                break;
            case 3:
                if (!crRunning) StartCoroutine(PlayVoiceAndWait(1));
                break;
            default:
                return true;
        }

        //Make lying ghost vanish
        if (iVoice > 0 && ghostLying!=null)
        {
            myColor.a = myColor.a - 0.002f;
            ghostMat.color = myColor;
            if (myColor.a <= 0.5f)
            {
                ParticleSystem ps = ghostLying.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.loop = false;
            }

            if (myColor.a <= 0f)
            {
                myColor.a = 1f;
                ghostMat.color = myColor;
                Destroy(ghostLying);
                ghostLying = null;
            }
        }

        return false;
    }

    IEnumerator WaitAndPlayVoice(int ivoice, float secs)
    {
        crRunning = true;
        m_voiceTxt = voiceTexts[ivoice];
        yield return new WaitForSeconds(secs);
        PlayVoiceLine(ivoice);
        iVoice++;
        crRunning = false;
    }

    IEnumerator PlayVoiceAndWait(int ivoice)
    {
        crRunning = true;
        m_voiceTxt = voiceTexts[ivoice];
        PlayVoiceLine(ivoice);
        yield return new WaitForSeconds(m_voice[ivoice].length);
        iVoice++;
        crRunning = false;
    }

    IEnumerator GhostIknow()
    {
        crRunning = true;
        yield return new WaitForSeconds(3);
        ghostWalking.GetComponent<Animator>().SetBool("GhostWalk", true);
        iVoice++;
        crRunning = false;
    }
}