using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class vertical : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Tangage;
    private RectTransform RTtangage;

    public GameObject Line;
    private RectTransform RTLine;

    private int direction;

    public XRNode inputController;
    bool wait = false;

    //private Vector3 tst =  new Vector3(float(0.0), float(0.0), float(0.0));
    void Start()
    {
        RTtangage = Tangage.GetComponent<RectTransform>();

        RTLine = Line.GetComponent<RectTransform>();

        StartLineTask();

        ThrowStabilizationTask();
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait)
        {
            VerifyControllers();
            StartCoroutine(JoystickWaiterDebounce());
        }

        MoveLine(1);
    }

    void ThrowStabilizationTask()
    {
        int newPosition = Random.Range(0, 7);
        switch (newPosition){
            case 0:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, -80, RTtangage.localPosition.z);
                break;
            case 1:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, -60, RTtangage.localPosition.z);
                break;
            case 2:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, -40, RTtangage.localPosition.z);
                break;
            case 3:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, -20, RTtangage.localPosition.z);
                break;
            case 4:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, 20, RTtangage.localPosition.z);
                break;
            case 5:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, 40, RTtangage.localPosition.z);
                break;
            case 6:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, 60, RTtangage.localPosition.z);
                break;
            case 7:
                RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, 80, RTtangage.localPosition.z);
                break;
            default:
                break;
        }
    }

    void VerifyControllers()
    {
        bool primaryButton;
        Vector2 inputAxis;

        InputDevice device = InputDevices.GetDeviceAtXRNode(inputController);

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButton) && primaryButton)
        {

            Debug.Log("Confirm button is pressed.");

            if (RTtangage.localPosition.y == 0)
            {
                ThrowStabilizationTask();
            }
        }
        if(device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis) && inputAxis[1] == 1)
        {
            Debug.Log("Up button is pressed.");


            if (RTtangage.localPosition.y != 80)
            {
                SolveStabilization(20);
            }

        }

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis) && inputAxis[1] == -1)
        {
            Debug.Log("Down button is pressed.");

            if (RTtangage.localPosition.y != -80)
            {
                SolveStabilization(-20);
            }

        }

    }

    void StartLineTask()
    {
        int newDirection = Random.Range(0, 1);
        switch (newDirection)
        {
            case 0:

                direction =  1;
                break;
                
            case 1:

                direction =  -1;
                break;
        }
        RTLine.localPosition = new Vector3(RTLine.localPosition.x, 0, RTLine.localPosition.z);
    }

    void MoveLine(float v)
    {
        RTLine.localPosition = new Vector3(RTLine.localPosition.x, RTLine.localPosition.y + (direction), RTLine.localPosition.z);
        RTLine.Translate(0, Time.deltaTime*direction*v, 0);

        if(RTLine.localPosition.y == 100)
        {
            StartLineTask();
        }
        else if (RTLine.localPosition.y == -100)
        {
            StartLineTask();
        }
    }

    void SolveStabilization(int increment)
    {
        
        RTtangage.localPosition = new Vector3(RTtangage.localPosition.x, RTtangage.localPosition.y + increment, RTtangage.localPosition.z);
       
    }

    IEnumerator JoystickWaiterDebounce()
    {
        wait = true;
        yield return new WaitForSecondsRealtime(0.1f);
        wait = false;
    }
}




