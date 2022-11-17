using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// 1 - controller les mini jeux
// 2 - augmenter la difficulté


public class difficultyAdjustment : MonoBehaviour
{
    int x;
    double fctA, fctC;
    bool init = false;
    bool audioReady, calculReady = false;
    int audioCount, calculCount = 0;
    int aTime = 16;
    public UnityEvent TriggerCalcul;
    public UnityEvent TriggerAudio;


    int truc;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startWaiter());
    }

    // send signal to trigger mini games faster and faster
    void Update()
    {
        if (init)
        {
            if (audioReady)
            {
                audioReady = false;
                audioCount += 1;
                fctA = -Math.Log(audioCount)+8+2;
                aTime = (int)Math.Round(fctA);
                Debug.Log(aTime);
                StartCoroutine(audioWaiter(aTime));
            }
            if (calculReady)
            {
                calculReady = false;
                calculCount += 1;
                fctC = -Math.Log(audioCount)+8+2;
                aTime = (int)Math.Round(fctC);
                StartCoroutine(calculWaiter(aTime));
            }
            
        }
    }


    // initialyse and offset
    IEnumerator startWaiter()
    {
        yield return new WaitForSeconds(5);
        TriggerCalcul?.Invoke();
        calculCount += 1;
        yield return new WaitForSeconds(12);
        calculReady = true;
        TriggerAudio?.Invoke();
        audioCount += 1;
        yield return new WaitForSeconds(7);
        init = true;
        yield return new WaitForSeconds(5);
        audioReady = true;

    }
    // wait time before next audio game
    IEnumerator audioWaiter(int allocatedTime)
    {
        TriggerAudio?.Invoke();
        //alloc time
        yield return new WaitForSeconds(allocatedTime);
        audioReady = true;
    }

    // wait time before next calcul game
    IEnumerator calculWaiter(int allocatedTime)
    {
        TriggerCalcul?.Invoke();
        yield return new WaitForSeconds(allocatedTime);
        calculReady = true;
    }
}