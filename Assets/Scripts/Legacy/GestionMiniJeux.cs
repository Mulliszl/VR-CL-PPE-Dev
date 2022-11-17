using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// was used to control mini games in triple task (activate and deactivate others scripts) in the tutorial scene
// not used anymore

public class GestionMiniJeux : MonoBehaviour
{
    public GameObject Audio;
    public GameObject Lacet;
    public GameObject Calcul;

    public GameObject bReady;

    public Text tInstructions;

    private AudioInteractable sAudio;
    private lacetInteraction sLacet;
    private Calculomatic sCalcul;

    private int iteration;

    
    // Start is called before the first frame update
    public void Start()
    {
        sAudio = Audio.GetComponent<AudioInteractable>();
        sLacet = Lacet.GetComponent<lacetInteraction>();
        sCalcul = Calcul.GetComponent<Calculomatic>();
        sAudio.enabled = false;
        sLacet.enabled = false;
        sCalcul.enabled = false;
        bReady.SetActive(false);
        Step1();
    }


    private void Step1()
    {
        //begin with sound
        sAudio.enabled = true;
        tInstructions.enabled = true;
        tInstructions.text = "jeu actif <Audio>\nobjectif : donner la direction\ndu son grace au joystick";

    }

    public void Step2()
    {
        sAudio.enabled = false;
        sCalcul.enabled = true;
        tInstructions.text = "jeu actif <Calcul>\nobjectif : choisir un réponse\npour le calcul proposé";
    }

    public void Step3()
    {
        sCalcul.enabled = false;
        sLacet.enabled = true;
        tInstructions.text = "jeu actif <Stabilisation>\nobjectif : à l'aide des leviers\nmaintenez la position de l'image\nà l'équilibre";
    }  
    
    public void Step4()
    {
        sAudio.enabled = true;
        tInstructions.text = "jeux actif \n<Stabilisation + Audio>\nessayez les deux en meme temps";
    }

    public void Step5()
    {
        sCalcul.enabled = true;
        tInstructions.text = "jeux actif <tous>\nBonne chance";
        
    }

    public void Step6()
    {
        bReady.SetActive(true);
        tInstructions.text = "jeux actif <tous>\nquand vous serez pret\nappuyez sur le bouton ready\npour passer au jeu complet";
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene("Cognitive-load-static-full-game");
    }
}
