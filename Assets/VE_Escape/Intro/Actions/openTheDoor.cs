using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class openTheDoor : AbstractAction
    {
        public GameObject door, TPEntrance, TPHallway, EntranceBlock, HallBlock;

        public override bool check(Environment e)
        {
            if (!door.GetComponent<DoorController>().DoorLocked)
            {
                EntranceBlock.SetActive(false);
            }

            RaycastHit hit;
            PlayerTransformations pt = e.getPlayerPositions();

            if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, 1.5f))
            {
                if (hit.collider.gameObject == HallBlock)
                {
                    HallBlock.SetActive(false);
                    return true;
                }
            }

            return false;
        }

        // Use this for initialization
        void Start() {
            m_actionName = "Open The Door";
        }

        // Update is called once per frame
        void Update() {

        }
    }
}