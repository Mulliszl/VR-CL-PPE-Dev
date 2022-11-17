using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

// used for measuring the response time of the subject
// used in the calcul and double task scene

public class AudioClick : MonoBehaviour
{
    //audio sources
    public AudioSource Audio;

    //controller
    public XRNode inputController;
    private bool click;
    private int waiter;

    //internals var
    bool ready = true;
    bool wait = false;
    private float counter = 0;
    //private List<AudioSource> test = {frontAudio, backAudio, rightAudio, leftAudio };

    public static List<AudioSource> audioSourceList;
    private SensorsData sensorsDataScript;

    //events
    public UnityEvent throwError;



    private void Start()
    {
        sensorsDataScript = GetComponent<SensorsData>();
    }

    private void Update()
    {
        // each time the timer end a new sound is played
        if (ready)
        {
            ready = false;
            Audio.Play();
            StartCoroutine(TimerAudio());
        }

        Listen();
        if (click && wait == false)
        {
            wait = true;
            StartCoroutine(RightWaiter());
        }
    }

    // set the repetition rate of the audio stimulus (average 14s : get ~4 stimuli per minutes) 
    IEnumerator TimerAudio()
    {
        waiter = Random.Range(11, 18);
        StartCoroutine(rapidite());
        yield return new WaitForSeconds(waiter);
        ready = true;
    }


    // buffer to count only one activation per click
    IEnumerator RightWaiter()
    {
        
        yield return new WaitForSecondsRealtime(0.5f);
        wait = false;
    }

    // save the response time
    IEnumerator rapidite()
    {
        counter = 0;
        while (click == false)
        {
            counter += Time.deltaTime;
            //Wait for a frame so that Unity doesn't freeze
            yield return null;
        }
        Debug.Log(counter);
        sensorsDataScript.SetResponseTime(counter.ToString());

    }
    // Get the joystick position
    void Listen()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputController);
        device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out click);
    }

}
