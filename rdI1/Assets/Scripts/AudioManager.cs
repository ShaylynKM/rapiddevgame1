using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip bg;
    public AudioClip playerShoot;
    public AudioClip bossFight;
    public AudioClip destroy;

    List<AudioSource> audios = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Play(0, "bossFight", true);

        for (int i = 0; i < 3; i++)
        {
            var audio = gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
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
            case "bossFight":
                return bossFight;
            case "Destroy":
                return destroy;
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
