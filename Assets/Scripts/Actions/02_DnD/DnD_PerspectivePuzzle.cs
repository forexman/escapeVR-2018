using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnD_PerspectivePuzzle : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject puzWall, candles, pLights, glassX, wallX;
    RaycastHit hit;
    RaycastHit[] hits;

    public LineRenderer laserLineRenderer;
    float laserWidth = 0.1f;
    float laserMaxLength = 20f;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Perspective Puzzle";
        if (puzWall == null) puzWall = GameObject.Find("Study_Props").transform.GetChild(0).GetChild(0).gameObject;
        if (candles == null) candles = GameObject.Find("Study_Props").transform.GetChild(1).GetChild(0).gameObject;
        if (pLights == null) pLights = GameObject.Find("Study_Props").transform.GetChild(1).GetChild(1).gameObject;
        if (glassX == null) glassX = GameObject.Find("Study_Props").transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        if (wallX == null) wallX = GameObject.Find("Study_Props").transform.GetChild(0).GetChild(1).GetChild(0).gameObject;


        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/DnD_Perspective"));
            voiceTexts.Add("Interesting... Maybe I can\n align the two crosses.");
        }

        // Hint Setup
        m_hintTxt = "HINT: Decipher the meaning of the writting on the wall. X marks the spot.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/DnD_Cue_Perspective");
        m_hintVoiceTxt = "I wonder what the writting\n on the window is about...";

        //Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        //laserLineRenderer.SetPositions(initLaserPositions);
        //laserLineRenderer.SetWidth(laserWidth, laserWidth);
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

        switch (iVoice)
        {
            case 0:
                //Turn on candle lights + glass writing
                puzWall.SetActive(true);
                foreach (Transform c in candles.transform)
                {
                    c.GetComponent<Light>().intensity = 2f;
                }
                iVoice++;
                //laserLineRenderer.enabled = false;
                break;
            case 1:
                //Sees inside cross, voiceline
                pt = e.getPlayerPositions();
                hits = Physics.RaycastAll(pt.HmdPosition, pt.HmdForwardVector, 20.0F, ~(1 << LayerMask.NameToLayer("Helmet")));

                //laserLineRenderer.enabled = true;
                //ShootLaserFromTargetPosition(pt.HmdPosition, pt.HmdForwardVector, laserMaxLength);

                for (int i = 0; i < hits.Length; i++)
                {
                    hit = hits[i];
                    //Debug.Log("Looking at: " + hit.collider.gameObject);
                    if (hit.collider.gameObject == glassX)
                    {
                        if (!crRunning) StartCoroutine(PlayVoiceAndWait(0));
                    }
                }
                break;
            case 2:
                //Crosses are united with raycast, light point_lights
                pt = e.getPlayerPositions();
                hits = Physics.RaycastAll(pt.HmdPosition, pt.HmdForwardVector, 30.0F, ~(1 << LayerMask.NameToLayer("Helmet")));

                //laserLineRenderer.enabled = true;
                //ShootLaserFromTargetPosition(pt.HmdPosition, pt.HmdForwardVector, laserMaxLength);

                for (int i = 0; i < hits.Length; i++)
                {
                    hit = hits[i];
                    //Debug.Log("2 Looking at: " + hit.collider.gameObject);
                    if (hit.collider.gameObject == glassX)
                    {
                        //Debug.Log("Saw glass X...");
                        for (int j = 0; j < hits.Length; j++)
                        {
                            hit = hits[j];
                            if (hit.collider.gameObject == wallX)
                            {
                                //Debug.Log("...and wall X");
                                foreach (Transform c in pLights.transform)
                                {
                                    c.GetComponent<Light>().intensity = 10.0f;
                                }
                                iVoice++;
                                break;
                            }
                        }
                    }
                }
                
                break;
            default:
                //laserLineRenderer.enabled = false;
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

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }
}
