using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStage : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (StageController.instance.StageID != 0) Destroy(this.gameObject);
    }
}
