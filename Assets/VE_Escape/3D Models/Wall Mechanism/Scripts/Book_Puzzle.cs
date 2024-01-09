using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book_Puzzle : MonoBehaviour {

    [SerializeField]
    GameObject book2, book3, book4, book6, book7;
    public AudioClip sound_book;
    public AudioSource audioSource;

    private bool isB2in, isB3in, isB4in, isB6in, isB7in;

    private bool isUnlocked;
    // Use this for initialization
    void Start()
    {
        audioSource.clip = sound_book;
        isB2in = false;
        isB3in = false;
        isB4in = false;
        isB6in = false;
        isB7in = false;
        isUnlocked = false;
    }

    public bool puzzleDone()
    {
        return isUnlocked;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name + " Book Entered " + gameObject.name + " spot");
        if (other.gameObject == book2)
        {
            isB2in = true;
            Destroy(book2);
            book2 = null;
            //book2.gameObject.SetActive(false);
            transform.Find("Tome 2").gameObject.SetActive(true);
            audioSource.Play();
        }

        if (other.gameObject == book3)
        {
            isB3in = true;
            Destroy(book3);
            book3 = null;
            transform.Find("Tome 3").gameObject.SetActive(true);
            audioSource.Play();
        }

        if (other.gameObject == book4)
        {
            isB4in = true;
            Destroy(book4);
            book4 = null;
            transform.Find("Tome 4").gameObject.SetActive(true);
            audioSource.Play();
        }

        if (other.gameObject == book6)
        {
            isB6in = true;
            Destroy(book6);
            book6 = null;
            transform.Find("Tome 6").gameObject.SetActive(true);
            audioSource.Play();
        }

        if (other.gameObject == book7)
        {
            isB7in = true;
            Destroy(book7);
            book7 = null;
            transform.Find("Tome 7").gameObject.SetActive(true);
            audioSource.Play();
        }

        if (isB2in && isB3in && isB4in && isB6in && isB7in && !isUnlocked) isUnlocked = true;
    }
}
