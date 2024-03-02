using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //in our level manager, we will define the things that all levels have: say, a beginning, completion, what to do on update, etc. We then make a lot of functions virtual so they can be overriden by the child classes (HomeLevel, JobLevel, JerryLevel, etc)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
