using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Slider volumeSlider; 

    void Start()
    {
        
        volumeSlider.value = AudioManager.Instance.volume;
       
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

  
    public void ValueChangeCheck()
    {
        AudioManager.Instance.UpdateVolume(volumeSlider.value);
    }
}