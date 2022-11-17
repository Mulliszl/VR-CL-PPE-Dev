using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    UnityEngine.XR.InputDevice device;

    private void Start()
    {
        StartCoroutine(manettes());
    }
    IEnumerator manettes()
    {
        yield return new WaitForSeconds(1);
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);

        if (leftHandDevices.Count == 1)
        {
            device = leftHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", device.name, device.characteristics.ToString()));
        }
        else if (leftHandDevices.Count > 1)
        {
            Debug.Log("Found more than one left hand!");
        }
        else if(leftHandDevices.Count == 0)
        {
            Debug.Log("pas de manettes!");
            Start();
        }
    }

    private void Update()
    {
        bool triggerValue,buttonPressed;
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {
            Debug.Log("Trigger button is pressed.");
        } 
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out buttonPressed) && buttonPressed)
        {
            Debug.Log("Primary button is pressed.");
        }
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out buttonPressed) && buttonPressed)
        {
            Debug.Log("Secondary button is pressed.");
        }
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out buttonPressed) && buttonPressed)
        {
            Debug.Log("Grip button is pressed.");
        }
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out buttonPressed) && buttonPressed)
        {
            Debug.Log("Menu button is pressed.");
        }

    }
}
