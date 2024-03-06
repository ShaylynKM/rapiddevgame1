using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePhase", menuName = "GameFlow/GamePhase")]
public class GamePhaseSO : ScriptableObject
{
    public float phaseDuration;
    public bool canMove;
    public bool canShoot;
    public bool canFreeze;
    public AudioClip musicClip;
    public GameObject anxietyMeterPrefab;
    public GamePhaseSO nextPhase;
    

}

