using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    private Animator _buttonAnimator;
    void Start()
    {
        _buttonAnimator = GetComponent<Animator>();
    }

    // send trigger to launch the animations on buttons
    private void OnTriggerEnter(Collider col)
    {
        _buttonAnimator.SetTrigger("buttonPressed");
    }

    private void OnTriggerExit(Collider col)
    {
        _buttonAnimator.SetTrigger("buttonReleased");
        onTriggerEnter?.Invoke();
    }




}

