using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.XR;
using Microsoft.MixedReality.Input;

// Main script for calcul scene

public class Calculomatic : MonoBehaviour
{
    //Text inputs
    public Text result;
    public Text current;
    public Text textTuto;

    //buttons
    public UnityEvent throwError;

    int nombre1, nombre2;
    int Ioperation;
    float calcul;
    float activatedButton = 0;
    private bool rightAnswer;
    private bool wait,complex;

    //Audio sources
    public AudioSource Audio;

    //Controller
    public XRNode inputController;
    private bool click;
    private int waiter;
    List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

    //Internals var
    private bool ready = true; // Verify if an audio routine can be throw
    private bool waitc = false;
    private bool once = true;
    private bool debu = false; // Used to verify if a task was started 
    private bool startd = true; // Used to verify if the scene was started 
    private bool end = false;
    private bool waitT = true;
    private bool tutorialAchieved = false;
    private float counter = 0;
    private List<float> simpleTask; // used to save the audio task response time at the simple calcul phase
    private List<float> complexTask; // used to save the audio task response time at the complex calcul phase
    //private List<AudioSource> test = {frontAudio, backAudio, rightAudio, leftAudio };

    public static List<AudioSource> audioSourceList;
    private SensorsData sensorsDataScript;
    private GlobalWaiter _globalWaiter;
    public GameObject scriptLocation;
    public GameObject scale;

    [HideInInspector]
    public float moySimple;
    [HideInInspector]
    public float moyComplex;


    private void Start()
    {
        current.text = "";
        result.text = "ready?";

        // Check if there will have the tutorial
        if (GlobalConfig.skipTuto)
        {
            //Set tutorialAchieved as true, clean the textTuto and write go at current 
            tutorialAchieved = true;
            current.text = "go";
            textTuto.text = "";
        }
        else
            //Start the coroutine for the tutorial
            StartCoroutine(TutorialWaiter());

        //Get the SensorData Object to write in some csv file columns
        sensorsDataScript = scriptLocation.GetComponent<SensorsData>();

        //Get the GlobalWaiter Text used to count the time pause 
        _globalWaiter = scriptLocation.GetComponent<GlobalWaiter>();
        
        // Verify if the task have to initiate by the complex task or simple task first
        if (GlobalConfig.reverse == false) complex = false;
        else complex = true;

        //Lists of float to save the all Audio Response Time
        simpleTask = new List<float>();
        complexTask = new List<float>();

        /*
        //INIWorker.IniWriteValue("interactions", "CalculDemande", result.text.ToString());
        //INIWorker.IniWriteValue("userid", "name", "patrick");
        
        sante = INIWorker.IniReadValue("userid", "sante");
        scene = INIWorker.IniReadValue("experience", "scene");
        INIWorker.IniWriteValue("userid", "age", "30");
        Debug.Log(sante);
        Debug.Log(scene);
        sante =" true";
        //INIWorker.IniWriteValue("userid", "sante", "bof");
        */
    }

    // Initialize the scene 
    private void debut()
    {
        one(true);
        // Start the coroutine Phase1
        StartCoroutine(Phase1Waiter());
    }

    // throw first calcul of each half
    private void one(bool first)
    {
        // check if ever entered in part 1
        if (first)
        {
            sensorsDataScript.Stage = "Partie 1";
            if (GlobalConfig.reverse == false)
                CalculGenerator();
            else
                ComplexCalculGenerator();
        }
        else
        {
            sensorsDataScript.Stage = "Partie 2";
            if (GlobalConfig.reverse == false)
                ComplexCalculGenerator();
            else
                CalculGenerator();
            
        }
        CalcAndShow();
    }

    //simple calculs
    private void Phase1()
    {
        CalculGenerator();
        CalcAndShow();
    }
    //complex calculs
    private void Phase2()
    {
        ComplexCalculGenerator();
        CalcAndShow();
    }

    // Update is called once per frame
    public void Update()
    {   // Wait for "go"
        if (debu)
        {   
            //Only once
            if (startd)
            {
                //get all left input devices
                UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, devices);
                //Call the function to initate the first task
                debut();
                startd = false;
            }
            
            // Verify if it is not in pause
            if (wait == false)
            {
                // In phase 1
                if (complex == false)
                {
                    // Verify if the answer was given
                    if (rightAnswer == true)
                    {
                        rightAnswer = false;
                        if (GlobalConfig.reverse == false)
                
                            Phase1();
                        else
                            Phase2();
                    }
                }
                
                // In phase 2
                else
                {

                    // Verify if the answer was given
                    if (rightAnswer == true)
                    {
                        rightAnswer = false;
                        if (GlobalConfig.reverse == false)
                            Phase2();
                        else
                            Phase1();
                    }
                }

            }
            else
            {
                // Verify if the pause was not did already
                if (once)
                {
                    once = false;
                    // Start to count the pause
                    StartCoroutine(pauseWaiter());
                    sensorsDataScript.Stage = "Pause";
                }
                current.text = "pause";
                result.text = "pause";
            }
            
            // Trigger audio stimulus
            if (ready && wait == false && end == false)
            {
                ready = false;
                Audio.Play();
                StartCoroutine(TimerAudio());
            }

            // Get inputs states
            Listen();

            // Buffer to block multiples selections per seconds
            if (click && waitc == false)
            {
                waitc = true;
                StartCoroutine(RightWaiter());
            }
        }

    }


    // Get simple random numbers
    private void CalculGenerator()
    {

        //nombre1 = UnityEngine.Random.Range(-9, 10);
        nombre1 = UnityEngine.Random.Range(10, 100);
        //nombre2 = UnityEngine.Random.Range(-9, 10);
        nombre2 = UnityEngine.Random.Range(10, 100);
        //Ioperation = UnityEngine.Random.Range(0, 3);
        Ioperation = 0;
    }
    private void ComplexCalculGenerator()
    {

        nombre1 = UnityEngine.Random.Range(100, 200);
        nombre2 = UnityEngine.Random.Range(100, 200);
        //Ioperation = UnityEngine.Random.Range(0, 3);
        Ioperation = 0;
    }


    private void CalcAndShow()
    {
        // Addition operator
        if (Ioperation == 0)
        {
            //sensorsDataScript.SetResponseTime("addition");
            calcul = nombre1 + nombre2;
            result.text = "(" + nombre1 + ")" + " + " + "(" + nombre2 + ")";
        }

        // Subtraction operator
        else if (Ioperation == 1)
        {
            //sensorsDataScript.SetResponseTime("soustraction");
            calcul = nombre1 - nombre2;
            result.text = "(" + nombre1 + ")" + " - " + "(" + nombre2 + ")";
        }

        // Multiplication operator
        else if (Ioperation == 2)
        {
            //sensorsDataScript.SetResponseTime("multiplication");
            calcul = nombre1 * nombre2;
            result.text = "(" + nombre1 + ")" + " * " + "(" + nombre2 + ")";
        }
    }

    // check the answer and send to csv
    private void Verification()
    {
        if (activatedButton == calcul)
        {
            rightAnswer = true;
            activatedButton = 0;
            sensorsDataScript.AnswerMath = "Correct";
        }
        else if (activatedButton != 0)
        {
            rightAnswer = false;
            throwError?.Invoke();
            activatedButton = 0;
            sensorsDataScript.AnswerMath = "Wrong";
        }
    }
    //activatedButton = calcul1;

    IEnumerator Phase1Waiter()
    {
        //Wait the time to end the first task
        yield return new WaitForSeconds(GlobalConfig.experienceTime);
        wait = true;
        if (GlobalConfig.reverse == false)
            complex = true;
        else
            complex = false;
    }
    IEnumerator Phase2Waiter()
    {

        // Wait the time to end the second task
        yield return new WaitForSeconds(GlobalConfig.experienceTime);
        end = true;
        result.text = "FIN";
        current.text = "FIN";
        sensorsDataScript.Stage = "FIN";

        // Verify if the NASA-TLX Scale has to be showed 
        if (GlobalConfig.scale)
        {
            moySimple = moylist(simpleTask);
            moyComplex = moylist(complexTask);

            // Show the scale 
            Scale a = scale.GetComponent<Scale>();
            a.answered = false;
            scale.SetActive(true);
            while (a.answered == false)
            {
                yield return null;
            }
        }

        Application.Quit();
    }

    
    IEnumerator waitpause()
    {
        _globalWaiter.timerPause();
        yield return new WaitForSeconds(GlobalConfig.pauseTime);
        waitT = false;
    }

    //Coroutine to wait the pause time and show the Nasa-TLX scale
    IEnumerator pauseWaiter()
    {
        result.text = "";
        if(GlobalConfig.scale)
        {
            StartCoroutine(waitpause());
            Scale a = scale.GetComponent<Scale>();
            a.answered = false;
            scale.SetActive(true);
            while (a.answered == false)
            {
                yield return null;
            }
            while (waitT == true)
            {
                yield return null;
            }

        }
        else
        {
            yield return new WaitForSeconds(GlobalConfig.pauseTime);
        }
        wait = false;
        one(false);
        current.text = "";
        StartCoroutine(Phase2Waiter());
    }

    IEnumerator TutorialWaiter()
    {   
        // Tutorial to wait for startup of cognitive load writing
        textTuto.text = "Visez le chiffre souhaité et appuyez sur <<trigger>> pour le sélectionner";
        yield return new WaitForSeconds(5);
        sensorsDataScript.Stage = "Tutoriel";
        yield return new WaitForSeconds(25);
        textTuto.text = "une fois sélectionné il s'ajoute à la liste de saisie";
        yield return new WaitForSeconds(10);
        textTuto.text = "Appuyez sur << = >> pour valider la saisie en cours";
        yield return new WaitForSeconds(10);
        textTuto.text = "Pour effacer appuyez sur la saisie en cours";
        yield return new WaitForSeconds(15);
        current.text = "go";
        textTuto.text = "Appuyez sur go pour démarrer";
        tutorialAchieved = true;
    }

    // Buttons for calcuation, add the clicked digits to current
    public void Button_1_Activated()
    {
        current.text = current.text + "1";
    }
    public void Button_2_Activated()
    {
        current.text = current.text + "2";
    }
    public void Button_3_Activated()
    {
        current.text = current.text + "3";
    }
    public void Button_4_Activated()
    {
        current.text = current.text + "4";
    }
    public void Button_5_Activated()
    {
        current.text = current.text + "5";
    }
    public void Button_6_Activated()
    {
        current.text = current.text + "6";
    }
    public void Button_7_Activated()
    {
        current.text = current.text + "7";
    }
    public void Button_8_Activated()
    {
        current.text = current.text + "8";
    }
    public void Button_9_Activated()
    {
        current.text = current.text + "9";
    }
    public void Button_0_Activated()
    {
        current.text = current.text + "0";
    }
    public void Button_minus_Activated()
    {
        current.text = current.text + "-";
    }
    public void Button_DEL_Activated()
    {
        // try erase nothing
        try
        {
            current.text = current.text.Remove(current.text.Length - 1);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log("il n'y a rien a effacer");
        }
        if (debu == false && startd == true && tutorialAchieved)
        {
            debu = true;
            current.text = "";
            textTuto.text = "";
        }

        
    }
    public void Button_VAL_Activated()
    {
        // try calculate with invalid simbols
        try
        {
            activatedButton = float.Parse(current.text);
        }
        catch(FormatException)
        {
            Debug.Log("le format n'est pas adapt�");
            current.text = "";
        }
        current.text = "";
        Verification();
    }

    // Wait until next audio stimulus
    IEnumerator TimerAudio()
    {
        waiter = UnityEngine.Random.Range(GlobalConfig.AudioStimulusAverage - GlobalConfig.AudioStimulusDelta, GlobalConfig.AudioStimulusAverage + GlobalConfig.AudioStimulusDelta + 1);
        Debug.Log(waiter);
        StartCoroutine(rapidite());
        yield return new WaitForSeconds(waiter);
        ready = true;
    }

    IEnumerator RightWaiter()
    {

        yield return new WaitForSecondsRealtime(0.5f);
        waitc = false;
    }

    //count how much time you take to respond

    IEnumerator rapidite()
    {
        counter = 0;

        // If the audio is not answered yet, count
        while (click == false)
        {
            counter += Time.deltaTime;
            if (counter > GlobalConfig.MaximumResponseTime)
            {
                break;
            }
            //Wait for a frame so that Unity doesn't freeze
            yield return null;
        }
        //send haptic response if joystick is clicked
        if (click)
        {
            vibration();
        }
        // Set the attribute Reponse Time Audio to be write in the csv
        sensorsDataScript.SetResponseTime(counter.ToString());

        // Push the Response Time to the respective list
        if (complex == false)
        {
            simpleTask.Add(counter);
        }
        else if (complex == true)
        {
            complexTask.Add(counter);
        }

    }

    // Get the joystick click
    void Listen()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputController);
        device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out click);
        //MotionController.GetPressableInputs();
    }

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

    void vibration()
    {
        foreach (var device in devices)
        {
            UnityEngine.XR.HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    float amplitude = 1f;
                    float duration = 0.5f;
                    device.SendHapticImpulse(channel, amplitude, duration);
                }
            }
        }
    }

}



/*
 * inputs
batteryLevel = work  0.07
menuButton = work
secondaryButton = null

Questionnaire

Mental Demand
Performance
Effort
Frustration



*/
