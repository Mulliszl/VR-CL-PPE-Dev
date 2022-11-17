using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Security.Cryptography;

public class PromptInteraction : MonoBehaviour
{
    //get shapes
    public GameObject carre;
    public GameObject rond;
    public GameObject triangle;
    public GameObject hegagone;

    //get buttons
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;

    //get texts
    public Text instructions;

    //get materials
    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;


    private MeshRenderer mesh1;
    private MeshRenderer mesh2;
    private MeshRenderer mesh3;
    private MeshRenderer mesh4;
    private Text text1;
    private Text text2;
    private Text text3;
    private Text text4;

    public bool debug = false;

    public UnityEvent throwError;
    public UnityEvent triggerStart;

    private int mainQ, secondaryQ;
    private bool endRemember;
    private int questionType;

    private GameObject rightAnswerShape;
    private Material rightAnswerColor;
    private string rightAnswerLetter;
    private GameObject guessedAnswerShape;
    private Material guessedAnswerColor;
    private string guessedAnswerLetter;


    private List<GameObject> shapes;
    private List<Material> colors;
    private List<string> letters;

    private void Start()
    {
        shapes = new List<GameObject> { carre,rond,triangle,hegagone };
        colors = new List<Material> { material1, material2, material3, material4 };
        letters = new List<string> { "A", "B", "C", "D" };

        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);

        if (debug)
        {
            Debug.Log(shapes[0].name + shapes[1].name + shapes[2].name + shapes[3].name);
        }

        mesh1 = shapes[0].GetComponentInChildren<MeshRenderer>();
        mesh2 = shapes[1].GetComponentInChildren<MeshRenderer>();
        mesh3 = shapes[2].GetComponentInChildren<MeshRenderer>();
        mesh4 = shapes[3].GetComponentInChildren<MeshRenderer>();
        text1 = shapes[0].GetComponentInChildren<Text>();
        text2 = shapes[1].GetComponentInChildren<Text>();
        text3 = shapes[2].GetComponentInChildren<Text>();
        text4 = shapes[3].GetComponentInChildren<Text>();

        TriggerStart();
    }

    public void TriggerStart()
    {
        Melange();
        Place();
        instructions.text = "Vous avez 20 secondes pour mémoriser le schéma";
        Active();
        StartCoroutine(remember());
    }

    private void Melange()
    {
        Shuffle<GameObject>(shapes);
        Shuffle<Material>(colors);
        Shuffle<string>(letters);
    }

    private void Place()
    {
        PlaceShapes();
        PlaceColors();
        PlaceLetters();
        if (debug)
        {
            Debug.Log(colors[0].name);
            Debug.Log(colors[1].name);
            Debug.Log(colors[2].name);
            Debug.Log(colors[3].name);
        }

    }

    private void Active()
    {
        carre.SetActive(true);
        rond.SetActive(true);
        triangle.SetActive(true);
        hegagone.SetActive(true);
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
    }

    private void PlaceShapes()
    {
        shapes[0].transform.localPosition = new Vector3(0.5f, 0, 0);
        shapes[1].transform.localPosition = new Vector3(0.18f, 0, 0);
        shapes[2].transform.localPosition = new Vector3(-0.12f, 0, 0);
        shapes[3].transform.localPosition = new Vector3(-0.42f, 0, 0);
    }

    private void PlaceColors()
    {
        /*
        shapes[0].GetComponent<MeshRenderer>().material = colors[0];
        shapes[1].GetComponent<MeshRenderer>().material = colors[1];
        shapes[2].GetComponent<MeshRenderer>().material = colors[2];
        shapes[3].GetComponent<MeshRenderer>().material = colors[3];
        Debug.Log( shapes[0].GetComponentInChildren<MeshRenderer>().gameObject.name);
        shapes[0].GetComponentInChildren<MeshRenderer>().material = colors[0];
        Debug.Log(shapes[1].GetComponentInChildren<MeshRenderer>().gameObject.name);
        shapes[1].GetComponentInChildren<MeshRenderer>().material = colors[1];
        Debug.Log(shapes[2].GetComponentInChildren<MeshRenderer>().gameObject.name);
        shapes[2].GetComponentInChildren<MeshRenderer>().material = colors[2];
        Debug.Log(shapes[3].GetComponentInChildren<MeshRenderer>().gameObject.name);
        shapes[3].GetComponentInChildren<MeshRenderer>().material = colors[3];
        */

        mesh1.material = colors[0];
        mesh2.material = colors[1];
        mesh3.material = colors[2];
        mesh4.material = colors[3];

    }

    private void PlaceLetters()
    {
        text1.text = letters[0];
        text2.text = letters[1];
        text3.text = letters[2];
        text4.text = letters[3];
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

    private void ChooseQuestion()
    {
        mainQ = Random.Range(0, 3);
        secondaryQ = Random.Range(0, 4);
        if (mainQ == 0)
        {//forme
            questionType = 0;
            if (secondaryQ == 0)
            {//carré
                instructions.text = "où était le carré ?";
                rightAnswerShape = carre;
        } else if (secondaryQ == 1)
            {//rond
                instructions.text = "où était le rond ?";
                rightAnswerShape = rond;
            } else if (secondaryQ == 2)
            {//triangle
                instructions.text = "où était le triangle ?";
                rightAnswerShape = triangle;
            } else if (secondaryQ == 1)
            {//hexagone
                instructions.text = "où était l'hexagone ?";
                rightAnswerShape = hegagone;
            }
        } else if (mainQ == 1)
        {//couleur
            questionType = 1;
            if (secondaryQ == 0)
            {//vert
                instructions.text = "où était la forme verte ?";
                rightAnswerColor = material1;
            }
            else if (secondaryQ == 1)
            {//jaune
                instructions.text = "où était la forme jaune ?";
                rightAnswerColor = material2;
            }
            else if (secondaryQ == 2)
            {//rouge
                instructions.text = "où était la forme rouge ?";
                rightAnswerColor = material3;
            }
            else if (secondaryQ == 1)
            {//noir
                instructions.text = "où était la forme noire ?";
                rightAnswerColor = material4;
            }
        } else if (mainQ == 1)
        {//lettre
            questionType = 2;
            if (secondaryQ == 0)
            {//A
                instructions.text = "où était le A ?";
                rightAnswerLetter = "A";
            }
            else if (secondaryQ == 1)
            {//B
                instructions.text = "où était le B ?";
                rightAnswerLetter = "B";
            }
            else if (secondaryQ == 2)
            {//C
                instructions.text = "où était le C ?";
                rightAnswerLetter = "C";
            }
            else if (secondaryQ == 1)
            {//D
                instructions.text = "où était le D ?";
                rightAnswerLetter = "D";
            }
        }
    }

    private void guessedbutton()
    {
        if (mainQ == 0)
        {//forme
            if (secondaryQ == 0)
            {//carré
                instructions.text = "où était le carré ?";
                guessedAnswerShape = carre;
            }
            else if (secondaryQ == 1)
            {//rond
                instructions.text = "où était le rond ?";
                rightAnswerShape = rond;
            }
            else if (secondaryQ == 2)
            {//triangle
                instructions.text = "où était le triangle ?";
                rightAnswerShape = triangle;
            }
            else if (secondaryQ == 1)
            {//hexagone
                instructions.text = "où était l'hexagone ?";
                rightAnswerShape = hegagone;
            }
        }
        else if (mainQ == 1)
        {//couleur
            if (secondaryQ == 0)
            {//vert
                instructions.text = "où était la forme verte ?";
                rightAnswerColor = material1;
            }
            else if (secondaryQ == 1)
            {//jaune
                instructions.text = "où était la forme jaune ?";
                rightAnswerColor = material2;
            }
            else if (secondaryQ == 2)
            {//rouge
                instructions.text = "où était la forme rouge ?";
                rightAnswerColor = material3;
            }
            else if (secondaryQ == 1)
            {//noir
                instructions.text = "où était la forme noire ?";
                rightAnswerColor = material4;
            }
        }
        else if (mainQ == 1)
        {//lettre
            if (secondaryQ == 0)
            {//A
                instructions.text = "où était le A ?";
                rightAnswerLetter = "A";
            }
            else if (secondaryQ == 1)
            {//B
                instructions.text = "où était le B ?";
                rightAnswerLetter = "B";
            }
            else if (secondaryQ == 2)
            {//C
                instructions.text = "où était le C ?";
                rightAnswerLetter = "C";
            }
            else if (secondaryQ == 1)
            {//D
                instructions.text = "où était le D ?";
                rightAnswerLetter = "D";
            }
        }
    }

    private void verification()
    {
        if(questionType == 0)
        {
            if(guessedAnswerShape == rightAnswerShape)
            {
                //Debug.Log("gagne");
                //Debug.Log(guessedAnswerShape.name);
                //Debug.Log(rightAnswerShape.name);
                //win = true;
                triggerStart?.Invoke();
            }
            else
            {
                //Debug.Log("perdu");
                //Debug.Log(guessedAnswerShape.name);
                //Debug.Log(rightAnswerShape.name);
                //win = false;
                throwError?.Invoke();
            }
        }
        else if (questionType == 1)
        {
            if (guessedAnswerColor == rightAnswerColor)
            {
                //Debug.Log("gagne");
                triggerStart?.Invoke();
            }
            else
            {
                //Debug.Log("perdu");
                //win = false;
                throwError?.Invoke();
            }
        }
        else if (questionType == 2)
        {
            if (guessedAnswerLetter == rightAnswerLetter)
            {
                //Debug.Log("gagne");
                //win = true;
                triggerStart?.Invoke();
            }
            else
            {
                //Debug.Log("perdu");
                //win = false;
                throwError?.Invoke();
            }
            
        }
    }

    IEnumerator remember()
    {
        yield return new WaitForSeconds(20);
        ChooseQuestion();
        carre.SetActive(false);
        rond.SetActive(false);
        triangle.SetActive(false);
        hegagone.SetActive(false);
        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        button4.SetActive(true);
    }


    public void FirstButtonPressed()
    {
        guessedAnswerShape = shapes[0];
        guessedAnswerColor = colors[0];
        guessedAnswerLetter = letters[0];
        verification();
    }
    public void SecondButtonPressed()
    {
        guessedAnswerShape = shapes[1];
        guessedAnswerColor = colors[1];
        guessedAnswerLetter = letters[1];
        verification();
    }
    public void thirdButtonPressed()
    {
        guessedAnswerShape = shapes[2];
        guessedAnswerColor = colors[2];
        guessedAnswerLetter = letters[2];
        verification();
    }
    public void FourthButtonPressed()
    {
        guessedAnswerShape = shapes[3];
        guessedAnswerColor = colors[3];
        guessedAnswerLetter = letters[3];
        verification();
    }
}
