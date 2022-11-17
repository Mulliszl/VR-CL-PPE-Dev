using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//activate vr on scene

public class VrActivator : MonoBehaviour
{
    void Start()
    {
        UnityEngine.XR.XRSettings.enabled = true;
    }
}
