using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    float sensivityValue = 1000f;
    float musicVolume = 0.5f;
    float sfxVolume = 0.5f;

    [SerializeField] Slider sensivitySlider, musicSlider, sfxSlider;


    void Awake()
    {
        if (PlayerPrefs.HasKey("MouseSensivity"))
        {
            sensivitySlider.value = PlayerPrefs.GetFloat("MouseSensivity");
        }
        else
        {
            sensivitySlider.value = sensivityValue;
            PlayerPrefs.SetFloat("MouseSensivity", sensivityValue);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicSlider.value = musicVolume;
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            sfxSlider.value = sfxVolume;
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        }
    }

    void Update()
    {
        MouseSensivityChanger();
        MusicVolumeChanger();
        SFXVolumeChanger();
    }

    void MouseSensivityChanger()
    {
        sensivityValue = sensivitySlider.value;
        FindAnyObjectByType<MouseInput>().mouseSensivity = sensivityValue;
        PlayerPrefs.SetFloat("MouseSensivity", sensivityValue);
    }

    void MusicVolumeChanger()
    {
        musicVolume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    void SFXVolumeChanger()
    {
        sfxVolume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
}
