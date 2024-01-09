using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds_Safe : MonoBehaviour {

    public AudioClip key_sound, code_correct, code_wrong, safe_open, safe_open_auto;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
        
	}
	
	public void Sound_Key() {
        audioSource.clip = key_sound;
        audioSource.Play();
    }

    public void Sound_Correct()
    {
        audioSource.clip = code_correct;
        audioSource.Play();
    }

    public void Sound_Wrong()
    {
        audioSource.clip = code_wrong;
        audioSource.Play();
    }

    public void Sound_Open()
    {
        audioSource.clip = safe_open;
        audioSource.Play();
    }

    public void Sound_OpenAuto()
    {
        audioSource.clip = safe_open_auto;
        audioSource.Play();
    }
}
