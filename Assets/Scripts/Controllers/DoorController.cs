using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
    public class DoorController : MonoBehaviour
    {
        public GameObject Door { get; set; }
        public GameObject Handle_Base { get; set; }
        public GameObject Lock { get; set; }
        public GameObject Handles { get; set; }
        public AudioSource Door_Audio { get; set; }

        public int Key_ID;
        public bool DoorLocked { get; set; }

        private AudioClip doorLocked, doorUnlock;
        CircularDrive cd;

        // Use this for initialization
        void Start()
        {
            //SteamVR listen for trigger
            SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);

            doorLocked = Resources.Load<AudioClip>("Audio/SFX/door_locked");
            doorUnlock = Resources.Load<AudioClip>("Audio/SFX/door_unlock");

            DoorLocked = true;
            Door_Audio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            Door_Audio.volume = ApplicationSettings.instance.SfxVol;

            //Door Setup
            Door = transform.GetChild(0).gameObject;
            Interactable inter = Door.AddComponent(typeof(Interactable)) as Interactable;
            LinearMapping lin = Door.AddComponent(typeof(LinearMapping)) as LinearMapping;
            cd = Door.AddComponent(typeof(CircularDrive)) as CircularDrive;
            cd.linearMapping = lin;
            cd.axisOfRotation = CircularDrive.Axis_t.ZAxis;
            cd.limited = true;
            cd.minAngle = 0;
            cd.maxAngle = 100;
            cd.enabled = false;
            cd.startAngle = 100;
            cd.forceStart = true;

            //Handle_Base Setup
            Handle_Base = Door.transform.GetChild(0).gameObject;

            //Lock Setup
            Lock = Handle_Base.transform.GetChild(0).gameObject;
            KeyInput ki = Lock.AddComponent(typeof(KeyInput)) as KeyInput;

            //Handles Setup
            Handles = Handle_Base.transform.GetChild(1).gameObject;
        }

        public void UnlockDoor()
        {
            DoorLocked = false;
            if (Door_Audio.clip != doorUnlock) Door_Audio.clip = doorUnlock;
            if (!Door_Audio.isPlaying) Door_Audio.Play();
            cd.enabled = true;
            Destroy(Handles.GetComponent<Interactable>());
            enabled = false;
        }

        private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            //Debug.Log("Trigger pressed at " + this);
            //Trying to open a locked door
            if (DoorLocked)
            {
                if (Handles.GetComponent<Interactable>().hoveringHand || Door.GetComponent<Interactable>().hoveringHand)
                {
                    //Debug.Log("Trigger pressed at " + this);
                    if (!Door_Audio.isPlaying)
                    {
                        if (Door_Audio.clip != doorLocked) Door_Audio.clip = doorLocked;
                        Door_Audio.Play();
                    }
                }
            }
        }

        public void PlayDoorBumps()
        {
            if (Door_Audio.clip != doorLocked) Door_Audio.clip = doorLocked;
            Door_Audio.Play();
        }
    }

}