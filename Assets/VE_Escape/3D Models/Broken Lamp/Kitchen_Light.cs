using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Kitchen_Light : MonoBehaviour
{
    public GameObject lightBulb;
    public AudioSource audioSource;
    public Light light;

    private bool isRightBulb, isLit;

    // Use this for initialization
    void Start()
    {
        isRightBulb = false;
        isLit = false;
        light.intensity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRightBulb && !isLit)
        {
            isLit = true;
            light.intensity = 12f;
            Destroy(lightBulb);
            lightBulb = null;
            //lightBulb.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name + " Key Entered " + gameObject.name + " keyhole");

        if (other.gameObject == lightBulb)
        {
            isRightBulb = true;
        }
    }
}

