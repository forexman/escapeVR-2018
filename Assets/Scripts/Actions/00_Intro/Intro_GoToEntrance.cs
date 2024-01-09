using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_GoToEntrance : SimpleAction
{
    public GameObject doorBell, urn;
    int iVoice = 0;
    PlayerTransformations pt;
    Vector3 hmdPosition;

    void Start()
    {
        if (!urn) urn = GameObject.Find("Normal_World_Props").transform.GetChild(0).GetChild(0).gameObject;
        if (!doorBell) doorBell = GameObject.Find("DoorBell");
        doorBell.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/SFX/doorbell");
        doorBell.GetComponent<AudioSource>().volume = ApplicationSettings.instance.SfxVol;

        //SimpleAction Setup
        m_actionName = "Go to Entrance";
        if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_CheckUrn>());

        if (m_voice.Count == 0)
        {
            m_voice.Add(Resources.Load<AudioClip>("Audio/VoiceLines/Intro_Delivery"));
        }
        m_voiceTxt = "Ah, and my delivery is here as well. \n" +
            "These guys are really quick";

        m_hintTxt = "HINT: Check on your delivery at the door.";

        if (!m_hintVoice) m_hintVoice = Resources.Load<AudioClip>("Audio/VoiceLines/Intro_Cue_Delivery");
        m_hintVoiceTxt = "My stuff is at the entrance, I probably should pick it.";
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

        if (iVoice == 0)
        {
            if (!urn.activeSelf) urn.SetActive(true);
            StartCoroutine(DoorBellRinging());
            iVoice++;
        }
        else
        {
            //Check if we went downstairs
            //Debug.Log("Checking pos");
            pt = e.getPlayerPositions();
            hmdPosition = pt.HmdPosition;
            hmdPosition.y = StageController.instance.tAreas[0].transform.position.y;
            //Debug.Log(hmdPosition);
            if (StageController.instance.tAreas[0].GetComponent<Collider>().bounds.Contains(hmdPosition))
            {
                //Debug.Log("I am at the entrance");
                // Check if the player looks at the urn.
                if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out RaycastHit hit, 10f, ~(1 << LayerMask.NameToLayer("Helmet"))))
                {
                    if (hit.collider.gameObject == urn)
                    {
                        //Debug.Log("Looking at: " + hit.collider.gameObject);
                        GameDirector.instance.ShowHint("");
                        return true;
                    }
                }
            }

        }

        return false;
    }

    IEnumerator DoorBellRinging()
    {
        doorBell.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(doorBell.GetComponent<AudioSource>().clip.length + 1f);
        PlayVoiceLine(0);
    }
}
