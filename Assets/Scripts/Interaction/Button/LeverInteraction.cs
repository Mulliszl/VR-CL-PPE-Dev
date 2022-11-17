using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// allow us to know when the player interact with levers

public class LeverInteraction : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    private void OnTriggerExit(Collider other)
    {
        onTriggerEnter?.Invoke();
    }
}
