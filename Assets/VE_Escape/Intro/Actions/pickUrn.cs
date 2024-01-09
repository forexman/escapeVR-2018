using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pickUrn : AbstractAction
{
    public GameObject urn, body, lid, mist; 
    public AudioClip  guideForUrn, cueForUrn;


    public float speed = 50f;

    public bool rotate = false;
    public bool lift = false;
    public bool lifted = false;
    public bool liftlid = false;
    public bool liftedlid = false;
    public bool activate2 = false;
    private float currentLerp = 0.0f;
    private float lerpTime = 10.0f;
    bool transition = false;
    bool flag = true;
    bool m_cuePlayed = false;
    private Vector3 targetUrnPos = new Vector3(6.69f, 2.94f, -6.18f);

    public override bool check(Environment e)
    {
        if (flag)
        {
            e.getAudioController().playSound(guideForUrn);
            if (!urn.activeSelf) urn.SetActive(true);
            flag = false;
        }

        if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
        {
            m_cuePlayed = true;
            e.getAudioController().playSound(cueForUrn);
        }

        // check whether one of the hands touches the urn or not
        PlayerTransformations pt = e.getPlayerPositions();
        Vector3 urnPosition = urn.transform.position;

        float leftHandDistance = (pt.LeftControllerPosition - urnPosition).magnitude;
        float rightHandDistance = (pt.RightControllerPosition - urnPosition).magnitude;

        // trigger if distance is less than 5cm
        if (leftHandDistance < 0.35f || rightHandDistance < 0.35f)
        {
            lift = true;
        }

        if (lift && !lifted)
        {
            LiftUrn();
        }
        if (liftlid && !liftedlid)
        {
            LiftLid();
        }
        if (rotate)
        {
            RotateUrn();
        }

        if (transition)
        {
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start () {
       m_actionName = "Pick up urn";
       m_cueThreshold = 120;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LiftUrn()
    {
        rotate = true;
        //urn.transform.Translate(Vector3.forward * Time.deltaTime);
        //urn.transform.Translate(Vector3.up * 3f * Time.deltaTime);
        currentLerp += Time.deltaTime;
        if (currentLerp >= lerpTime) currentLerp = lerpTime;
        urn.transform.position = Vector3.Lerp(urn.transform.position, targetUrnPos, currentLerp / lerpTime);
        if (Vector3.Distance(urn.transform.position, targetUrnPos) <= 0.1f)
        {
            lifted = true;
            liftlid = true;
            body.transform.GetChild(0).gameObject.SetActive(true);
            mist.SetActive(true);
            StartCoroutine(MoveToShadow());
        }
        if (urn.transform.position.y > 1.7f)
        {
            lifted = true;
            liftlid = true;
            body.transform.GetChild(0).gameObject.SetActive(true);
            mist.SetActive(true);
            StartCoroutine(MoveToShadow());
            //levelController.GetComponent<SteamVR_LoadLevel>().enabled = true;
        }
    }

    IEnumerator MoveToShadow()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("Shadow World VR", LoadSceneMode.Single);
        transition = true; 
    }

    void LiftLid()
    {
        lid.transform.Translate(Vector3.forward * Time.deltaTime);
        if (lid.transform.position.y > 2.7f)
        {
            liftedlid = true;
            Destroy(lid);
            lid = null;
        }
    }

    void RotateUrn()
    {
        urn.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }

}
