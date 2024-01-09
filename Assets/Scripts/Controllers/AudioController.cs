using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioController : MonoBehaviour
{

    List<AudioSource> m_sources;

    private GameObject m_hmd;

    public void initAudioController(GameObject hmd)
    {
        m_hmd = hmd;
        m_sources = new List<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // check for all sources if they are still playing
        // queue the ones who are finished
        List<AudioSource> finished = new List<AudioSource>();

        foreach (AudioSource source in m_sources)
        {
            if (!source.isPlaying)
                finished.Add(source);
        }

        // remove finished sources
        foreach (AudioSource finSource in finished)
        {
            m_sources.Remove(finSource);
            Destroy(finSource.gameObject);
        }
    }

    /// <summary>
    /// plays a specified sound at the players position
    /// </summary>
    public void playSound(AudioClip clip)
    {
        AudioSource source = new GameObject().AddComponent<AudioSource>();
        source.loop = false;
        source.clip = clip;
        source.Play();

        source.transform.position = m_hmd.transform.position;
        source.transform.parent = m_hmd.transform;

        m_sources.Add(source);
    }

    /// <summary>
    /// Plays a specified sound at the specified position
    /// </summary>
    /// <param name="clip">The clip played
    /// <param name="position">The position the sound is played at</param>
    public void playSound(AudioClip clip, Vector3 position)
    {

        AudioSource source = new GameObject().AddComponent<AudioSource>();
        source.loop = false;
        source.clip = clip;
        source.Play();

        source.transform.position = position;

        m_sources.Add(source);
    }

    /// <summary>
    /// Plays a specified sound at the specified gameObject, following it
    /// </summary>
    /// <param name="clip">The clip played
    /// <param name="gameObject">The gameObject the sound is played at</param>
    public void playSound(AudioClip clip, GameObject gameObject)
    {
        AudioSource source = new GameObject().AddComponent<AudioSource>();
        source.loop = false;
        source.clip = clip;
        source.Play();

        source.transform.position = gameObject.transform.position;
        source.transform.parent = gameObject.transform;

        m_sources.Add(source);
    }
}
