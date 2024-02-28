using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float volume = 1f;
    public Slider volumeSlider;

    public AudioClip bg;
    public AudioClip playerShoot;
    public AudioClip bossFight;
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
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider = FindObjectOfType<Slider>();
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
            volumeSlider.onValueChanged.RemoveAllListeners(); 
            volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }

        UpdateVolume(volume);

        Play(0, "bossFight", true);

        for (int i = 0; i < 8; i++)
        {
            var audio = gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }

        volumeSlider.value = AudioManager.Instance.volume;

        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void UpdateVolume(float volume)
    {
        this.volume = volume;
        foreach (var audio in audios)
        {
            audio.volume = volume;
        }
    }

    public void ValueChangeCheck()
    {
        AudioManager.Instance.UpdateVolume(volumeSlider.value);
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
