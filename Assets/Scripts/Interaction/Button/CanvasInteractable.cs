using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasInteractable : MonoBehaviour
{
    public Text instruction;
    private Animator _canvasAnimator;
    private bool finished = true;
    private bool stop = false;
    private string instructions;
    void Start()
    {
        _canvasAnimator = GetComponent<Animator>();
    }


    // allow to expand and shrink the help panel
    public void Toogle(string instructions)
    {   // if already expanded
        if (_canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("pop-upExpand"))
        {   // if same help button
            if (instruction.text == instructions)
            {
                _canvasAnimator.SetTrigger("shrink");
            }
            else
            {// shrink, update instructions while close and expand
                _canvasAnimator.SetTrigger("shrink");
                StartCoroutine(waiterToCollapse(instructions));
                _canvasAnimator.SetTrigger("expand");
            }
        }
        else //if (_canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("pop-upShrink"))
        {
            _canvasAnimator.SetTrigger("expand");
            instruction.text = instructions;
        }

    }


    // allow to shrink on confirm
    public void Shrink()
    {
        if (_canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("pop-upExpand"))
        {
            _canvasAnimator.SetTrigger("shrink");
        }
    }

    // wait for the panel to be closed
    IEnumerator waiterToCollapse(string instructions)
    {
        yield return new WaitForSeconds(0.5f);
        instruction.text = instructions;
    }

    /*
    public void expand(string instructions)
    {
        //if (_canvasAnimator.GetCurrentAnimatorStateInfo(0).IsName("pop-upExpand"))
        if (finished)
        {
            _canvasAnimator.SetTrigger("expand");
            instruction.text = instructions;
        }
        else
        {
            shrink();
            StartCoroutine(waiterToCollapse(instructions));
            _canvasAnimator.SetTrigger("expand");
            stop = true;
        }
        StartCoroutine(waitShrink());
        
    }

    public void shrink()
    {
        _canvasAnimator.SetTrigger("shrink");
    }

    IEnumerator waitShrink()
    {
        finished = false;
        yield return new WaitForSeconds(5);
        
        if (stop == false)
        {
            shrink();
            finished = true;
        }

        else
        {
            stop = false;
            finished = false;
        }


        
    }

*/
}