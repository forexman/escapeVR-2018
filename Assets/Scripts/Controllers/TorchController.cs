using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TorchController : MonoBehaviour
{
    public bool IsLit { get; set; }

    private void Start()
    {
        IsLit = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            
            //Destroy Battery
            Destroy(collision.gameObject);

            //close lid
            transform.GetChild(2).gameObject.SetActive(true);

            //Turn up light to 10
            transform.GetChild(0).GetComponent<Light>().intensity = 20f;
            transform.GetChild(1).GetComponent<Light>().intensity = 20f;

            this.GetComponent<BoxCollider>().isTrigger = false;

            IsLit = true;
        }
    }
}
