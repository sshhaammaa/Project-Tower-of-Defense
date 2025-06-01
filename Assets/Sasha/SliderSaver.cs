using UnityEngine;
using UnityEngine.UI;

public class SliderController1 : MonoBehaviour
{
    public Slider slider;
    private const string SliderKey = "SliderValue1";

    void Start()
    {
        float savedValue = PlayerPrefs.GetFloat(SliderKey, slider.value);
        slider.value = savedValue;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(SliderKey, value);
    }
}