using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformations
{
    public PlayerTransformations(Vector3 leftContrPos, Quaternion leftContrRot, Vector3 rightContrPos, Quaternion rightContrRot, Vector3 hmdPos, Vector3 hmdForwadVector, Quaternion hmdRot, Vector3 playerPos, Quaternion playerRot)
    {
        LeftControllerPosition = leftContrPos;
        LeftControllerRotation = leftContrRot;

        RightControllerPosition = rightContrPos;
        RightControllerRotation = rightContrRot;

        HmdPosition = hmdPos;
        HmdForwardVector = hmdForwadVector;
        HmdRotation = hmdRot;

        PlayerPosition = playerPos;
        PlayerRotation = playerRot;
    }

    public Vector3 LeftControllerPosition { get; private set; }
    public Quaternion LeftControllerRotation { get; private set; }

    public Vector3 RightControllerPosition { get; private set; }
    public Quaternion RightControllerRotation { get; private set; }

    public Vector3 HmdPosition { get; private set; }
    public Vector3 HmdForwardVector { get; private set; }
    public Quaternion HmdRotation { get; private set; }

    public Vector3 PlayerPosition { get; private set; }
    public Quaternion PlayerRotation { get; private set; }
}

public class Environment : MonoBehaviour {

    private GameObject m_leftController, m_rightController, m_hmd, m_player;
    private AudioController m_audioController;


    /// <summary>
    /// Inits Environment. Is called by the GameDirector to set the Controller and HMD for the positions
    /// </summary>
    /// <param name="leftController">GameObject that is attached to or the left controller</param>
    /// <param name="rightController">GameObject that is attached to or the right controller</param>
    /// <param name="hmd">GameObject that is attached to or the HMD</param>
    public void initEnvironment(GameObject leftController, GameObject rightController, GameObject hmd)
    {
        m_leftController = leftController;
        m_rightController = rightController;
        m_hmd = hmd;
        m_player = null;
    }
    public void initEnvironment(GameObject leftController, GameObject rightController, GameObject hmd, GameObject player)
    {
        m_leftController = leftController;
        m_rightController = rightController;
        m_hmd = hmd;
        m_player = player;
    }

    /// <summary>
    /// Get the time at the start of the current frame since start of the application
    /// </summary>
    /// <returns>Seconds since start of the game</returns>
    public float getCurrentTime()
    {
        return Time.time;
    }

    /// <summary>
    /// Returns the position and orientation of the players head and hands
    /// </summary>
    /// <returns>Object containing players positions and orientations</returns>
    public PlayerTransformations getPlayerPositions()
    {

        Vector3 leftContrPos;
        Quaternion leftContrRot;
        if (m_leftController)
        {
            leftContrPos = m_leftController.transform.position;
            leftContrRot = m_leftController.transform.rotation;
        }
        else
        {
            leftContrPos = new Vector3();
            leftContrRot = new Quaternion();
        }

        Vector3 rightContrPos;
        Quaternion rightContrRot;
        if (m_rightController)
        {
            rightContrPos = m_rightController.transform.position;
            rightContrRot = m_rightController.transform.rotation;
        }
        else
        {
            rightContrPos = new Vector3();
            rightContrRot = new Quaternion();
        }

        Vector3 hmdPos;
        Vector3 hmdForwardVector;
        Quaternion hmdRot;
        if (m_hmd)
        {
            hmdPos = m_hmd.transform.position;
            hmdForwardVector = m_hmd.transform.forward;
            hmdRot = m_hmd.transform.rotation;
        }
        else
        {
            hmdPos = new Vector3();
            hmdForwardVector = new Vector3();
            hmdRot = new Quaternion();
        }

        Vector3 playerPos;
        Quaternion playerRot;
        if (m_player)
        {
            playerPos = m_player.transform.position;
            playerRot = m_player.transform.rotation;
        }
        else
        {
            playerPos = new Vector3();
            playerRot = new Quaternion();
        }

        return new PlayerTransformations(leftContrPos, leftContrRot, rightContrPos, rightContrRot, hmdPos, hmdForwardVector, hmdRot, playerPos, playerRot);
    }

    /// <summary>
    /// Gives access to the object marked as the left controller
    /// </summary>
    /// <returns>GameObject marked as left controller</returns>
    public GameObject getLeftController()
    {
        return m_leftController;
    }

    /// <summary>
    /// Gives access to the object marked as the right controller
    /// </summary>
    /// <returns>GameObject marked as right controller</returns>
    public GameObject getRightController()
    {
        return m_rightController;
    }

    /// <summary>
    /// Gives access to the object marked as the hmd
    /// </summary>
    /// <returns>GameObject marked as hmd</returns>
    public GameObject getHmd()
    {
        return m_hmd;
    }

    public GameObject getPlayer()
    {
        return m_player;
    }

    public AudioController getAudioController()
    {
        return m_audioController;
    }
}
