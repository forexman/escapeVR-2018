using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpEvidence : AbstractAction
{
    public GameObject evidence, contents;
    AudioController audioSource;
    public AudioClip helpMe;

    private bool suspense = false;
    private bool grabbed = false;
    private bool helpme = false;
    private bool running = false;
    private bool movedBack = false;

    public override bool check(Environment e)
    {

        // check whether one of the hands touches the doe or not
        PlayerTransformations pt = e.getPlayerPositions();
        audioSource = e.getAudioController();

        Vector3 evPosition = evidence.transform.position;

        float leftHandDistance = (pt.LeftControllerPosition - evPosition).magnitude;
        float rightHandDistance = (pt.RightControllerPosition - evPosition).magnitude;

        // trigger if distance is less than 5cm
        if ((leftHandDistance < 0.2f || rightHandDistance < 0.2f) && contents.activeSelf)
        {
            grabbed = true;
        }

        if (grabbed && !running && !suspense)
        {
            StartCoroutine(Suspense());
        }

        if (suspense && !running && !helpme)
        {
            StartCoroutine(HelpMeGhost());
        }

        if (grabbed && helpme)
        {
            StartCoroutine(MoveToNormal());
        }

        if (movedBack)
        {
            return true;
        }

        return false;

    }

    IEnumerator Suspense()
    {
        running = true;
        yield return new WaitForSeconds(5);
        suspense = true;
        running = false;
    }

    IEnumerator HelpMeGhost()
    {
        running = true;
        audioSource.playSound(helpMe);
        yield return new WaitForSeconds(helpMe.length);
        helpme = true;
        running = false;
    }

    IEnumerator MoveToNormal()
    {
        yield return new WaitForSeconds(5);
        Valve.VR.SteamVR_Fade.Start(Color.clear, 0);
        Valve.VR.SteamVR_Fade.Start(Color.black, 3);
        SceneManager.LoadScene("Normal World Low Res", LoadSceneMode.Single);
        movedBack = true;
    }

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
   
    }
}
