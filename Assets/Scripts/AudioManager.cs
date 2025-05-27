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

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        switch (currentSceneName)
        {
            case "MainMenu":
            case "MenuControllers":
            case "Settings":
                musicSource.clip = backgroundMenu;
                break;
            case "FirstScene":
                musicSource.clip = backgroundLevel1;
                break;
            case "Level2":
                musicSource.clip = backgroundLevel2;
                break;
            default:
                musicSource.clip = backgroundLevel1;
                break;
        }

        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        //Debug.Log("Playing SFX: " + clip.name);
        SFXSource.Stop();
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        // Crear un nuevo AudioSource temporal
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.outputAudioMixerGroup = SFXSource.outputAudioMixerGroup; // Asignar el mismo AudioMixerGroup
        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.Play();

        // Destruir el AudioSource después de que termine el clip
        Destroy(tempSource, clip.length);
    }

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

    public void StopStepsLoop()
    {
        if (stepsSource != null && stepsSource.isPlaying)
            stepsSource.Stop();
    }

    public void PlayMusic(AudioClip clip)
    {
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

    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }
}
