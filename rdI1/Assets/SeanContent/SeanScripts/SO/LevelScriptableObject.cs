using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelSO", menuName = "LevelSO/LevelSO", order = 2)]
public class LevelScriptableObject : ScriptableObject
{
    public bool CanMove;
    public bool CanShoot;
    public bool CanFreeze;
    public float PlayerSpeed;
    public string NextScene;
    public string LoseScene; //this only is for if you want a custom level to send the player back to; for example, if you fail in jerry p3 you may want to go back to jerry p1, or even the jerry dialogue at p1
    public float LevelDuration;


}
