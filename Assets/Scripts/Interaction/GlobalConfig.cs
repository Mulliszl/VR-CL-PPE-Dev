using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

// this script load ans store the parameters from the config file across all the scenes


public static class GlobalConfig 
{
    //scene
    public static string type;

    // calcul
    public static int experienceTime;
    public static int pauseTime;
    public static bool reverse;
    public static bool scale;
    public static bool skipTuto;
    public static int AudioStimulusAverage;
    public static int AudioStimulusDelta;
    public static float MaximumResponseTime;

    //user
    public static string name;
    public static int age;
    public static string health;

    public static void initialize()
    {
        INIParser ini = new INIParser();
        // generate the ini file if it is not at the right place
        if (!File.Exists(Application.persistentDataPath + "/config.txt"))
        {
                using (StreamWriter sw = File.CreateText(Application.persistentDataPath + "/config.txt"))
                {
                    sw.WriteLine("[userid]");
                    sw.WriteLine("name = No Name Set");
                    sw.WriteLine("age = 0");
                    sw.WriteLine("health = Unknown\n");
                    sw.WriteLine("[experiment]");
                    sw.WriteLine("; choosed scene -calcul - doubleTask - tripleTaskNoInteruption - tripleTaskNASAtlx");
                    sw.WriteLine("type = calcul\n");
                    sw.WriteLine("; if true begin the hard way true = 1 false = 0");
                    sw.WriteLine("reverse = 0\n");
                    sw.WriteLine("; phase time in seconds");
                    sw.WriteLine("experienceTime = 180\n");
                    sw.WriteLine("; pause time in seconds");
                    sw.WriteLine("pauseTime = 30\n");
                    sw.WriteLine("; if true propose a survey to evaluate cognitive load true = 1 false = 0");
                    sw.WriteLine("survey = 1\n"); 
                    sw.WriteLine("; allow to skip the tutorial may cuse saving issues on cognitive load data true = 1 false = 0");
                    sw.WriteLine("skipTuto = 0\n");
                    sw.WriteLine("; set the repetition rate for the audio stimulus");
                    sw.WriteLine("AudioStimulusAverage = 14\n");
                    sw.WriteLine("; set the maximus deviation from the average in seconds");
                    sw.WriteLine("AudioStimulusDelta = 3\n");
                    sw.WriteLine("; set maximum response time (time saved if there is no response from the user)");
                    sw.WriteLine("MaximumResponseTime = 10\n");
            }
        }
        ini.Open(Application.persistentDataPath + "/config.txt");

        // store the parameters into variables

        type = ini.ReadValue("experiment", "type", "noscene");

        name = ini.ReadValue("userid", "name", "noname");
        age = ini.ReadValue("userid", "age", 0);
        health = ini.ReadValue("userid", "health", "unknown");

        experienceTime = ini.ReadValue("experiment", "experienceTime", 180);
        pauseTime = ini.ReadValue("experiment", "pauseTime", 30);
        reverse = ini.ReadValue("experiment", "reverse", false);
        scale = ini.ReadValue("experiment", "survey", false);
        skipTuto = ini.ReadValue("experiment", "skipTuto", false);
        AudioStimulusAverage = ini.ReadValue("experiment", "AudioStimulusAverage", 14);
        AudioStimulusDelta = ini.ReadValue("experiment", "AudioStimulusDelta", 3);
        MaximumResponseTime = ini.ReadValue("experiment", "MaximumResponseTime", 10);
        ini.Close();

        // select the scene from INI file
        if (type == "calcul")
        {
            SceneManager.LoadScene("Cognitive-load-static-math");
        }
        else if (type == "tripleTaskNASAtlx ")
        {
            SceneManager.LoadScene("Cognitive-load-static-full-game-nasa-tlx");
        } 
        else if (type == "tripleTaskNoInteruption")
        {
            SceneManager.LoadScene("Cognitive-load-static-full-game");
        }
        else if (type == "doubleTask")
        {
            SceneManager.LoadScene("TripleTaskCalcul");
        }  
    }
}
