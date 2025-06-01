using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource musicSource;
    private const string VolumeKey = "MusicVolume";

    void Start()
    {
        // ����������� ��������� ������� ��� ���������� �������� �� �������������
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        volumeSlider.value = savedVolume;
        musicSource.volume = savedVolume;

        // ������ ������� �� ���� �������� ��������
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }
}