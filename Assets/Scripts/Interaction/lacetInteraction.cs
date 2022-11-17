using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// 1 - choisi une rotation du background
// 2 - attend une corresction du joueur pour le corriger

public class lacetInteraction : MonoBehaviour
{
    public GameObject background;
    private RectTransform movement;
    public bool deadly;
    private int actualRotation;
    public float accelz;
    private int ready;
    private int tmpNow;
    private int tmpGlobal;
    private float angle;

    public GameObject lever1;
    public GameObject lever2;
    public GameObject leverHead1;
    public GameObject leverHead2;
    private HingeJoint joint1;
    private HingeJoint joint2;
    private int offset1;
    private int offset2;
    private int iterations = 0;
    private bool TutoAchieved = false;

    public UnityEvent LeverDirectionWrong;
    public UnityEvent LeverDirectionCorrect;

    public UnityEvent EndTutoLacet;
    public UnityEvent throwError;
    //string[] lines;

    void Start()
    {
        movement = background.GetComponent<RectTransform>();
        ready = 0;
        Application.targetFrameRate = 60;
        joint1 = lever1.GetComponent<HingeJoint>();
        joint2 = lever2.GetComponent<HingeJoint>();
    }

    void Update()
    {
        StartCoroutine(Wait());
        if  (iterations >= 60 && TutoAchieved == false)
        {
            TutoAchieved = true;
            EndTutoLacet?.Invoke();
        }
    }
// don't start right now
    IEnumerator Wait()
    {
        
        yield return new WaitForSeconds(5);
        rotation();
        CheckAngle();
    }

    private void rotation()
    {
        if(ready == 0)
        {
            //choose next acceleration
            tmpNow = Random.Range(-1, 2);
            //change acceleration only 3 time per seconds (fps)
            ready = 20;
            //update speed
            tmpGlobal += tmpNow;
            iterations += 1;     
        }


        // set maximum speed
        if (tmpGlobal > 5)
        {
            tmpGlobal = 5;
        }
        else if (tmpGlobal < -5)
        {
            tmpGlobal = -5;
        }

        // get speed with levers offsets
        accelz = (offset1 + offset2 + tmpGlobal);

        angle = movement.eulerAngles.z;
        // rotation go from -180 to 180 instead of [0,360]
        angle = (angle > 180) ? angle - 360 : angle;
        // rotate image
        movement.Rotate(0,0, accelz * Time.deltaTime);
        // errors if to much rotation
        if (angle > 60)
        {
            movement.eulerAngles = new Vector3(movement.eulerAngles.x, movement.eulerAngles.y, 0);
            if (deadly)
            {
                throwError?.Invoke();
            }
           
        }
        if (angle < -60)
        {
            movement.eulerAngles = new Vector3(movement.eulerAngles.x, movement.eulerAngles.y, 0);
            if (deadly)
            {
                throwError?.Invoke();
            }
        }
        ready -= 1;
        
        //Debug.Log(ready);
    }

    public void VerifyLever()
    {
        if ((angle > 0 && offset1+offset2 < 0) || (angle < 0 && offset1 + offset2 > 0))
        {
            LeverDirectionCorrect?.Invoke();
        }
        else
        {
            LeverDirectionWrong?.Invoke();
        }
    }

    void CheckAngle()
    {
        //angle 1
        //max
        if (joint1.angle >= joint1.limits.max * 0.7 && joint1.angle <= joint1.limits.max)
        {
            lever1.GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            leverHead1.GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            offset1 = -6;
        }

        //medium
        else if (joint1.angle >= joint1.limits.max * 0.3 && joint1.angle <= joint1.limits.max * 0.7)
        {
            lever1.GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
            leverHead1.GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
            offset1 = -3;
        } 
        //min
        else if (joint1.angle <= joint1.limits.max * 0.3)
        {
            lever1.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
            leverHead1.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
            offset1 = 0;
        }
        
        //angle 2
        //max
        if (joint2.angle >= joint2.limits.max * 0.7 && joint2.angle <= joint2.limits.max)
        {
            lever2.GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            leverHead2.GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            offset2 = 6;
        }
        
        //medium
        else if (joint2.angle >= joint2.limits.max * 0.3 && joint2.angle <= joint2.limits.max * 0.7)
        {
            lever2.GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
            leverHead2.GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
            offset2 = 3;
        } 
        //min
        else if (joint2.angle <= joint2.limits.max * 0.3)
        {
            lever2.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
            leverHead2.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
            offset2 = 0;
        }
    }

}
