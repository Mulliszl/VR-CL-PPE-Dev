using HP.Omnicept.Unity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HP.Omnicept;
using HP.Omnicept.Messaging;
using HP.Omnicept.Messaging.Messages;

namespace scripts
{
    public class sensors_display : MonoBehaviour
    {
        private GliaBehaviour _gliaBehaviour;
        protected GliaBehaviour gliaBehaviour
        {
            get
            {
                if (_gliaBehaviour == null)
                {
                    _gliaBehaviour = FindObjectOfType<GliaBehaviour>();
                }
                return _gliaBehaviour;
            }
        }
    }
}