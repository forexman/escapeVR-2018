using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DrawScript : MonoBehaviour {

    public Text codeText;
    public int digits;
    public float lerpTime;
    public bool isDrawOpened, passDone;
    public AudioClip drawOpening;

    public string password;
    private string codeTextValue;

    public GameObject codePanel, drawer, battery, display;
    public AudioSource audioSource;


    // Use this for initialization
    void Start () {
        //this.transform.position += new Vector3(0, 0.003f, 0);
        isDrawOpened = false;
        codeTextValue = "";
        battery.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        codeText.text = codeTextValue;

        if (codeTextValue == password && !passDone && !isDrawOpened)
        {
            Debug.Log("Draw Opening");

            //Play sound of draw opening
            //if (!audioSource_mechanism.isPlaying)
            //{
            //    (audioSource_mechanism.GetComponent("Sounds_Safe") as Sounds_Safe).Sound_OpenAuto();
            //}

            (audioSource.GetComponent("Sounds_Safe") as Sounds_Safe).Sound_Correct(); //Beep to open

            //(audioSource.GetComponent("Sounds_Safe") as Sounds_Safe).Sound_Open(); //Slide to open



            battery.gameObject.SetActive(true);

            //Move draw so that it is open
            // -0.03 on y axis to open draw
            // COULD BE IMPROVED TO MOVE GRADUALLY
            this.transform.position += new Vector3(0, 0, 0.3f);

            isDrawOpened = true;

        }


        //Wrong Code, reset text field for re-entry
        if (codeTextValue.Length >= digits && !isDrawOpened)
        {
            codeTextValue = "";
            (audioSource.GetComponent("Sounds_Safe") as Sounds_Safe).Sound_Wrong();
        }

    }

    public void AddDigit(string digit)
    {
        if (!isDrawOpened)
        {
            (audioSource.GetComponent("Sounds_Safe") as Sounds_Safe).Sound_Key();
            codeTextValue += digit;
        }
    }

    public void RemoveDigit()
    {
        if (!isDrawOpened)
        {
            (audioSource.GetComponent("Sounds_Safe") as Sounds_Safe).Sound_Key();
            codeTextValue = codeTextValue.Substring(0, codeTextValue.Length - 1); ;
        }
    }

}
