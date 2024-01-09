using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class attatchTo : MonoBehaviour
    {
        public Transform target;

        private void Start()
        {
            if(target) this.transform.parent = target.transform;
        }

        private void LateUpdate()
        {
            //transform.position = target.position;
        }
    }
}
