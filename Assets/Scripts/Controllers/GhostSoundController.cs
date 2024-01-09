using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSoundController : MonoBehaviour
{
    AudioClip scream, cry;
    AudioSource aSrc;

    // Start is called before the first frame update
    void Start()
    {
        scream = Resources.Load<AudioClip>("Audio/SFX/woman_scream");
        cry = Resources.Load<AudioClip>("Audio/SFX/woman_crying");
        
        aSrc = GetComponent<AudioSource>();
        aSrc.volume = ApplicationSettings.instance.SfxVol;
        aSrc.loop = false;
    }

    public void PlayScream()
    {
        StartCoroutine(CrPlayScream());
    }

    public void StopCry()
    {
        aSrc.Stop();
    }

    IEnumerator CrPlayScream()
    {
        aSrc.clip = scream;
        aSrc.Play();
        yield return new WaitForSeconds(scream.length + 1);
        aSrc.clip = cry;
        aSrc.loop = true;
        aSrc.Play();
    }
}
