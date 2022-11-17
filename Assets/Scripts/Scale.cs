using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Scale : MonoBehaviour
{
    
    
    public GameObject CanvasScale;
    public Text MentalDemandValue;
    public Text PerformanceValue;
    public Text EffortValue;
    public Text FrustrationValue;
    public Text PhysicalDemandValue;
    public Text TemporalDemandValue;
    public GameObject calculLocation;
    private Calculomatic _Calculomatic;
    private MainTripleTask _MainTripleTask;
    public SensorsData _SensorsData;

    //Path to create the txt file
    private string path;
    public bool answered { get; set; }

    
    void Start()
    {
        
        //Create the txt file name to save the scale answer
        string now = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        path = Directory.GetCurrentDirectory() + @"\SensorsCSVData\"+GlobalConfig.name+@"\" + GlobalConfig.type+ '-' + now + ".txt";
        
        // Get the location of the scripts according to the scene
        if (GlobalConfig.type == "calcul")
            _Calculomatic = calculLocation.GetComponent<Calculomatic>();
        else if (GlobalConfig.type == "doubleTask")
            _MainTripleTask = calculLocation.GetComponent<MainTripleTask>();
    }       
    
    //When the button confirm is pressed
    public void OnConfirm()
    {


        if (!File.Exists(path))
        {

            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("Pause Rate: ");
                sw.WriteLine("  Mental Demmand: " + MentalDemandValue.text);
                sw.WriteLine("  Performance: " + PerformanceValue.text);
                sw.WriteLine("  Effort: " + EffortValue.text);
                sw.WriteLine("  Frustration: " + FrustrationValue.text);
                sw.WriteLine("  Physical Demand: " + PhysicalDemandValue.text);
                sw.WriteLine("  Temporal Demand: " + TemporalDemandValue.text);


            }
        }

        else
        {
            // Get average of cognitive load
            float moySimple = moylist(_SensorsData.simpleTask);
            float moyComplex = moylist(_SensorsData.complexTask);

            // Write output file
            using (StreamWriter sw = File.AppendText(path))
            {

                sw.WriteLine("End Rate: ");
                sw.WriteLine("  Mental Demmand: " + MentalDemandValue.text);
                sw.WriteLine("  Performance: " + PerformanceValue.text);
                sw.WriteLine("  Effort: " + EffortValue.text);
                sw.WriteLine("  Frustration: " + FrustrationValue.text);
                sw.WriteLine("  Physical Demand: " + PhysicalDemandValue.text);
                sw.WriteLine("  Temporal Demand: " + TemporalDemandValue.text);
                sw.WriteLine("Response Time: ");
                if (GlobalConfig.type == "calcul")
                {
                    sw.WriteLine("  Average of simple calculations: " + _Calculomatic.moySimple.ToString());
                    sw.WriteLine("  Average of complex calculations: " + _Calculomatic.moyComplex.ToString());
                }
                else if (GlobalConfig.type == "doubleTask")
                {
                    sw.WriteLine("  Average of simple task: " + _MainTripleTask.moySimple.ToString());
                    sw.WriteLine("  Average of double task: " + _MainTripleTask.moyComplex.ToString());
                }
                sw.WriteLine("Cognitive Load: ");
                sw.WriteLine("  Average of first half cognitive load: " + moySimple);
                sw.WriteLine("  Average of second half cognitive load: " + moyComplex);

            }
        }

        answered = true;
        CanvasScale.SetActive(false);
                            
    }

    //Change the text value when the slide value changes
    public void OnSlide(Slider sld)
    {
        if(sld.name == "Slider Mental Demand")
        {
            MentalDemandValue.text = sld.value.ToString();
        }
        else if (sld.name == "Slider Performance")
        {
            PerformanceValue.text = sld.value.ToString();
        }
        else if (sld.name == "Slider Effort")
        {
            EffortValue.text = sld.value.ToString();
        }
        else if (sld.name == "Slider Temporal Demand")
        {
            TemporalDemandValue.text = sld.value.ToString();
        }
        else if (sld.name == "Slider Physical Demand")
        {
            PhysicalDemandValue.text = sld.value.ToString();
        }
        else
        {
            FrustrationValue.text = sld.value.ToString();
        }
    }


    // Used to average lists
    private float moylist(List<float> list)
    {
        float moy = 0;
        for (int i = 0; i < list.Count; i++)
        {
            moy += list[i];
        }
        moy = moy / list.Count;
        return moy;
    }


}
