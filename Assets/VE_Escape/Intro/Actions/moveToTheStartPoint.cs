using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToTheStartPoint : AbstractAction
{
    public GameObject marker, TPEntrance;
    public AudioClip moveIntoTheHouse;
    private Vector3 AreaPos;


    private bool flag = true;

    public override bool check(Environment e)
    {
        if (flag) {
            e.getAudioController().playSound(moveIntoTheHouse);
            flag = false;
        }

        // check whether the player reaches the marker
        PlayerTransformations pt = e.getPlayerPositions();

        Vector3 markerPosition = marker.transform.position;
        Vector3 hmdPosition = pt.HmdPosition;

        // set y for both to 0.0, as height is not relevant for this check
        markerPosition.y = 0.0f;
        hmdPosition.y = 0.0f;


        float distance = (hmdPosition - markerPosition).magnitude;
        // Debug.Log(distance);

        // trigger if distance is less than ？cm
        if (distance < 0.8f)
        {
            marker.transform.position = new Vector3(marker.transform.position.x, marker.transform.position.y, marker.transform.position.z +10f);
            TPEntrance.transform.position = AreaPos;
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start () {
        m_actionName = "Move To The Start Point";
        AreaPos = TPEntrance.transform.position;
        TPEntrance.transform.position = new Vector3(TPEntrance.transform.position.x, TPEntrance.transform.position.y, TPEntrance.transform.position.z -10f);      
    }
}
