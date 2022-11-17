using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// used to start GlobalConfig and load the choosed scene
// only used in the menu
public class initializer : MonoBehaviour
{
    void Start()
    {
        GlobalConfig.initialize();
    }
}
