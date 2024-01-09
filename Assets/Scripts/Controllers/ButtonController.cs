using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ButtonController : MonoBehaviour
{
    public MacPass mac;
    public Kitchen_Safe kSafe;
    public BedRoom_Safe bSafe;
    public Drawer_Safe dSafe;
    public string number;

    // Start is called before the first frame update
    void Start()
    {
        SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);
    }

    private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (this.GetComponent<Interactable>().hoveringHand)
        {
            //Debug.Log("Trigger pressed at " + this);
            if (number == "Delete")
            {
                if(mac!=null) mac.RemoveDigit();
                else if (kSafe != null) kSafe.RemoveDigit();
                else if (bSafe != null) bSafe.RemoveDigit();
                else if (dSafe != null) dSafe.RemoveDigit();
            }
            else
            {
                if (mac != null) mac.AddDigit(number);
                else if (kSafe != null) kSafe.AddDigit(number);
                else if (bSafe != null) bSafe.AddDigit(number);
                else if (dSafe != null) dSafe.AddDigit(number);
            }
        }
    }
}
