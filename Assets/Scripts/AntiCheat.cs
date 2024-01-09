using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AntiCheat : MonoBehaviour
{
    public bool verbose = false;
    /*private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.gameObject.tag == "Wall")
        {
            //Debug.Log("Collision With Wall: "+ collision.transform.gameObject.name);
            SteamVR_Fade.Start(Color.black, 0.2f);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.gameObject.tag == "Wall")
        {
            SteamVR_Fade.Start(Color.clear, 0.2f);
        }
    }*/
    private void OnTriggerEnter(Collider collision)
    {
            if (verbose) Debug.Log("Collision With: "+ collision.transform.gameObject.name);
            SteamVR_Fade.Start(Color.black, 0.2f);
    }

    private void OnTriggerExit(Collider collision)
    {
            SteamVR_Fade.Start(Color.clear, 0.2f);
    }
}
