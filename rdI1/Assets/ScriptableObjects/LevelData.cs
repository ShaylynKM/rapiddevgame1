using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public bool CanMove;
    public bool CanShoot;
    public bool CanFreeze;
    public float PlayerSpeed;
    public string NextScene;
    public string LoseScene;
    public float LevelDuration;
    public float stageDurations;


}