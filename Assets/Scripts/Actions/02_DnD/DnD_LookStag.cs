using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnD_LookStag : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    GameObject painting, perspectivePuzz;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Look at Stag";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<DnD_DoeSwitch>());
        if (painting == null) painting = GameObject.Find("Painting_Deer");
        if (perspectivePuzz == null) perspectivePuzz = GameObject.Find("Study_Props").transform.GetChild(0).gameObject;

        // Voicelines Setup
        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/DnD_LookAtStag"));
            voiceTexts.Add("There is the stag. Just\n need to find its doe now.");
        }

        // Hint Setup
        //m_hintTxt = "HINT: Unlock the Bedroom door!";

        //if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Bed_Cue_UnlockBedroom");
        //m_hintVoiceTxt = "I should hurry upstairs.";
    }

    public override bool Check(Environment e)
    {
        /*// Check the need for hint
        if (ApplicationSettings.instance.HasHints)
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
                //Activate Writting on the wall
                perspectivePuzz.SetActive(true);

                // See Painting, What the
                //Check if we are in the study
                pt = e.getPlayerPositions();
                hmdPosition = pt.HmdPosition;
                hmdPosition.y = StageController.instance.tAreas[2].transform.position.y;
                if (StageController.instance.tAreas[2].GetComponent<Collider>().bounds.Contains(hmdPosition))
                {
                    // Check if the player looks at the painting.
                    if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out RaycastHit hit, 10f, ~(1 << LayerMask.NameToLayer("Helmet"))))
                    {
                        if (hit.collider.gameObject == painting)
                        {
                            if (!crRunning) StartCoroutine(PlayVoiceAndWait(iVoice));
                        }
                    }
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
