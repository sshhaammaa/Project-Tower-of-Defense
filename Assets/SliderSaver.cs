using UnityEngine;
using UnityEngine.UI;

public class SliderSaver : MonoBehaviour
{
    public Slider slider; 
    private const string SaveKey = "SliderValue";

    void Start()
    {
       
        if (PlayerPrefs.HasKey(SaveKey))
        {
            float savedValue = PlayerPrefs.GetFloat(SaveKey);
            slider.value = savedValue;
        }

        
        slider.onValueChanged.AddListener(SaveSliderValue);
    }

    
    private void SaveSliderValue(float value)
    {
        PlayerPrefs.SetFloat(SaveKey, value);
        PlayerPrefs.Save(); 
    }
}