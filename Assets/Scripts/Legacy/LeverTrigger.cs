using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private HingeJoint hinge;
    void Start()
    {
        //hingeJoint =  GetComponent<HingeJoint>();
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    void CheckAngle()
    {
        if(GetComponent<HingeJoint>().angle >= GetComponent<HingeJoint>().limits.max * 0.5 && GetComponent<HingeJoint>().angle <= GetComponent<HingeJoint>().limits.max)
        {
            //GetComponent<HingeJoint>().SetTrigger("hingeJointReachMedium");
        }

        if(GetComponent<HingeJoint>().angle == GetComponent<HingeJoint>().limits.max)
        {
            //GetComponent<HingeJoint>().SetTrigger("hingeJointReachMax");
        }
    }
}
