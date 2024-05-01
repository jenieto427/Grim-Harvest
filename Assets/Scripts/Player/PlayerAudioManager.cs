using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource walkingSoundSource; // Assign an AudioSource that has the walking sound clip

    public void PlayWalkingSound()
    {
        if (!walkingSoundSource.isPlaying)
        {
            walkingSoundSource.Play();
        }
    }
    public void StopWalkingSound()
    {
        if (walkingSoundSource.isPlaying)
        {
            walkingSoundSource.Stop();
        }
    }
}
