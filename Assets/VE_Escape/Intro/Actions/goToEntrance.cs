using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goToEntrance : AbstractAction
{
    public GameObject urn, TPAreaEntrance;
    public AudioClip doorbell, guideToEntrance;
    public AudioSource door;

    bool flag1 = true;
    bool flag2 = true;
	bool m_cuePlayed = false;
    private bool running = false;

    public override bool check(Environment e)
    {
        if (flag1 && !running)
        {
            StartCoroutine(WaitForBell());
        }

        if (!flag1 && flag2 && !running)
        {
            e.getAudioController().playSound(guideToEntrance);
            flag2 = false;
        }

        RaycastHit hit;
        PlayerTransformations pt = e.getPlayerPositions();

        if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, 9f))
        {
            if (hit.collider.gameObject == urn)
            {
                urn.GetComponent<BoxCollider>().enabled = false;
                //urn.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                return true;
            }
        }

        return false;
    }

    IEnumerator WaitForBell()
    {
        running = true;
        door.clip = doorbell;
        door.Play();
        yield return new WaitForSeconds(doorbell.length);
        flag1 = false;
        urn.SetActive(true);
        running = false;
    }

    // Use this for initialization
    void Start()
    {
        m_actionName = "Go To Entrance";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
