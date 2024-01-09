using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MacPass : MonoBehaviour {

    public Text codeText;
    public string password;

    GameObject mac, keyboard, codePanel;
    Material mailMat;
    AudioSource aS;
    AudioClip click, wrongPass, correctPass, mail;

    string codeTextValue = "";
    bool show_mail = false;
    bool passCor = false;
    bool isUnlocked = false;

    // Use this for initialization
    private void Start()
    {
        mac = transform.GetChild(0).gameObject;
        keyboard = transform.GetChild(1).gameObject;
        codePanel = keyboard.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        mailMat = transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material;

        aS = mac.GetComponent<AudioSource>();
        aS.volume = ApplicationSettings.instance.SfxVol;

        click = Resources.Load<AudioClip>("Audio/SFX/mac_key");
        wrongPass = Resources.Load<AudioClip>("Audio/SFX/mac_wrong_pass");
        correctPass = Resources.Load<AudioClip>("Audio/SFX/mac_correct_pass");
        mail = Resources.Load<AudioClip>("Audio/SFX/mac_mail");
    }

    // Update is called once per frame
    void Update()
    {
        codeText.text = codeTextValue;
        if (!isUnlocked)
        {
            if (codeTextValue == password & !passCor)
            {
                aS.clip = correctPass;
                aS.Play();
                codeTextValue = "";
                passCor = true;
            }

            if(passCor)
            {
                if (!aS.isPlaying)
                {
                    aS.clip = mail;
                    aS.Play();
                    show_mail = true;
                    isUnlocked = true;
                    mac.GetComponent<MeshRenderer>().material = mailMat;
                }
            }

            if (codeTextValue.Length >= password.Length && !passCor)
            {
                codeTextValue = "";
                aS.clip = wrongPass;
                aS.Play();
            }
        }
    }

    public void AddDigit(string digit)
    {
        if (!isUnlocked)
        {
            //Debug.Log("Adding: " + digit);
            aS.clip = click;
            aS.Play();
            codeTextValue += digit;
        }
    }

    public void RemoveDigit()
    {
        if (!isUnlocked)
        {
            aS.clip = click;
            aS.Play();
            codeTextValue = codeTextValue.Substring(0, codeTextValue.Length - 1); ;
        }
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
