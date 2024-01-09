using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
    public class BedRoom_Safe : MonoBehaviour
    {
        public Text codeText;
        public float lerpTime;
        public string password1, password2;
        public GameObject codePanel, safeDoor, contents, light1, light2;
        public AudioSource audioSource, audioSource_mechanism;

        public bool isSafeOpened, pass1done, pass2done;
        string codeTextValue = "";
        private Vector3 targetAngle = new Vector3(0f, 90f, 0f);
        private Vector3 currentAngle;

        // Use this for initialization
        private void Start()
        {
            currentAngle = safeDoor.transform.eulerAngles;
            targetAngle = currentAngle + targetAngle;
            isSafeOpened = false;
            pass1done = false;
            pass2done = false;
            contents.gameObject.SetActive(false);
            codeText.text = codeTextValue;

        }

        private void Update()
        {
            if (pass1done && pass2done && !isSafeOpened)
            {
                if (!audioSource_mechanism.isPlaying)
                {
                    audioSource_mechanism.GetComponent<Sounds_Safe>().Sound_OpenAuto();
                }
                contents.gameObject.SetActive(true);
                //Debug.Log("Moving Doors from " + currentAngle + " to " + targetAngle);
                currentAngle = new Vector3(Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime / lerpTime),
                    Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime / lerpTime),
                    Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime / lerpTime));
                safeDoor.transform.eulerAngles = currentAngle;

                if (currentAngle.y >= targetAngle.y - 3f)
                {
                    isSafeOpened = true;
                    audioSource_mechanism.Stop();
                }
            }
        }

        public void AddDigit(string digit)
        {
            if (!isSafeOpened)
            {
                audioSource.GetComponent<Sounds_Safe>().Sound_Key();
                codeTextValue += digit;
                codeText.text = codeTextValue;
                if (codeTextValue.Length == password1.Length) CheckPassword();
            }
        }

        public void RemoveDigit()
        {
            if (!isSafeOpened)
            {
                audioSource.GetComponent<Sounds_Safe>().Sound_Key();
                codeTextValue = codeTextValue.Substring(0, codeTextValue.Length - 1);
                codeText.text = codeTextValue;
            }
        }

        void CheckPassword()
        {
            if ((codeTextValue == password1) && !pass1done)
            {
                pass1done = true;
                light1.transform.GetChild(0).gameObject.SetActive(true);
                light1.transform.GetChild(1).gameObject.SetActive(false);
                audioSource.GetComponent<Sounds_Safe>().Sound_Correct();
            }
            else if ((codeTextValue == password2) && !pass2done)
            {
                pass2done = true;
                light2.transform.GetChild(0).gameObject.SetActive(true);
                light2.transform.GetChild(1).gameObject.SetActive(false);
                audioSource.GetComponent<Sounds_Safe>().Sound_Correct();
            }
            else
            {
                audioSource.GetComponent<Sounds_Safe>().Sound_Wrong();
            }
            codeTextValue = "";
            codeText.text = codeTextValue;
        }
    }
}