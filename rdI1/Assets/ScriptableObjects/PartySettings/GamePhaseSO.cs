
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePhase", menuName = "GameFlow/GamePhase")]
public class GamePhaseSO : ScriptableObject
{
    public float phaseDuration;
    public bool canMove;
    public bool canShoot;
    public bool canFreeze;
    public GameObject anxietyMeterPrefab;
    public GamePhaseSO nextPhase;
    public DialogueSceneScriptableObject dialogueScene;

}

