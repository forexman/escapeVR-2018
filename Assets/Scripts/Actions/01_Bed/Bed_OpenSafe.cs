using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Bed_OpenSafe : SimpleAction
{
    int iVoice = 0;
    bool crRunning = false;
    PlayerTransformations pt;
    Vector3 hmdPosition;
    List<string> voiceTexts = new List<string>();

    public BedRoom_Safe safe;

    void Start()
    {
        //SimpleAction Setup
        m_actionName = "Open Safe";
        //if (m_nextActions.Count == 0) m_nextActions.Add(FindObjectOfType<Intro_CheckUrn>());
        if (safe == null) safe = FindObjectOfType<BedRoom_Safe>();

        //SteamVR listen for trigger
        SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);
    }

    public override bool Check(Environment e)
    {

        switch (iVoice)
        {
            case 0:
                // Wait for evidence pick up
                break;
            case 1:
                // Going back to normal world, outro
                if (!crRunning) StartCoroutine(MoveToNormal());
                break;
            default:
                GameDirector.instance.ShowHint("");
                return true;
        }
        return false;
    }

    IEnumerator MoveToNormal()
    {
        crRunning = true;
        SteamVR_Fade.Start(Color.black, 5);
        yield return new WaitForSeconds(6);
        StageController.instance.MoveToNormalRealm();
        iVoice++;
        crRunning = false;
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

    private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (safe.contents.transform.GetChild(0).GetComponent<Interactable>().hoveringHand && iVoice == 0)
        {
            //Debug.Log("Evidence picked");
            iVoice++;
        }
    }
}
