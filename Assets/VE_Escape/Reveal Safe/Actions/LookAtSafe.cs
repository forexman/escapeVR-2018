using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class LookAtSafe : AbstractAction
    {

        public GameObject safe, wall_mechanism;
        public AudioClip m_voiceLine;

        public float m_maxDistrance;

        private bool m_cuePlayed = false;

        public override bool check(Environment e)
        {
            if (!m_cuePlayed && e.getCurrentTime() > m_activatedTime + m_cueThreshold)
            {
                m_cuePlayed = true;
                e.getAudioController().playSound(m_audioCues[0]);
            }

            if ((wall_mechanism.GetComponent("Wall_Mehanism") as Wall_Mehanism).IsSafeRevealed())
            {

                // Check if the player looks at the safe. If yes, play the voiceline and move on.
                RaycastHit hit;
                PlayerTransformations pt = e.getPlayerPositions();

                if (Physics.Raycast(pt.HmdPosition, pt.HmdForwardVector, out hit, m_maxDistrance))
                {
                    if (hit.collider.gameObject == safe)
                    {
                        e.getAudioController().playSound(m_voiceLine);
                        return true;
                    }
                }
            }

            return false;
        }

        // Use this for initialization
        void Start()
        {
            m_actionName = "Looking for the Safe";
            m_cueThreshold = 120;
        }
    }
}