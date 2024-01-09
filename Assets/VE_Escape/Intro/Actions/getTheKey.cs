using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getTheKey : AbstractAction
{

    public GameObject key, pot, door, helpGrab;
    public AudioClip check_pot, get_key;

    public float m_maxDistrance;
    bool flag = true;


    public override bool check(Environment e)
    {

        // Check if the player looks at the door. If yes, play the voiceline and move on.
        RaycastHit hit;
        PlayerTransformations pt = e.getPlayerPositions();

        if (flag) { 
            if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, m_maxDistrance))
            {
                if (hit.collider.gameObject == door)
                {
                    e.getAudioController().playSound(check_pot);
                    door.GetComponent<Collider>().enabled = false;
                    flag = false;
                }
            }
        }

        // check whether one of the hands touches the key or not
        Vector3 keyPosition = key.transform.position;

        float leftHandDistance = (pt.LeftControllerPosition - keyPosition).magnitude;
        float rightHandDistance = (pt.RightControllerPosition - keyPosition).magnitude;

        // trigger if distance is less than 5cm
        if (leftHandDistance < 0.15f || rightHandDistance < 0.15f)
        {
            e.getAudioController().playSound(get_key);
            helpGrab.SetActive(false);
            return true;
        }

        return false;
    }

        // Use this for initialization
    void Start () {
        m_actionName = "Get the Key";
        //helpGrab.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
