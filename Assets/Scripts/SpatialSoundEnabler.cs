using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HP.VR.SpatialAudio;

// used for initialysing hp spatial audio

public class SpatialSoundEnabler : MonoBehaviour
{
    void Start()
    {
        HPVRSpatialAudioEnabler.EnableSpatialAudio();
    }
}
