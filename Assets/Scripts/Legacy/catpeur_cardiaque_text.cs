using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HP.Omnicept;
using HP.Omnicept.Messaging;
using HP.Omnicept.Messaging.Messages;

namespace scripts
{ 
    public class catpeur_cardiaque_text : sensors_display
    {
    [SerializeField] private TextMeshProUGUI TexteCapteurCardiaque = null;
    void Start()
    {
        gliaBehaviour.OnHeartRate.AddListener(OnHeartRate);
    }


    private void OnHeartRate(HeartRate hr)
    {
        Debug.Log(Time.time);
        if (hr != null)
        {
                TexteCapteurCardiaque.text = hr.Rate.ToString();
        }
    }
    }

}
