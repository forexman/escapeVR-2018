using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class KeyController : MonoBehaviour
{

    public int Key_ID;
    BoxCollider bc;
    Rigidbody rb;
    Interactable ib;
    VelocityEstimator ve;
    Throwable thr;

    // Start is called before the first frame update
    void Start()
    {
        this.tag = "Key";
        bc = GetComponent<BoxCollider>();
        if (!bc)
            bc = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        rb = GetComponent<Rigidbody>();
        if (!rb)
            rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        ib = GetComponent<Interactable>();
        if (!ib)
            ib = gameObject.AddComponent(typeof(Interactable)) as Interactable;
        ve = GetComponent<VelocityEstimator>();
        if (!ve)
            ve = gameObject.AddComponent(typeof(VelocityEstimator)) as VelocityEstimator;
        thr = GetComponent<Throwable>();
        if (!thr)
            thr = gameObject.AddComponent(typeof(Throwable)) as Throwable;
    }

    public void Strip()
    {
        //Destroy(thr);
        //Destroy(ve);
        //Destroy(ib);
        //Destroy(rb);
        //Destroy(bc);
    }
}
