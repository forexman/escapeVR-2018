using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Car_PerspectivePuzzle : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();
    RaycastHit hit;
    RaycastHit[] hits;
    GameObject drawer, safe, oProps, vase;

    LineRenderer laserLineRenderer;
    float laserWidth = 0.1f;
    float laserMaxLength = 30f;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Study Perspective Puzzle";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Car_FindCarNumber>());
        if (drawer == null) drawer = GameObject.Find("Drawer.002_Safe");
        if (safe == null) safe = GameObject.Find("Drawer_Safe");
        if (oProps == null) oProps = GameObject.Find("Office_Props");
        //laserLineRenderer = GetComponent<LineRenderer>();
        //laserLineRenderer.enabled = false;

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Car_Perspecive_01"));
            voiceTexts.Add("Huh, a keypad? I wonder\n what the colors mean...");
        }

        // Hint Setup
        m_hintTxt = "HINT: What do the candles point to?";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Car_Cue_Perspective");
        m_hintVoiceTxt = "What are these candles pointing to?";
    }

    public override bool Check(Environment e)
    {
        // Check the need for hint
        if (ApplicationSettings.instance.HasHints && iVoice!=3)
        {
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }
        if (ApplicationSettings.instance.HasHints && iVoice == 3)
        {
            m_hintVoice = null;
            m_hintTxt = "HINT: Why does the vase have colors?";
            if (m_cueThreshold <= e.getCurrentTime() - m_activatedTime)
            {
                ShowHint();
                m_activatedTime = e.getCurrentTime();
            }
        }

        if (oProps.transform.GetChild(1).GetComponent<TorchController>().IsLit) iVoice = 4;

        switch (iVoice)
        {
            case 0:
                //Activate Keypad
                safe.GetComponent<Drawer_Safe>().ActivateSafePuzzle();
                //Wait Until we see it
                pt = e.getPlayerPositions();
                // Check if the player looks at the painting.
                if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out RaycastHit hit, 10f, ~(1 << LayerMask.NameToLayer("Helmet"))))
                {
                    if (hit.collider.gameObject == drawer)
                    {
                        if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                    }
                }
                break;
            case 1:
                //Activate window circle + vase
                oProps.transform.GetChild(2).gameObject.SetActive(true);
                //Light the candles
                foreach (Transform c in oProps.transform.GetChild(0).GetChild(1))
                {
                    c.GetComponent<Light>().intensity = 2f;
                }
                iVoice++;
                break;
            case 2:
                // Check if we see vase and circle
                //Crosses are united with raycast, light point_lights
                pt = e.getPlayerPositions();
                hits = Physics.RaycastAll(pt.HmdPosition, pt.HmdForwardVector, 30.0F, ~(1 << LayerMask.NameToLayer("Helmet")));

                //laserLineRenderer.enabled = true;
                //ShootLaserFromTargetPosition(pt.HmdPosition, pt.HmdForwardVector, laserMaxLength);

                for (int i = 0; i < hits.Length; i++)
                {
                    hit = hits[i];
                    //Debug.Log("Looking at: " + hit.collider.gameObject);
                    if (hit.collider.gameObject == oProps.transform.GetChild(2).GetChild(0).gameObject)
                    {
                        //Debug.Log("Saw glass X...");
                        for (int j = 0; j < hits.Length; j++)
                        {
                            hit = hits[j];
                            if ((hit.collider.gameObject == oProps.transform.GetChild(2).GetChild(1).gameObject) 
                                || (hit.collider.gameObject.name == "Lisabo"))
                            {
                                //Debug.Log("...and vase X");
                                oProps.transform.GetChild(0).GetChild(2).GetComponent<Light>().intensity = 50.0f;
                                iVoice++;
                                break;
                            }
                        }
                    }
                }
                break;
            case 3:
                //Insert battery to torch
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

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        /*if (Physics.Raycast(ray, out raycastHit, length, ~(1 << LayerMask.NameToLayer("Helmet"))))
        {
            endPosition = raycastHit.point;
        }*/

        //laserLineRenderer.SetPosition(0, targetPosition);
        //laserLineRenderer.SetPosition(1, endPosition);
    }
}