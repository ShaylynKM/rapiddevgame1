using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float volume = 1f;
   
    public AudioClip bg;
    public AudioClip playerShoot;
    //public AudioClip bossFight;
    public AudioClip bossKill;
    public AudioClip playerKill;
    public AudioClip heal;
    public AudioClip hurt;


    List<AudioSource> audios = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audios.AddRange(GetComponents<AudioSource>());
            audios.AddRange(GetComponentsInChildren<AudioSource>());

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateVolume(volume);

        Play(0, "bossFight", true);

        for (int i = 0; i < 8; i++)
        {
            var audio = gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }


    public void UpdateVolume(float volume)
    {
        this.volume = volume;
        foreach (var audioSource in audios) 
        {
            audioSource.volume = volume;
        }
    }

    public AudioSource AddAudioSource(AudioClip clip, bool loop = false)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = clip;
        newAudioSource.loop = loop;
        audios.Add(newAudioSource);
        return newAudioSource;
    }

    public void RemoveAudioSource(AudioSource audioSource)
    {
        if (audios.Contains(audioSource))
        {
            audios.Remove(audioSource);
            Destroy(audioSource);
        }
    }


    public void Play(int index, string name, bool isLoop)
    {
        if (index < 0 || index >= audios.Count)
            return;

        var clip = GetAudioClip(name);
        if (clip != null)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }

    

    AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "bg":
                return bg;
            case "PlayerShoot":
                return playerShoot;
            
            case "bossKill":
                return bossKill;
            case "playerKill":
                return playerKill;
            case "heal":
                return heal;
            case "hurt":
                return hurt;
            default:
                return null;
        }
    }

    public void Stop(int index)
    {
        if (index >= 0 && index < audios.Count)
        {
            audios[index].Stop();
        }
    }

}
