using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalWaiter : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Compteur;

    public void timerPause()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        Compteur.text = "";
        for (int i = GlobalConfig.pauseTime; i > 0; i--)
        {
            Compteur.text = i.ToString();
            yield return new WaitForSeconds(1);
            //Compteur.text = i.ToString();
        }
        Compteur.text = "";
    }
}
