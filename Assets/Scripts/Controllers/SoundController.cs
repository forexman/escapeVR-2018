using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name == "BG_Audio") GetComponent<AudioSource>().volume = ApplicationSettings.instance.BgVol;
        else if(gameObject.name == "SFX_Audio") GetComponent<AudioSource>().volume = ApplicationSettings.instance.SfxVol + 0.2f;
        else GetComponent<AudioSource>().volume = ApplicationSettings.instance.SfxVol;
    }
}
