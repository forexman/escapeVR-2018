using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleAction : MonoBehaviour
{
    // Public List of next actions to be attached in the Unity UI
    public List<SimpleAction> m_nextActions;

    // Audio
    [Header("Voice Lines")]
    [Tooltip("Starting Voice Line")]
    public List<AudioClip> m_voice;
    protected string m_voiceTxt;
    [Tooltip("Hint Voice Line")]
    public AudioClip m_hintVoice;
    protected string m_hintVoiceTxt;
    protected string m_hintTxt;

    // Vars
    protected bool m_activated = false;
    protected float m_activatedTime;
    protected float m_cueThreshold;
    protected string m_actionName;

    public SimpleAction()
    {
        m_cueThreshold = 60;
        m_nextActions = new List<SimpleAction>();
        m_actionName = "Abstract Action";
    }

    // returns Name of the Action for debug purposes
    // Will be printed to log on state change
    public string GetActionName()
    {
        return m_actionName;
    }

    /// <summary>
    /// called at activation of the action
    /// default implementation sets the activatedTime
    /// </summary>
    /// <param name="e">Environment parameter</param>
    public virtual void Activate(Environment e)
    {
        m_activated = true;
        m_cueThreshold = ApplicationSettings.instance.HintThres * 60;
        m_activatedTime = e.getCurrentTime();

        //Clearing Hint
        if (ApplicationSettings.instance.HasHints)
            GameDirector.instance.ShowHint("");
    }

    /// <summary>
    /// Accessor for the next actions, following this one
    /// </summary>
    /// <returns>A list of AbstracActions</returns>
    public List<SimpleAction> GetNextActions()
    {
        return m_nextActions;
    }

    /// <summary>
    /// check function that is called by the GameController in regular intervals.
    /// Returns if it's conditions are satisfied as an bool.
    /// </summary>
    /// <param name="e">Environment parameter</param>
    /// <returns>Returns true if it's ready to progress</returns>
    public abstract bool Check(Environment e);

    public void PlayVoiceLine(int i)
    {
        if(GameDirector.instance.Sfx_audio.isPlaying) GameDirector.instance.Sfx_audio.Stop();
        GameDirector.instance.Sfx_audio.clip = m_voice[i];
        GameDirector.instance.Sfx_audio.Play();
        if (ApplicationSettings.instance.HasSubtitles)
            GameDirector.instance.ShowSubtitle(m_voiceTxt, m_voice[i].length);
    }

    public void ShowHint()
    {
        if (m_hintVoice)
        {
            GameDirector.instance.Sfx_audio.clip = m_hintVoice;
            GameDirector.instance.Sfx_audio.Play();
            GameDirector.instance.ShowSubtitle(m_hintVoiceTxt, m_hintVoice.length);
        }
        GameDirector.instance.ShowHint(m_hintTxt);
    }
}
