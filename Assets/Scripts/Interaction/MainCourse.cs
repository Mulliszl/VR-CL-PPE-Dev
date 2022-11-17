using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


// 1 - conttroller les mini jeux
// 2 - augmenter la difficulté


public class MainCourse : MonoBehaviour
{
    int x;
    private int init = 0;
    double fctA, fctC;
    bool audioReady = true, calculReady = true;
    int audioCount, calculCount = 0;
    int aTime = 16;
    int truc;

    public UnityEvent TriggerCalcul;
    public UnityEvent TriggerAudio;

    public Material materialOff;
    public Material materialOn;
    public GameObject ledStabilisation;

    public GameObject Rbutton;

    public Text instructions;
    public GameObject AudioGame;
    public GameObject MathGame;
    public GameObject StabilisationGame;
    private OneTimeAudioInteractable audioScript;
    private OneTimeCalculomatic calculScript;
    private lacetInteraction stabilisationScript;
    private SensorsData SensorsDataScript;
    private string sceneName;
    private bool buttonReady = false;

    private int AudioIterator , CalculIterator, nextStep;




    // 1 - Tutorial : explain how to use each game
    // 2 - Cognitive Load Step 1 : Math only
    // 3 - Cognitive Load Step 2 : Math + Audio
    // 4 - Cognitive Load Step 3 : Math + Audio + Stabilisation


    void Start()
    {
        SensorsDataScript = GetComponent<SensorsData>();
        audioScript = AudioGame.GetComponent<OneTimeAudioInteractable>();
        calculScript = StabilisationGame.GetComponent<OneTimeCalculomatic>();
        stabilisationScript = StabilisationGame.GetComponent<lacetInteraction>();
        //audioScript.enabled = false;
        //calculScript.enabled = false;
        stabilisationScript.enabled = false;
        ledStabilisation.GetComponent<MeshRenderer>().material = materialOff;
        SensorsDataScript.SetStage("PHASE 0 : TUTORIEL");
        sceneName = SceneManager.GetActiveScene().name;
        //Debug.Log(sceneName);
    }


    //
    void Update()
    {
        if (nextStep == 3)
        {
            if(init == 0)
            {
                SensorsDataScript.SetStage("PHASE 1 : CALCUL");
                instructions.text = "PHASE 1 - CALCUL\n 1 minute\nboutons";
                stabilisationScript.enabled = false;
                ledStabilisation.GetComponent<MeshRenderer>().material = materialOff;
                if (buttonReady == true || sceneName != "Cognitive-load-static-full-game-nasa-tlx")
                {
                    StartCoroutine(waiterTier1());
                }
                

            }
            if (sceneName == "Cognitive-load-static-full-game-nasa-tlx")
            {
                if (buttonReady)
                {
                    if (calculReady)
                    {
                        calculReady = false;
                        calculCount += 1;
                        fctC = -Math.Log(audioCount) + 8 + 2;
                        aTime = (int)Math.Round(fctC);
                        StartCoroutine(calculWaiter(aTime));
                    }
                }
            }
            else
            {
                if (calculReady)
                {
                    calculReady = false;
                    calculCount += 1;
                    fctC = -Math.Log(audioCount) + 8 + 2;
                    aTime = (int)Math.Round(fctC);
                    StartCoroutine(calculWaiter(aTime));
                }
            }
        }
        else if (nextStep == 4)
        {
            if(init == 1)
            {
                SensorsDataScript.SetStage("PHASE 2 : CALCUL + AUDIO");
                instructions.text = "PHASE 2 - CALCUL + AUDIO\n 1 minute 30 secondes\nboutons + joystick";
                if(buttonReady == true || sceneName != "Cognitive-load-static-full-game-nasa-tlx")
                {
                    StartCoroutine(waiterTier2());
                }
                //buttonReady = false;
            }
            if (sceneName == "Cognitive-load-static-full-game-nasa-tlx")
            {
                if (buttonReady)
                {
                    if (calculReady)
                    {
                        calculReady = false;
                        calculCount += 1;
                        fctC = -Math.Log(audioCount) + 8 + 2;
                        aTime = (int)Math.Round(fctC);
                        StartCoroutine(calculWaiter(aTime));
                    }
                    if (audioReady)
                    {
                        audioReady = false;
                        audioCount += 1;
                        fctA = -Math.Log(audioCount) + 8 + 2;
                        aTime = (int)Math.Round(fctA);
                        StartCoroutine(audioWaiter(aTime));
                    }
                }
            } else
            {
                if (calculReady)
                {
                    calculReady = false;
                    calculCount += 1;
                    fctC = -Math.Log(audioCount) + 8 + 2;
                    aTime = (int)Math.Round(fctC);
                    StartCoroutine(calculWaiter(aTime));
                }
                if (audioReady)
                {
                    audioReady = false;
                    audioCount += 1;
                    fctA = -Math.Log(audioCount) + 8 + 2;
                    aTime = (int)Math.Round(fctA);
                    StartCoroutine(audioWaiter(aTime));
                }
            }
        }
        else if (nextStep == 5)
        {
            if (init == 2)
            {
                SensorsDataScript.SetStage("PHASE 3 : CALCUL + AUDIO + STALILISATION");
                instructions.text = "PHASE 3 - CALCUL + AUDIO + STABILISATION\n 2 minute\nboutons + joystick + leviers";
                if (buttonReady == true || sceneName != "Cognitive-load-static-full-game-nasa-tlx")
                {
                    StartCoroutine(waiterTier3());
                }
                stabilisationScript.enabled = true;
                ledStabilisation.GetComponent<MeshRenderer>().material = materialOn;
                //buttonReady = false;
            }
            if (sceneName == "Cognitive-load-static-full-game-nasa-tlx")
            {
                if (buttonReady)
                {
                    if (calculReady)
                    {
                        calculReady = false;
                        calculCount += 1;
                        fctC = -Math.Log(audioCount) + 8 + 2;
                        aTime = (int)Math.Round(fctC);
                        StartCoroutine(calculWaiter(aTime));
                    }
                    if (audioReady)
                    {
                        audioReady = false;
                        audioCount += 1;
                        fctA = -Math.Log(audioCount) + 8 + 2;
                        aTime = (int)Math.Round(fctA);
                        StartCoroutine(audioWaiter(aTime));
                    }
                }
            }
            else
            {
                if (calculReady)
                {
                    calculReady = false;
                    calculCount += 1;
                    fctC = -Math.Log(audioCount) + 8 + 2;
                    aTime = (int)Math.Round(fctC);
                    StartCoroutine(calculWaiter(aTime));
                }
                if (audioReady)
                {
                    audioReady = false;
                    audioCount += 1;
                    fctA = -Math.Log(audioCount) + 8 + 2;
                    aTime = (int)Math.Round(fctA);
                    StartCoroutine(audioWaiter(aTime));
                }
            }
        }
        else if (nextStep == 6)
        {
            SceneManager.LoadScene("Grand Menu");
        }
        else
        {
            Tutorial();
        }
        //Debug.Log(aTime);
    }


    private void Tutorial()
    {
        if (nextStep == 0)
        {
            Step1();
        } else if (nextStep == 1)
        {
            Step2();
        } else if(nextStep == 2)
        {
            Step3();
        }
        
    }
    private void Step1()
    {
        if (calculReady)
        {
            calculReady = false;
            instructions.text = "TUTORIEL - CALCUL\nappuyez sur un des trois boutons pour répondre au calcul";
            calculCount += 1;
            aTime = 8;
            StartCoroutine(calculWaiter(aTime));
            CalculIterator += 1;
        }
        else if (CalculIterator == 5)
        {
            nextStep = 1;
        }
    }
    private void Step2()
    {
        if (audioReady && calculReady)
        {
            audioReady = false;
            audioCount += 1;
            aTime = 8;
            instructions.text = "TUTORIEL - AUDIO\nutilisez le joystick pour donner la provenance du son";
            StartCoroutine(audioWaiter(aTime));
            AudioIterator += 1;
        } else if (AudioIterator == 5)
        {
            nextStep = 2;
        }
    }
    private void Step3()
    {
        if (audioReady)
        {
            stabilisationScript.enabled = true;
            instructions.text = "TUTORIEL - STABILISATION\nutilisez les leviers pour redresser \nlevier gauche pour redresser dans le sens horraire\nlevier droit pour redresser dans le sens trigo\nvous pouvez voir la position de l'avion sur l'horizon artificiel";
            ledStabilisation.GetComponent<MeshRenderer>().material = materialOn;
            StartCoroutine(StabilisationWaiter());
            nextStep = 10;
            buttonReady = false;
        }

    }

    IEnumerator StabilisationWaiter()
    {
        yield return new WaitForSeconds(20);
        nextStep = 3;
        if (sceneName == "Cognitive-load-static-full-game-nasa-tlx")
        {
            Rbutton.SetActive(true);
        }
    }

    IEnumerator waiterTier1()
    {
        init = 1;
        Rbutton.SetActive(false);
        yield return new WaitForSeconds(60);
        buttonReady = false;
        Rbutton.SetActive(true);
        nextStep = 4;
    } 
    IEnumerator waiterTier2()
    {
        init = 2;
        Rbutton.SetActive(false);
        yield return new WaitForSeconds(90);
        buttonReady = false;
        Rbutton.SetActive(true);
        nextStep = 5;
    }
    IEnumerator waiterTier3()
    {
        init = 3;
        Rbutton.SetActive(false);
        yield return new WaitForSeconds(120);
        buttonReady = false;
        Rbutton.SetActive(true);
        nextStep = 6;
    }

    IEnumerator audioWaiter(int allocatedTime)
    {
        TriggerAudio?.Invoke();
        //alloc time
        yield return new WaitForSeconds(allocatedTime);
        audioReady = true;
    }

    IEnumerator calculWaiter(int allocatedTime)
    {
        TriggerCalcul?.Invoke();
        yield return new WaitForSeconds(allocatedTime);
        calculReady = true;
    }

    public void ButtonReady()
    {
        buttonReady = true;
        Rbutton.SetActive(false);

    }
}