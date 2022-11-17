using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HP.Omnicept;
using HP.Omnicept.Messaging;
using HP.Omnicept.Messaging.Messages;
using TMPro;

// this script change the color of a gameobject depending on the user's heart rate
//it 's just a test to understand the glia ans is only used in the bowling scene

namespace scripts
{
    public class cube_color : sensors_display
    {
        [SerializeField] private GameObject alley = null;
        void Start()
        {
            var alleyRenderer = alley.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
            gliaBehaviour.OnHeartRate.AddListener(OnHeartRate);

        }
        private void OnHeartRate(HeartRate hr)
        {
            //Debug.Log(Time.time);
            if (hr != null)
            {
                uint nb_battements = hr.Rate;
                System.Diagnostics.Debug.WriteLine(hr.Rate.ToString());
                if (nb_battements < 60 ){
                    var alleyRenderer = alley.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
                } else if (nb_battements < 100) {
                    var alleyRenderer = alley.GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
                } else {
                    var alleyRenderer = alley.GetComponent<Renderer>().material.color = UnityEngine.Color.red;
                }
            }
        }
    }

}