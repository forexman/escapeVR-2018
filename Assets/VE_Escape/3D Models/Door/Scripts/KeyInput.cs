using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class KeyInput : MonoBehaviour
    {
        private GameObject Key { get; set; }
        private DoorController Door { get; set; }
        private Hand KeyHand { get; set; }

        // Use this for initialization
        void Start()
        {
            Transform t = transform;
            while (t.parent != null)
            {
                if (t.parent.GetComponent<DoorController>())
                {
                    Door = t.parent.GetComponent<DoorController>();
                    break;
                }
                t = t.parent.transform;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Key")
            {
                //Debug.Log("Collision with Key");
                // A key entered...
                if(collision.gameObject.GetComponent<KeyController>().Key_ID == Door.Key_ID)
                {
                    //Debug.Log("Collision with right Key");
                    //... and is the correct one
                    //Make it a regular gameObject
                    //collision.gameObject.GetComponent<KeyController>().Strip();

                    //Create Dummy key
                    GameObject dummyK = new GameObject("Entrance_Key_Dummy");
                    MeshFilter mf = dummyK.AddComponent(typeof(MeshFilter)) as MeshFilter;
                    mf.mesh = collision.gameObject.GetComponent<MeshFilter>().mesh;
                    MeshRenderer mr = dummyK.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                    mr.material = collision.gameObject.GetComponent<MeshRenderer>().material;
                    //dummyK.transform.rotation = collision.gameObject.transform.rotation;

                    //Destroy Real key
                    collision.gameObject.transform.parent.GetComponent<Hand>().DetachObject(collision.gameObject);
                    Destroy(collision.gameObject);

                    //Place it in the correct place
                    dummyK.transform.parent = this.transform;
                    dummyK.transform.parent = Door.Lock.transform;
                    dummyK.transform.localPosition = new Vector3(-0.019f, -0.108f, -0.669f);
                    dummyK.transform.localRotation = Quaternion.identity;
                    //Let the door know it is open
                    Door.UnlockDoor();
                }
            }
        }
    }
}
