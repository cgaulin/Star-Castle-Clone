using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] radio;

    private AudioSource audioSource;
    private int index = 0;
    private bool startFirstSong, turnedOnRadio = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRadio();

        if (!audioSource.isPlaying && turnedOnRadio)
        {
            LoopThroughSongs();
        }
    }

    private void HandleRadio()
    {
        if (Input.GetKeyDown(KeyCode.R) && startFirstSong)
        {
            if (audioSource.isPlaying && index < radio.Length - 1)
            {
                LoopThroughSongs();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && !startFirstSong)
        {
            FirstSongStart();
            turnedOnRadio = true;
        }
    }

    private void PlayNextSong()
    {
        audioSource.Stop();
        index++;
        audioSource.PlayOneShot(radio[index]);
    }

    private void FirstSongStart()
    {
        audioSource.Stop();
        index = 0;
        audioSource.PlayOneShot(radio[index]);
        startFirstSong = true;
    }

    private void LoopThroughSongs()
    {
        PlayNextSong();
        if (index == radio.Length - 1)
        {
            startFirstSong = false;
        }
    }
}
