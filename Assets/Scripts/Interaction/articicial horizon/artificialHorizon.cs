using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class artificialHorizon : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _suivi;
    private Canvas EscCan;
    private RectTransform Crt;
    public GameObject Tangage;
    public GameObject Roulis;
    private RectTransform RTtangage;
    private RectTransform RTroulis;

    //private Vector3 tst =  new Vector3(float(0.0), float(0.0), float(0.0));
    void Start()
    {
 
        _suivi = GetComponentInParent<Transform>();
        GameObject tempObject = GameObject.Find("backgroundCanvas");
        RTtangage = Tangage.GetComponent<RectTransform>();
        RTroulis = Roulis.GetComponent<RectTransform>();
        if (tempObject != null)
        {
            //If we found the object , get the Canvas component from it.
            EscCan = tempObject.GetComponent<Canvas>();
            if (EscCan == null)
            {
                Debug.Log("Could not locate Canvas component on " + tempObject.name);
            }
            Crt = EscCan.GetComponent<RectTransform>();
        }


    }


    // Update is called once per frame
    void Update()
    {
        float rEuler_z = Crt.eulerAngles.z;
        //Quaternion tmp = (0f,0f,Crt.rotation.z,Space.Self);
        //RTtangage.SetPositionAndRotation(RTtangage.position, tmp);
        RTroulis.eulerAngles = new Vector3(RTroulis.eulerAngles.x, RTroulis.eulerAngles.y, rEuler_z );
        //Debug.Log(tmp);
    }
}




