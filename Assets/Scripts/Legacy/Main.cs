using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// was used as a menu to choose between a tutorial and full game fot triple task
// is not used anymore


public class Main : MonoBehaviour
{
    public GameObject scriptcontainer;
    static public GameObject vrrig;

    // get the scripts
    public GameObject Audio;
    public GameObject Lacet;
    public GameObject Calcul;
    static private AudioInteractable sAudio;
    static private lacetInteraction sLacet;
    static private Calculomatic sCalcul;

    static private GestionMiniJeux scriptTuto;

    private void Start()
    {
        scriptTuto = scriptcontainer.GetComponent<GestionMiniJeux>();
        scriptTuto.enabled = false;
        vrrig.SetActive(false);
        sAudio = Audio.GetComponent<AudioInteractable>();
        sLacet = Lacet.GetComponent<lacetInteraction>();
        sCalcul = Calcul.GetComponent<Calculomatic>();
        sAudio.enabled = false;
        sLacet.enabled = false;
        sCalcul.enabled = false;
    }

    // buttons for choosing tutorial of full game
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 150, 150));

        StartButtons();

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("tuto")) LaunchTuto();
        if (GUILayout.Button("full game")) Normal();

    }
    static void LaunchTuto()
    {
        scriptTuto.enabled = true;
    }
    static void Normal()
    {
        vrrig.SetActive(true);
        sAudio.enabled = true;
        sLacet.enabled = true;
        sCalcul.enabled = true;
    }
}



