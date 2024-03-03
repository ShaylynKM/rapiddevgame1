using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartySettings", menuName = "ScriptableObjects/Party Stages", order = 2)]
public class PartyStages : ScriptableObject
{
    public bool CanMove;
    public bool CanShoot;
    public bool CanFreeze;
    public float PlayerSpeed;
    public float Duration;
}
