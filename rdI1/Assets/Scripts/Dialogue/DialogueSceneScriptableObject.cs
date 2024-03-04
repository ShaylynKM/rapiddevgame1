using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScene", menuName = "ScriptableObjects/DialogueScene", order = 1)]
public class DialogueSceneScriptableObject : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
    
}
