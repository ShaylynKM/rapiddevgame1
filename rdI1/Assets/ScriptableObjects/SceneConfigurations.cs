using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneConfigurations", menuName = "Configuration/SceneConfigurations")]
public class SceneConfigurations : ScriptableObject
{
    public GamePhaseSO homeConfig;
    public GamePhaseSO schoolConfig;
    public GamePhaseSO jobConfig;
    public GamePhaseSO partyConfig;
 
}
