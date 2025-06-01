using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip; // ������� �������� � ���������
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.loop = true; // �� ��������
            audioSource.playOnAwake = false;
            audioSource.Play();
        }
    }
}