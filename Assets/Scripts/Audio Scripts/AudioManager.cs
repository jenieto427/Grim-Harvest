using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] songs; // Assign this in the Inspector
    private AudioSource audioSource;
    private int[] intervals = { 300 }; // Time intervals in seconds (10 min, 20 min, 30 min)

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayMusicRandomly());
    }

    private IEnumerator PlayMusicRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetRandomTime());
            PlayRandomSong();
        }
    }

    private void PlayRandomSong()
    {
        if (songs.Length == 0) return;

        audioSource.clip = songs[Random.Range(0, songs.Length)];
        audioSource.Play();
    }

    private float GetRandomTime()
    {
        // Select a random interval from the array
        int index = Random.Range(0, intervals.Length);
        return intervals[index];
    }
}
