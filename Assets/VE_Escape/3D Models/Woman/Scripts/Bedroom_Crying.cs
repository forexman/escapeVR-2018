using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bedroom_Crying : MonoBehaviour
{

    public AudioClip scream, cry, beg;
    public AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource.clip = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.clip == cry)
        {
            if(!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    public void Sound_Crying()
    {
        audioSource.clip = cry;
        audioSource.Play();
    }

    public void Sound_Scream()
    {
        audioSource.clip = scream;
        audioSource.Play();
    }

    public void Sound_Beg()
    {
        audioSource.clip = beg;
        audioSource.Play();
    }

    public void Sound_Stop()
    {
        Debug.Log("Stopping Sound");
        audioSource.clip = null;
        audioSource.Stop();
    }
}
