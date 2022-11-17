using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class errorCounter : MonoBehaviour
{
    public GameObject X1;
    public GameObject X2;
    public GameObject X3;
    public GameObject X4;
    
    // choose if making 5 errors will end the application
    public bool deadly = false;

    private int errorCount;

    // deactivate 
    public void Start()
    {
        errorCount = 0;
        X1.SetActive(false);
        X2.SetActive(false);
        X3.SetActive(false);
        X4.SetActive(false);
    }

    // count the number of errors via events and end th application if deady is activated
    public void CountAndShow()
    {
        errorCount += 1;

        if (errorCount == 1)
        {
            X1.SetActive(true);
        }       
        else if (errorCount == 2)
        {
            X2.SetActive(true);
        }        
        else if (errorCount == 3)
        {
            X3.SetActive(true);
        }
        else if (errorCount == 4) 
        {
            X4.SetActive(true);
        }
        else if (errorCount <= 5 && deadly)
        {
            SceneManager.LoadScene("Grand Menu");
            //errorCount = 0;
        }
    }

}
