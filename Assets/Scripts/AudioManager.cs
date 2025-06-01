using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    private AudioSource stepsSource;

    [Header("Music Clips")]
    public AudioClip backgroundLevel1;
    public AudioClip backgroundLevel2;
    public AudioClip backgroundMenu;
    public AudioClip battle;
    public AudioClip epicBattle;
    public AudioClip endGame;
    public AudioClip startGame;

    [Header("Audio Clips")]
    public AudioClip death;
    public AudioClip sword;
    public AudioClip sword2;
    public AudioClip steps;
    public AudioClip jump;
    public AudioClip wallSlide;
    public AudioClip skeletonSnarl;
    public AudioClip skeletonAttack;
    public AudioClip coin;
    public AudioClip waterDrops;
    public AudioClip monsterDeath;
    public AudioClip axe;
    public AudioClip thunder;
    public AudioClip magicDrop;
    public AudioClip magicExplode;

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        switch (currentSceneName)
        {
            case "MainMenu":    
            case "MenuControllers":
            case "Settings":
                musicSource.clip = startGame;
                break;
            case "FirstScene":
                musicSource.clip = backgroundLevel1;
                break;
            case "Level2":
                musicSource.clip = backgroundLevel2;
                break;
            case "Endgame":
                musicSource.clip = endGame;
                break;
            default:
                musicSource.clip = backgroundLevel1;
                break;
        }

        musicSource.Play();
    }

    /// <summary>
    /// Plays a sound effect using the specified audio clip.
    /// </summary>
    /// <param name="clip">The audio clip to be played. Cannot be null.</param>
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.Stop();
        SFXSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays a sound effect using a temporary audio source.
    /// </summary>
    /// <remarks>This method creates a temporary <see cref="AudioSource"/> to play the specified audio clip.
    /// The temporary audio source is automatically destroyed after the clip finishes playing.</remarks>
    /// <param name="clip">The audio clip to be played. Cannot be null.</param>
    /// <param name="volume">The volume at which the audio clip will be played. Must be between 0.0 and 1.0. Defaults to 1.0.</param>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.outputAudioMixerGroup = SFXSource.outputAudioMixerGroup;
        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.Play();

        Destroy(tempSource, clip.length);
    }

    /// <summary>
    /// Plays a looping audio clip of steps if it is not already playing.
    /// </summary>
    /// <remarks>This method ensures that an <see cref="AudioSource"/> is attached to the game object and
    /// configured  to play the steps audio clip in a loop. If the audio source is already playing, the method does
    /// nothing.</remarks>
    public void PlayStepsLoop()
    {
        if (stepsSource == null)
        {
            stepsSource = gameObject.AddComponent<AudioSource>();
            stepsSource.outputAudioMixerGroup = SFXSource.outputAudioMixerGroup;
            stepsSource.clip = steps;
            stepsSource.loop = true;
            stepsSource.volume = 1f;
        }
        if (!stepsSource.isPlaying)
            stepsSource.Play();
    }

    /// <summary>
    /// Stops the playback of the steps sound loop if it is currently playing.
    /// </summary>
    /// <remarks>This method checks if the steps sound source is not null and is currently playing before
    /// stopping it.</remarks>
    public void StopStepsLoop()
    {
        if (stepsSource != null && stepsSource.isPlaying)
            stepsSource.Stop();
    }

    /// <summary>
    /// Plays the specified audio clip using the music source.
    /// </summary>
    /// <param name="clip">The audio clip to be played. Cannot be null.</param>
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying)
            return; // If the same clip is already playing, do nothing

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            musicSource.Pause();
            if (stepsSource != null && stepsSource.isPlaying)
                stepsSource.Pause();
        }
        else
        {
            musicSource.UnPause();
            if (stepsSource != null && !stepsSource.isPlaying)
                stepsSource.UnPause();
        }
    }

    /// <summary>
    /// Stops the currently playing music.
    /// </summary>
    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }
}
