using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.XR;
using Microsoft.MixedReality.Input;

public class MainTripleTask : MonoBehaviour
{
    //Text inputs
    public Text result;
    public Text current;
    public Text textTuto;
    public GameObject Tangage;

    public GameObject Line;
    private RectTransform RTLine;

    private int direction;

    //Buttons
    public UnityEvent throwError;

    int nombre1, nombre2;
    int Ioperation;
    float calcul;
    float activatedButton = 0;
    private bool rightAnswer;
    private bool wait, complex;

    //audio sources
    public AudioSource Audio;

    //controller
    public XRNode LeftInputController;
    public XRNode RightInputController;
    private bool click, check;
    private int waiter;
    Vector2 inputAxis;
    List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();


    private RectTransform RTtangage;
    //bool wait = false;

    //internals var
    bool ready = true; // Verify if an audio routine can be throw
    bool waitc = false;
    bool waitd = false;
    bool once = true;
    bool debu = false; // Used to verify if a task was started
    bool startd = true; // Used to verify if the scene was started
    bool end = false;
    bool waitT = true;
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

    public float moySimple;
    public float moyComplex;

    

    




    private void Start()
    {
    RTLine = Line.GetComponent<RectTransform>();
        _globalWaiter = scriptLocation.GetComponent<GlobalWaiter>();
        current.text = "";
        result.text = "ready?";

        // launch or not the tuto
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

        //Get The Tangage Object to manipulate the position of the line
        RTtangage = Tangage.GetComponent<RectTransform>();
        
        // the order the games are displayed
        if (GlobalConfig.reverse == false)
            complex = false;
        else
            complex = true;
        
        // get values for average response times
        simpleTask = new List<float>();
        complexTask = new List<float>();
    }

    private void debut()
    {
        //initialyse mini games
        one(true);
        //duration of part1
        StartCoroutine(Phase1Waiter());
    }

    private void one(bool first)
    {
        // check if ever entered in part 1
        if (first)
        {
            sensorsDataScript.Stage = "Partie 1";
            if (GlobalConfig.reverse == false)
            {
                StartLineTask();
            } 
            else
            {
                CalculGenerator();
                CalcAndShow();
                StartLineTask();
            }
        }
        else
        {
            sensorsDataScript.Stage = "Partie 2";
            if (GlobalConfig.reverse == false)
            {
                CalculGenerator();
                CalcAndShow();
                StartLineTask();
            }
            else
            {
                StartLineTask();
            } 


        }

    }

    private void Phase1()
    {
        //calcul = 1000;
    }
    private void Phase2()
    {
        CalculGenerator();
        CalcAndShow();
    }

    // Update is called once per frame
    public void Update()
    {
        // go is pressed
        if (debu)
        {   //only once
            if (startd)
            {
                //Call the function to initate the first task
                UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, devices);
                debut();
                startd = false;
            }
            //don't work during pause and end
            if (wait == false && end == false)
            {   // wait for debounce
                if (!waitd)
                {
                    MoveLine(0.01f);
                    //VerificationVertical();
                }
                if(complex)
                {//calculations
                    if (rightAnswer == true && end == false)
                    {
                        rightAnswer = false;
                        Phase2();
                    }
                }

            }

            else if (end == true) { }

            else
            {
                // Verify if the pause was not did already
                if (once)
                {//set pause
                    once = false;
                    // Start to count the pause
                    StartCoroutine(pauseWaiter());
                    sensorsDataScript.Stage = "Pause";
                }
                current.text = "pause";
                result.text = "pause";
            }
            
            // play audio stimulus
            if (ready && wait == false && end == false)
            {
                ready = false;
                Audio.Play();
                StartCoroutine(TimerAudio());
            }

            // get buttons states
            Listen();

            // Buffer to block multiples selections per seconds
            if (click && waitc == false)
            {
                waitc = true;
                StartCoroutine(RightWaiter());
            }
        }

    }
    //generate simple calculs
    private void CalculGenerator()
    {

        //nombre1 = UnityEngine.Random.Range(-9, 10);
        nombre1 = UnityEngine.Random.Range(10, 100);
        //nombre2 = UnityEngine.Random.Range(-9, 10);
        nombre2 = UnityEngine.Random.Range(10, 100);
        //Ioperation = UnityEngine.Random.Range(0, 3);
        Ioperation = 0;
    }
    //generate complex calculs
    private void ComplexCalculGenerator()
    {

        nombre1 = UnityEngine.Random.Range(100, 200);
        nombre2 = UnityEngine.Random.Range(100, 200);
        //Ioperation = UnityEngine.Random.Range(0, 3);
        Ioperation = 0;
    }
    //calculate every possibility
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

    // check answer and send result to csv
    private void VerificationCalcul()
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
        if (GlobalConfig.scale)
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
        Debug.Log("tuto");
        // Tutorial to wait for startup of cognitive load writing
        textTuto.text = "Visez le chiffre souhait et appuyez sur <<trigger>> pour le sélectionner";
        yield return new WaitForSeconds(5);
        sensorsDataScript.Stage = "Tutoriel";
        yield return new WaitForSeconds(10);
        textTuto.text = "une fois sélectionné il s'ajoute  la liste de saisie";
        yield return new WaitForSeconds(10);
        textTuto.text = "Appuyez sur << = >> pour valider la saisie en cours";
        yield return new WaitForSeconds(10);
        textTuto.text = "Pour effacer appuyez sur la saisie en cours";
        yield return new WaitForSeconds(10);
        textTuto.text = "utilisez le joystick pour faire monter ou descendre l'image";
        yield return new WaitForSeconds(10);
        textTuto.text = "utilisez le bouton A pour valider";
        yield return new WaitForSeconds(10);
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
        }


    }
    public void Button_VAL_Activated()
    {

        // try calculate with invalid simbols
        try
        {
            activatedButton = float.Parse(current.text);
        }
        catch (FormatException)
        {
            Debug.Log("le format n'est pas adapt");
            current.text = "";
        }
        current.text = "";
        VerificationCalcul();
    }

    // Wait until next audio stimulus
    IEnumerator TimerAudio()
    {
        waiter = UnityEngine.Random.Range(GlobalConfig.AudioStimulusAverage - GlobalConfig.AudioStimulusDelta, GlobalConfig.AudioStimulusAverage + GlobalConfig.AudioStimulusDelta +1);
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
            if(counter > GlobalConfig.MaximumResponseTime)
            {
                break;
            }
            //Wait for a frame so that Unity doesn't freeze
            yield return null;
        }
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
    // Get the joystick position
    void Listen()
    {

        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(LeftInputController);
        leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out click);

        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(RightInputController);
        rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out check);
        rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);


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

    // stop triggering the buttons each frame
    IEnumerator JoystickWaiterDebounce()
    {
        waitd = true;
        yield return new WaitForSecondsRealtime(0.5f);
        waitd = false;
    }

    //Initiate the Line Task
    void StartLineTask()
    {
        // Get a random number to decide the direction of the line
        int newDirection = UnityEngine.Random.Range(0, 2);
        switch (newDirection)
        {
            case 0:

                direction = 1;
                break;

            case 1:

                direction = -1;
                break;
        }

        // Put the line at the position 0
        RTLine.localPosition = new Vector3(RTLine.localPosition.x, 0, RTLine.localPosition.z);
    }

    void MoveLine(float v)
    {

        // Move the line to the direction with velocity v
        RTLine.localPosition = new Vector3(RTLine.localPosition.x, RTLine.localPosition.y + (direction), RTLine.localPosition.z);
        RTLine.Translate(0, Time.deltaTime * direction * v, 0);


        // Verify if the line passed the limits
        if (RTLine.localPosition.y >= 325)
        {
            StartLineTask();
            sensorsDataScript.Stabilization = "Wrong";
        }
        else if (RTLine.localPosition.y <= -325)
        {
            StartLineTask();
            sensorsDataScript.Stabilization = "Wrong";
        }
        // Verify if the button was pressed and if the line is in the green zone, if yes change the line movement direction
        else if (check && ((RTLine.localPosition.y <= 150 && direction > 0) || (RTLine.localPosition.y >= -150 && direction < 0)))
        {
            //StartLineTask();
            sensorsDataScript.Stabilization = "Wrong";
        } 
        else if (check)
        {
            sensorsDataScript.Stabilization = "Correct";
            check = false;
            direction = direction * -1;
            StartCoroutine(JoystickWaiterDebounce());
        }
    }

    // Send a vibration to the controller when the Audio is answered
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
                    float duration = 0.2f;
                    device.SendHapticImpulse(channel, amplitude, duration);
                }
            }
        }
    }


}
