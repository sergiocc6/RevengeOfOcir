using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the AudioMixer
    public Slider musicSlider; // Reference to the UI Slider for music volume
    public Slider SFXSlider; // Reference to the UI Slider for SFX volume

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume")) // Check if the volume setting exists
        {
            LoadVolume(); // Load the saved volume setting
        }
        else
        {
            SetMusicVolume(); // Set the initial volume when the game starts
            SetSFXVolume(); // Set the initial SFX volume when the game starts
        }

        
    }
    /// <summary>
    /// Function to set the music volume based on the slider value.
    /// </summary>
    public void SetMusicVolume()
    {
        float volume = musicSlider.value; // Get the value from the slider
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20); // Set the volume in the AudioMixer
    }

    /// <summary>
    /// Function to set the SFX volume based on the slider value.
    /// </summary>
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value; // Get the value from the slider
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20); // Set the volume in the AudioMixer
    }

    /// <summary>
    /// Load saved volumes settings from PlayerPrefs.
    /// </summary>
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume"); // Load the saved music volume setting
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume"); // Load the saved SFX volume setting
        SetMusicVolume();
        SetSFXVolume();
    }

    /// <summary>
    /// Saves the current music and sound effects (SFX) volume settings to persistent storage.
    /// </summary>
    /// <remarks>The method retrieves the current values from the associated sliders and stores them using 
    /// <see cref="PlayerPrefs"/> for later retrieval. The saved settings can be used to restore  volume preferences
    /// across application sessions.</remarks>
    public void AcceptVolumeSettings()
    {
        float musicVolume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", musicVolume); // Save the music volume setting
        float SFXVolume = musicSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume); // Save the SFX volume setting
    }
}
