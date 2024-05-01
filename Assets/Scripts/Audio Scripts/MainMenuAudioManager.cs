using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Attach your AudioSource component here
    public List<AudioClip> playlist = new List<AudioClip>(); // Populate with your audio clips in the Unity Editor

    private List<AudioClip> songsToPlay;
    private AudioClip currentSong;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        songsToPlay = new List<AudioClip>(playlist);
        PlayNextSong();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    void PlayNextSong()
    {
        if (songsToPlay.Count == 0)
        {
            songsToPlay = new List<AudioClip>(playlist); // Reset the playlist when all songs have been played
        }

        // Remove the currently playing song from the pool (if any)
        if (currentSong != null)
        {
            songsToPlay.Remove(currentSong);
        }

        // Randomly pick the next song to play
        int index = Random.Range(0, songsToPlay.Count);
        currentSong = songsToPlay[index];
        audioSource.clip = currentSong;
        audioSource.Play();

        // Add back the previous song into the pool for future rounds
        if (currentSong != null && playlist.Count > 1)
        {
            songsToPlay.Add(currentSong);
        }
    }
}
