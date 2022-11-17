using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

// deactivate camera and audiolisteners of others players
// used in multiplater scenes in order to keep the render stable

public class Deactivation : NetworkBehaviour
{
    private AudioListener myAL;
    private Camera myC;
    private TrackedPoseDriver myTPD;
    // Start is called before the first frame update
    public void Start()
    {
        if (IsLocalPlayer)
        {

        }
        else
        {
            myAL = GetComponent<AudioListener>();
            myC = GetComponent<Camera>();
            myTPD = GetComponent<TrackedPoseDriver>();
            if (myAL.enabled == true)
            {
                myAL.enabled = false;
            }if(myC.enabled == true)
            {
                myC.enabled = false;
            }if(myTPD.enabled == true)
            {
                myTPD.enabled = false;
            } 
        }
    }

}
