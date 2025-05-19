using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    private AudioSource stepsSource;

    [Header("Music Clips")]
    public AudioClip background;
    public AudioClip battle;
    public AudioClip epicBattle;

    [Header("Audio Clips")]
    public AudioClip death;
    public AudioClip sword;
    public AudioClip steps;
    public AudioClip jump;
    public AudioClip wallSlide;
    public AudioClip skeletonSnarl;
    public AudioClip skeletonAttack;
    public AudioClip coin;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        Debug.Log("Playing SFX: " + clip.name);
        SFXSource.Stop();
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        Debug.Log("Playing SFX: " + clip.name);
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.Play();
        Destroy(tempSource, clip.length);
    }

    public void PlayStepsLoop()
    {
        if (stepsSource == null)
        {
            stepsSource = gameObject.AddComponent<AudioSource>();
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


    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }
}
