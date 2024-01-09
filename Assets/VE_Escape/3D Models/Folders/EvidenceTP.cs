using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvidenceTP : MonoBehaviour {

    public GameObject evidence, contents, LeftController, RightController;
    AudioSource audioSource;
    public AudioClip helpMe;

    private bool grabbed = false;
    private bool helpme = false;
    private bool running = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float leftHandDistance = (LeftController.transform.position - evidence.transform.position).magnitude;
        float rightHandDistance = (RightController.transform.position - evidence.transform.position).magnitude;

        // trigger if distance is less than 5cm
        if (leftHandDistance < 0.35f || rightHandDistance < 0.35f && contents.activeSelf)
        {
            grabbed = true;
        }

        if(grabbed && !running && !helpme)
        {
            StartCoroutine(HelpMeGhost());
        }

        if (grabbed && helpme)
        {
            StartCoroutine(MoveToNormal());
        }
    }

    IEnumerator HelpMeGhost()
    {
        running = true;
        audioSource.clip = helpMe;
        audioSource.Play();
        yield return new WaitForSeconds(helpMe.length);
        helpme = true;
        running = false;
    }

    IEnumerator MoveToNormal()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Normal World Low Res", LoadSceneMode.Single);
    }
}
