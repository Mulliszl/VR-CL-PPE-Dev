using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Security.Cryptography;


// same as Calcul interactable but set up to trigger once only when triggerStart() is called
public class OneTimeCalculomatic : MonoBehaviour
{
    //text inputs
    public Text result;
    public Text greenButton;
    public Text yellowButton;
    public Text redButton;

    public UnityEvent throwError;
    public UnityEvent startTimer;
    public UnityEvent breakTimer;
    public UnityEvent throwCorrectAnswer;
    public UnityEvent throwWrongAnswer;

    public GameObject ledCalcul;
    public Material MaterialOn;
    public Material MaterialOff;

    int nombre1, nombre2;
    int Ioperation;
    float calcul, calcul1, calcul2, calcul3;
    float activatedButton = 1.2f;
    private bool rightAnswer = false;
    private int iterations = 0;
    private List<Text> buttons;


    // Start is called before the first frame update
    void CalculStart()
    {
        buttons = new List<Text> { greenButton, yellowButton, redButton };
        CalculGenerator();
        Shuffle<Text>(buttons);
        CalcAndShow();
        startTimer?.Invoke();
        ledCalcul.GetComponent<MeshRenderer>().material = MaterialOn;
    }

    // Update is called once per frame
    public void Update()
    {
        iterations += 1;
        Verification();
        if (rightAnswer == true && activatedButton != 1.2f)
        {
            activatedButton = 1.2f;
            rightAnswer = false;
            result.text = "";
            greenButton.text = "";
            yellowButton.text = "";
            redButton.text = "";
            ledCalcul.GetComponent<MeshRenderer>().material = MaterialOff;
            breakTimer?.Invoke();
        }
    }
    private void CalculGenerator()
    {

        nombre1 = Random.Range(-9, 10);
        nombre2 = Random.Range(-9, 10);
        Ioperation = Random.Range(0, 3);

    }

    private void CalcAndShow()
    {
        // Addition operator
        if (Ioperation == 0)
        {
            calcul = nombre1 + nombre2;
            result.text = nombre1 + " + " + nombre2;
        }

        // Subtraction operator
        else if (Ioperation == 1)
        {
            calcul = nombre1 - nombre2;
            result.text = nombre1 + " - " + nombre2;
        }

        // Multiplication operator
        else if (Ioperation == 2)
        {
            calcul = nombre1 * nombre2;
            result.text = nombre1 + " * " + nombre2;
        }
        calcul1 = nombre1 + nombre2;
        calcul2 = nombre1 - nombre2;
        calcul3 = nombre1 * nombre2;
        buttons[0].text = calcul1.ToString();
        buttons[1].text = calcul2.ToString();
        buttons[2].text = calcul3.ToString();
    }

    private void Verification()
    {
        if (activatedButton == calcul && activatedButton!= 1.2f)
        {
            rightAnswer = true;
            throwCorrectAnswer?.Invoke();
        }
        else if (activatedButton != 1.2f)
        {
            rightAnswer = false;
            throwError?.Invoke();
            throwWrongAnswer?.Invoke();
        }
    }

    public static void Shuffle<T>(IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void GreenButtonActivated()
    {
        //activatedButton = calcul1;
        activatedButton = float.Parse(greenButton.text);
    }
    public void YellowButtonActivated()
    {
        //activatedButton = calcul2;
        activatedButton = float.Parse(yellowButton.text);
    }
    public void RedButtonActivated()
    {
        //activatedButton = calcul3;
        activatedButton = float.Parse(redButton.text);
    }

    public void triggerStart()
    {
        CalculStart();
    }
}




