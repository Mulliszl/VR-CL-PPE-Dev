using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//deactivate vr on scene
public class VrDisabler : MonoBehaviour
{
    void Start()
    {
        UnityEngine.XR.XRSettings.enabled = false;
    }
}
