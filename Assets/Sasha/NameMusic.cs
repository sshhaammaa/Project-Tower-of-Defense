using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip; // Присвой аудіофайл у інспекторі
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true; // За бажанням
            audioSource.playOnAwake = false;
            audioSource.Play();
        }
    }
}