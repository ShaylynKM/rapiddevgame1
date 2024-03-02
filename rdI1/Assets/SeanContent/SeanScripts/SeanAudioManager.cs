using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeanAudioManager : Singleton<SeanAudioManager>
{

    public float volume = 1f;

    [SerializeField]
    AudioPairsScriptableObject audioFiles; //use serialize field rather than public if you can avoid it

    Dictionary<AudioNames, AudioSource> audioSources = new Dictionary<AudioNames, AudioSource>();

    void Awake()
    {

        foreach (AudioPairs entry in audioFiles.audioPairs)
        {
            AudioSource source = Instantiate(entry.Value); //a prefab has to be instantiated in the scene
            audioSources.Add(entry.Key, source);

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateVolume(volume);

    }

    public void UpdateVolume(float volume)
    {
        this.volume = volume;
        foreach (KeyValuePair<AudioNames, AudioSource> entry in audioSources)
        {
            entry.Value.volume = volume;

            //audioSources.Add(entry.Key, Resources.Load<AudioSource>(entry.Value));
        }

    }

    public void Play(AudioNames name, bool isLoop = false)
    {
        if (audioSources.ContainsKey(name))
        {
            audioSources[name].loop = isLoop;
            audioSources[name].Play();
        }
    }

    AudioClip GetAudioClip(AudioNames name)
    {
        return audioSources[name].clip;
    }

    AudioClip GetAudioClip(string name)
    {
        //switch (name)
        //{
        //    case "bg":
        //        return bg;
        //    case "PlayerShoot":
        //        return playerShoot;
        //    case "bossFight":
        //        return bossFight;
        //    case "bossKill":
        //        return bossKill;
        //    case "playerKill":
        //        return playerKill;
        //    case "heal":
        //        return heal;
        //    case "hurt":
        //        return hurt;
        //    default:
        //        return null;
        //}
        return null;
    }
    public void Stop(AudioNames name)
    {
        audioSources[name]?.Stop();
    }
    public void Stop(int index)
    {
        //if (index >= 0 && index < audios.Count)
        //{
        //    audios[index].Stop();
        //}
    }
}

