using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Valve.VR.InteractionSystem
{
    public class Urn_Transform : MonoBehaviour
    {
        public GameObject urn, body, lid, mist, levelController, LeftController, RightController;
        public AudioSource audioSource;

        public float speed = 200f;

        public bool rotate = false;
        public bool lift = false;
        public bool lifted = false;
        public bool liftlid = false;
        public bool liftedlid = false;
        public bool activate2 = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float leftHandDistance = (LeftController.transform.position - urn.transform.position).magnitude;
            float rightHandDistance = (RightController.transform.position - urn.transform.position).magnitude;

            // trigger if distance is less than 5cm
            if (leftHandDistance < 0.35f || rightHandDistance < 0.35f)
            {
                lift = true;
            }

            if (lift && !lifted)
            {
                LiftUrn();
            }
            if (liftlid && !liftedlid)
            {
                LiftLid();
            }
            if (rotate)
            {
                RotateUrn();
            }
            //SceneManager.LoadScene("Shadow World Low Res", LoadSceneMode.Additive);
        }

        IEnumerator Example()
        {
            yield return new WaitForSeconds(15);
            SceneManager.LoadScene("Shadow World Low Res Final", LoadSceneMode.Single);
            //StartCoroutine(LoadYourAsyncScene());
        }

        IEnumerator LoadYourAsyncScene()
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Shadow World Low Res Final");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        void LiftUrn()
        {
            rotate = true;
            urn.transform.Translate(Vector3.forward * Time.deltaTime);
            urn.transform.Translate(Vector3.up * 3f * Time.deltaTime);
            if (urn.transform.position.y > 1.7f)
            {
                lifted = true;
                liftlid = true;
                body.transform.GetChild(0).gameObject.SetActive(true);
                mist.SetActive(true);
                StartCoroutine(Example());
                //levelController.GetComponent<SteamVR_LoadLevel>().enabled = true;
            }
        }

        void LiftLid()
        {
            lid.transform.Translate(Vector3.forward * Time.deltaTime);
            if (lid.transform.position.y > 2.7f)
            {
                liftedlid = true;
                Destroy(lid);
                lid = null;
            }
        }

        void RotateUrn()
        {
            urn.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}