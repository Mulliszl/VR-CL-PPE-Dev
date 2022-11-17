using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

// set a timer for audio task

public class AudioTimer : MonoBehaviour
{
    public UnityEvent throwError;
    //public AudioSource sonDepart;
    //public AudioSource sonFin;
    public Text CompteARebours;
    private float time = 15f;
    bool stopped = true;
    bool breakTime = false;
    private int iterations = 0;
    private double fctA;
    // Start is called before the first frame update
    private void TimerStart()
    {
        StartCoroutine(timer());
        time += 1;
    }

    public void breakTimer()
    {
        breakTime = true;
        //Debug.Log(breakTime);
    }


    public void resetTimer()
    {
        breakTime = false;
        if(iterations == 0)
        {
            //time = 11f;
            time = 5;
        }
        else
        {
            //fctA = -Math.Log(iterations) + 8;
            //time = (int)Math.Round(fctA);
            time = 5;
        }
        iterations += 1;
        
        if (stopped == true)
        {
            TimerStart();
        }
    }


    IEnumerator timer()
    {
        while (time > 0)
        {
            stopped = false;
            if (breakTime)
            {
                breakTime = false;
                stopped = true;
                break;
            }
            time -= 1;
            CompteARebours.text = time.ToString();
            yield return new WaitForSeconds(1f);

        }
        if (time == 0)
        {
            throwError?.Invoke();
            stopped = true;
        }
    }
}
