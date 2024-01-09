using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAction : MonoBehaviour
{

    // Public List of next actions to be attached in the Unity UI
    public List<AbstractAction> m_nextActions;

    // Public List of AudioClips to be attached in the Unity UI
    public List<AudioClip> m_audioCues;

    protected bool m_activated = false;
    protected float m_activatedTime;
    protected float m_cueThreshold;
    protected string m_actionName;

    public AbstractAction()
    {
        m_cueThreshold = 300;
        m_audioCues = new List<AudioClip>();
        m_nextActions = new List<AbstractAction>();
        m_actionName = "Abstract Action";
    }

    // returns Name of the Action for debug purposes
    // Will be printed to log on state change
    public string getActionName()
    {
        return m_actionName;
    }

    /// <summary>
    /// called at activation of the action
    /// default implementation sets the activatedTime
    /// </summary>
    /// <param name="e">Environment parameter</param>
    public virtual void activate(Environment e)
    {
        m_activated = true;
        m_activatedTime = e.getCurrentTime();
    }

    /// <summary>
    /// Accessor for the next actions, following this one
    /// </summary>
    /// <returns>A list of AbstracActions</returns>
    public List<AbstractAction> getNextActions()
    {
        return m_nextActions;
    }

    /// <summary>
    /// check function that is called by the GameController in regular intervals.
    /// Returns if it's conditions are satisfied as an bool.
    /// </summary>
    /// <param name="e">Environment parameter</param>
    /// <returns>Returns true if it's ready to progress</returns>
    public abstract bool check(Environment e);


}
