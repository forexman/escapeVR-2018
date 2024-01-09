using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class Wall_Mehanism : MonoBehaviour
    {
        [SerializeField]
        GameObject puzzle, shelf, safe, doorL, doorR;
        public AudioClip shelf_sound, walls_sound, safe_sound;
        public AudioSource shelf_audio, doors_audio, safe_audio;
        public float lerpTime = 6.0f;

        private bool safeRevealed, shelfMoved, doorsMoved;
        private Vector3 shelf_pos, doorL_pos, doorR_pos, safe_pos;
        private float currentLerp;
       
        // Use this for initialization
        void Start()
        {
            currentLerp = 0.0f;
            safeRevealed = false;
            shelfMoved = false;
            doorsMoved = false;
            shelf_pos = shelf.transform.position;
            doorL_pos = doorL.transform.position;
            doorR_pos = doorR.transform.position;
            safe_pos = safe.transform.position;
            shelf_audio.clip = shelf_sound;
            doors_audio.clip = walls_sound;
            safe_audio.clip = safe_sound;

        }

        // Update is called once per frame
        void Update()
        {
            if (puzzle.GetComponent<Book_Puzzle>().puzzleDone())
            {
                //Debug.Log("Unlocking Safe");
                if (!shelfMoved)
                {
                    if (!shelf_audio.isPlaying)
                    {
                        //Debug.Log("shelf_audio.isPlaying");
                        shelf_audio.Play();
                    }
                    currentLerp += Time.deltaTime;
                    if (currentLerp >= lerpTime) currentLerp = lerpTime;
                    shelf.transform.position = Vector3.Lerp(shelf_pos, shelf_pos + Vector3.forward, currentLerp / lerpTime);
                    if (shelf.transform.position == shelf_pos + Vector3.forward)
                    {
                        shelfMoved = true;
                        currentLerp = 0.0f;
                        shelf_audio.Stop();
                    }
                }
                if (!doorsMoved && shelfMoved)
                {
                    if (!doors_audio.isPlaying)
                    {
                        //Debug.Log("doors audio.isPlaying");
                        doors_audio.Play();
                    }
                    currentLerp += Time.deltaTime;
                    if (currentLerp >= lerpTime) currentLerp = lerpTime;
                    // Debug.Log("Moving Doors from " + doorL_pos + " to " + doorL_pos + Vector3.forward);
                    doorL.transform.position = Vector3.Lerp(doorL_pos, doorL_pos + Vector3.forward /2.5f, currentLerp / lerpTime);
                    doorR.transform.position = Vector3.Lerp(doorR_pos, doorR_pos + Vector3.back /2.5f, currentLerp / lerpTime);
                    if (doorL.transform.position == doorL_pos + Vector3.forward/2.5f)
                    {
                        doorsMoved = true;
                        currentLerp = 0.0f;
                        doors_audio.Stop();
                    }
                }
                if (doorsMoved && shelfMoved && !safeRevealed)
                {
                    if (!safe_audio.isPlaying)
                    {
                        //Debug.Log("safe audio.isPlaying");
                        safe_audio.Play();
                    }
                    currentLerp += Time.deltaTime;
                    if (currentLerp >= lerpTime) currentLerp = lerpTime;
                    //Debug.Log("Moving Doors from " + safe_pos + " to " + (safe_pos + new Vector3(-0.32f, 0.0f, 0.0f)));
                    safe.transform.position = Vector3.MoveTowards(safe_pos, safe_pos + new Vector3(-0.32f,0.0f,0.0f), currentLerp / lerpTime);
                    if (safe.transform.position == safe_pos + new Vector3(-0.32f, 0.0f, 0.0f))
                    {
                        safe_audio.Stop();
                        safeRevealed = true;
                    }
                }
            }
        }

        public bool IsSafeRevealed()
        {
            return safeRevealed;
        }
    }
}
