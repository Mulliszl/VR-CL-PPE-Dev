using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;




// 1 - choose audio source
// 2 - play it
// 3 - wait for the player to respond
// 4 - compare




public class AudioInteractable : MonoBehaviour
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
    bool ready = true;
    bool wait = false;
    private bool TutoAchieved = false;
    private bool TutoP2Achieved = false;
    int iterations = 0;
    //private List<AudioSource> test = {frontAudio, backAudio, rightAudio, leftAudio };

    public static List<AudioSource> audioSourceList;

    //events
    public UnityEvent throwError;
    public UnityEvent EndTutoAudio;
    public UnityEvent EndTutoAudio2;
    public UnityEvent resetTimer;
    // Update is called once per frame

    private void Update()
    {
        if (ready)
        {
            ready = false;
            actualSource = AudioSelection();
            resetTimer?.Invoke();
            actualSource.Play();
            iterations += 1;
        }

        // listen and return answer
        Listen();
        guessedSource = ChooseDirection();
        if(guessedSource == actualSource && wait == false)
        {
            wait = true;
            StartCoroutine(RightWaiter());
            
        }
        else if(guessedSource != null && wait == false)
        {
            wait = true;
            StartCoroutine(WrongWaiter());
        }
        //tutoriel phases for triple task
        if(iterations == 5 && TutoAchieved == false)
        {
            TutoAchieved = true;
            EndTutoAudio?.Invoke();
        }
        else if(iterations == 10 && TutoP2Achieved == false)
        {
            TutoP2Achieved = true;
            EndTutoAudio2?.Invoke();
        }

    }
  

    // buffer for correct answer
    IEnumerator RightWaiter()
    {
        ready = true;
        yield return new WaitForSecondsRealtime(0.2f);
        wait = false;
    }

    // buffer for wrong answer
    IEnumerator WrongWaiter()
    {
        throwError?.Invoke();
        yield return new WaitForSecondsRealtime(0.2f);
        wait = false;
    }

    // Get the joystick position
    void Listen()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputController);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    // save the direction choosed by the user
    private AudioSource ChooseDirection()
    {
        Debug.Log (inputAxis);
        if (inputAxis[0] == 1.0f)
        {
            //Debug.Log ("yay");
            soundChoosed = rightAudio;
        } 
        else if(inputAxis[0] == -1.0f)
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

    // link joystick direction to audioSource
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


}
