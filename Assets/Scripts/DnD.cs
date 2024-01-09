using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DnD : MonoBehaviour
{
    public string tag;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

        if (objs.Length > 1)
        {
            Debug.Log("Found more than 1 of this tag, destroying...");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}