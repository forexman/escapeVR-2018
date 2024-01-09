using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bed_Bookshelf : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject bookshelf;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Bookshelf Puzzle";
        if (m_nextActions.Count == 0)
        {
            m_nextActions.Add(FindObjectOfType<Bed_OpenSafe>());
            m_nextActions.Add(FindObjectOfType<Car_GoToGarage>());
            m_nextActions.Add(FindObjectOfType<DnD_LookStag>());
        }
        if (bookshelf == null) bookshelf = GameObject.Find("Wall_Mechanism");

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Bookshelf_01"));
            voiceTexts.Add("These books look strange.\n Some of them are missing...");
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Bookshelf_02"));
            voiceTexts.Add("Two passwords? Just what\n am I going to find inside?");
        }

        // Hint Setup
        m_hintTxt = "HINT: The books are numbered...";

        //if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_UnlockBedroom");
       // m_hintVoiceTxt = "I should hurry upstairs.";
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        if (ApplicationSettings.instance.HasHints)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }

        //In case we do not trigger Voiceline, skip
        if (bookshelf.GetComponent<Wall_Mehanism>().IsSafeRevealed() && iVoice==0)
        {
            iVoice++;
        }

        switch (iVoice)
        {
            case 0:
                // See Bookself, voiceline
                //Check if we are in the bedroom
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[10].transform.position.y;
                if (StageController.instance.tAreas[10].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    // Check if the player looks at the bookself.
                    if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out RaycastHit hit, 10f, ~(1 << LayerMask.NameToLayer("Helmet"))))
                    {
                        if (hit.collider.gameObject == bookshelf.transform.GetChild(0).gameObject)
                        {
                            if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                        }
                    }
                }
                break;
            case 1:
                if (bookshelf.GetComponent<Wall_Mehanism>().IsSafeRevealed())
                {
                    if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                }
                break;
            default:
                GameDirector.instance.ShowHint("");
                return true;
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
}
