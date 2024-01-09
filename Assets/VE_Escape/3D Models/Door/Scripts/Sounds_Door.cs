using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds_Door : MonoBehaviour {

    public AudioClip locked_sound, unlock_sound;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        
	}
	
	public void Sound_Locked() {
        audioSource.clip = locked_sound;
        audioSource.Play();
    }

    public void Sound_Unlocked()
    {
        audioSource.clip = unlock_sound;
        audioSource.Play();
    }
    
}
