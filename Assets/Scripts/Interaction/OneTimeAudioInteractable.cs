using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

// same as audio interactable but set up to trigger once only when triggerStart() is called
public class OneTimeAudioInteractable : MonoBehaviour
{
    //audio sources
    public AudioSource frontAudio;
    public AudioSource backAudio;
    public AudioSource rightAudio;
    public AudioSource leftAudio;

    //controller
    public XRNode inputController;

    //internals var
    private Vector2 inputAxis;
    private AudioSource songDrawed;
    private AudioSource soundChoosed;
    private AudioSource actualSource;
    private AudioSource guessedSource;
    private int randomAudio;
    bool ready = false;
    bool wait = false;
    bool trigger = false;
    int iterations = 0;
    //private List<AudioSource> test = {frontAudio, backAudio, rightAudio, leftAudio };

    public static List<AudioSource> audioSourceList;

    //events
    public UnityEvent throwError;
    public UnityEvent resetTimer;
    public UnityEvent breakTimer;
    public UnityEvent throwCorrectAnswer;
    public UnityEvent throwWrongAnswer;

    public GameObject ledAudio;
    public Material MaterialOn;
    public Material MaterialOff;
    // Update is called once per frame

    private void Update()
    {
        if (trigger)
        {
            ready = false;
            trigger = false;
            actualSource = AudioSelection();
            ledAudio.GetComponent<MeshRenderer>().material = MaterialOn;
            resetTimer?.Invoke();
            //Debug.Log("Start Audio Task");
            actualSource.Play();
            iterations += 1;
        }
        if (ready)
        {
            ledAudio.GetComponent<MeshRenderer>().material = MaterialOff;
            breakTimer?.Invoke();
            //Debug.Log("End Audio Task");
            ready = false;

        }

        Listen();
        guessedSource = ChooseDirection();
        if (guessedSource == actualSource && wait == false && actualSource != null)
        {
            wait = true;
            actualSource = null;
            StartCoroutine(RightWaiter());
            throwCorrectAnswer?.Invoke();


        }
        else if (guessedSource != null && wait == false)
        {
            wait = true;
            StartCoroutine(WrongWaiter());
            throwWrongAnswer?.Invoke();
        }

    }



    IEnumerator RightWaiter()
    {
        ready = true;
        yield return new WaitForSecondsRealtime(0.5f);
        wait = false;
    }
    IEnumerator WrongWaiter()
    {
        throwError?.Invoke();
        yield return new WaitForSecondsRealtime(0.5f);
        wait = false;
    }
    // Get the joystick position
    void Listen()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputController);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        //Debug.Log(inputAxis[0]);
        //Debug.Log(inputAxis[1]);
    }

    private AudioSource ChooseDirection()
    {
        //Debug.Log (inputAxis);
        if (inputAxis[0] == 1.0f)
        {
            //Debug.Log ("yay");
            soundChoosed = rightAudio;
        }
        else if (inputAxis[0] == -1.0f)
        {
            soundChoosed = leftAudio;
        }
        else if (inputAxis[1] == 1.0f)
        {
            soundChoosed = frontAudio;
        }
        else if (inputAxis[1] == -1.0f)
        {
            soundChoosed = backAudio;
        }
        else
        {
            soundChoosed = null;
        }
        return soundChoosed;
    }

    private AudioSource AudioSelection()
    {
        randomAudio = Random.Range(0, 4);
        if (randomAudio == 0)
        {
            songDrawed = frontAudio;
        }
        else if (randomAudio == 1)
        {
            songDrawed = backAudio;
        }
        else if (randomAudio == 2)
        {
            songDrawed = rightAudio;
        }
        else if (randomAudio == 3)
        {
            songDrawed = leftAudio;
        }
        /*else
        {
            songDrawed = null;
        }*/
        return songDrawed;
    }

    public void triggerStart()
    {
        trigger = true;
    }
}
