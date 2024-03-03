
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePhase", menuName = "GameFlow/GamePhase")]
public class GamePhaseSO : ScriptableObject
{
    public string phaseName;
    public DialogueSceneScriptableObject dialogue;
    public bool canMove;
    public bool canShoot;
    public bool canFreeze;

}

