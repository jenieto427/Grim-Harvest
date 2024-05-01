using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance

    public AudioClip[] songs; // Assign this in the Inspector
    private AudioSource audioSource;
    private int[] intervals = { 300 }; // Time intervals in seconds

    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
            // Makes the object not be destroyed automatically when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // If instance already exists and it's not this, then destroy this to enforce singleton pattern
            Destroy(gameObject);
        }
    }

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
