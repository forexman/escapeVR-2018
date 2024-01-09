using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
    public class Kitchen_Safe : MonoBehaviour
    {

        [SerializeField]
        Text codeText;
        [SerializeField]
        string password;

        AudioSource aSrc;
        AudioClip key_sound, code_correct, code_wrong, safe_open, safeActive;

        bool isActive, isSafeOpened, isSafeUnlocked;
        string codeTextValue = "";

        // Use this for initialization
        private void Start()
        {
            isActive = false;
            isSafeUnlocked = false;
            isSafeOpened = false;
            codeText.text = codeTextValue;

            aSrc = GetComponent<AudioSource>();
            aSrc.volume = ApplicationSettings.instance.SfxVol;

            key_sound = Resources.Load<AudioClip>("Audio/SFX/Safe/input");
            code_correct = Resources.Load<AudioClip>("Audio/SFX/Safe/correct");
            code_wrong = Resources.Load<AudioClip>("Audio/SFX/Safe/wrong");
            safe_open = Resources.Load<AudioClip>("Audio/SFX/Safe/open");
            safeActive = Resources.Load<AudioClip>("Audio/SFX/Safe/active");

            //SteamVR listen for trigger
            SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);
        }

        public void AddDigit(string digit)
        {
            if (!isSafeOpened && isActive)
            {
                if(aSrc.clip != key_sound) aSrc.clip = key_sound;
                aSrc.Play();
                codeTextValue += digit;
                codeText.text = codeTextValue;
                if (codeTextValue.Length == password.Length) CheckPassword();
            }
        }

        public void RemoveDigit()
        {
            if (!isSafeOpened && isActive)
            {
                if (aSrc.clip != key_sound) aSrc.clip = key_sound;
                aSrc.Play();
                codeTextValue = codeTextValue.Substring(0, codeTextValue.Length - 1); ;
                codeText.text = codeTextValue;
            }
        }

        void CheckPassword()
        {
            if (codeTextValue == password)
            {
                if (aSrc.clip != code_correct) aSrc.clip = code_correct;
                aSrc.Play();
                isSafeUnlocked = true;
                transform.GetChild(1).gameObject.GetComponent<Interactable>().enabled = true;
            }
            else
            {
                codeTextValue = "";
                codeText.text = codeTextValue;
                if (aSrc.clip != code_wrong) aSrc.clip = code_wrong;
                aSrc.Play();
            }
        }

        public void ActivateSafePuzzle()
        {
            if (!isActive)
            {
                isActive = true;
                //Turn Light On
                transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                if (aSrc.clip != safeActive) aSrc.clip = safeActive;
                aSrc.Play();
            }
        }

        public bool IsSafeOpen()
        {
            return isSafeOpened;
        }

        private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            //Trying to open a locked door
            if (isSafeUnlocked) 
            {
                if (transform.GetChild(1).gameObject.GetComponent<Interactable>().hoveringHand)
                {
                    if (aSrc.clip != safe_open)
                    {
                        isSafeOpened = true;
                        aSrc.clip = safe_open;
                        aSrc.Play();
                        transform.GetChild(1).eulerAngles = new Vector3(
                            transform.GetChild(1).eulerAngles.x,
                            transform.GetChild(1).eulerAngles.y + 120,
                            transform.GetChild(1).eulerAngles.z
                        );

                        //Showing Key
                        this.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}