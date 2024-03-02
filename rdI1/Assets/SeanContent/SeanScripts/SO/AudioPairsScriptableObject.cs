using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioPairsSO", menuName = "ScriptableObjects/AudioPairs", order = 2)]
public class AudioPairsScriptableObject : ScriptableObject
{
    public List<AudioPairs> audioPairs = new List<AudioPairs>();
    
}
[System.Serializable]
public struct AudioPairs
{
    public AudioNames Key;
    public AudioSource Value;
}
[System.Serializable]
public enum AudioNames
{
    BackgroundMusic, PlayerShoot, BossFight, BossKill, PlayerKill, Heal, DropKickAnElephant, PunchJerry
}
