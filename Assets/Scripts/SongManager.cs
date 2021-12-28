using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] bool playMusic = false;
    [SerializeField] AudioClip[] songs;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetNewSong();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            GetNewSong();
        }
    }

    public void GetNewSong()
    {
        if (playMusic)
        {
            audioSource.Stop();
            int randIndex = Random.Range(0, songs.Length);
            audioSource.clip = songs[randIndex];
            audioSource.Play();
        }
    }
}
