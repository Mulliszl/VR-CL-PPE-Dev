using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HP.Omnicept.Messaging.Messages;
using UnityEngine.UI;
using TMPro;


namespace scripts
{
    public class capteur_charge_cognitive : sensors_display
    {
        public TextMeshProUGUI TexteChargeCognitive;
        private Material cogLoadMaterial;
        public Image cognitiveLoad;
        public Image noCLDataWarning;
        public bool showRawValue = false;
        private float targetCLConfidence;
        private float currentCL = 0f;
        private float targetCL = 0;

        void Start()
        {
            gliaBehaviour.OnCognitiveLoad.AddListener(OnCognitiveLoad);
            cogLoadMaterial = cognitiveLoad.material;
        }

        private void OnCognitiveLoad(CognitiveLoad cl)
        {
            if (cl != null)
            {
                targetCL = cl.CognitiveLoadValue;
                targetCLConfidence = cl.StandardDeviation;

                if (!showRawValue)
                {
                    if (cl.CognitiveLoadValue > 0.66)
                    {
                        TexteChargeCognitive.text = "High";
                    }
                    else if (cl.CognitiveLoadValue > 0.33)
                    {
                        TexteChargeCognitive.text = "Med";
                    }
                    else if (cl.CognitiveLoadValue > 0)
                    {
                        TexteChargeCognitive.text = "Low";
                    }
                    else
                    {
                        TexteChargeCognitive.text = "N/A";
                    }
                }
                else
                {
                    TexteChargeCognitive.text = targetCL.ToString("0.00");
                }
            }

            noCLDataWarning.gameObject.SetActive(cl == null);
        }

        void Update()
        {
            currentCL = Mathf.Lerp(currentCL, targetCL, 15f * Time.deltaTime);
            cogLoadMaterial.SetFloat("_FillAmmount", currentCL);

            var cl = gliaBehaviour.GetLastCognitiveLoad();
        }
    }
}