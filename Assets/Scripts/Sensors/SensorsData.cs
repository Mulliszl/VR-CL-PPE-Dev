using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using HP.Omnicept;
using HP.Omnicept.Messaging;
using HP.Omnicept.Unity;
using HP.Omnicept.Messaging.Messages;
using CsvHelper;
using System.IO;
using System.Globalization;
using TMPro;

//Defines the columns struct of csv file saved on the scene MyCsvDouble
public class MyCsvDouble
{


    public string Timestamp { get; set; }
    public string Stage { get; set; }
    public string ResponseTime { get; set; }
    public string Stabilisation { get; set; }
    public string AnswerMath { get; set; }

    public string HeartRate { get; set; }

    public string HeartRateVariabilitySdnn { get; set; }
    public string HeartRateVariabilityRmssd { get; set; }

    public string LeftEyeGazeX { get; set; }
    public string LeftEyeGazeY { get; set; }
    public string LeftEyeGazeZ { get; set; }
    public string LeftEyeGazeConfidence { get; set; }
    public string LeftEyeOpenness { get; set; }
    public string LeftEyeOpennessConfidence { get; set; }
    public string LeftEyePupilDilation { get; set; }
    public string LeftEyePupilDilationConfidence { get; set; }
    public string LeftEyePupilPositionX { get; set; }
    public string LeftEyePupilPositionY { get; set; }

    public string RightEyeGazeX { get; set; }
    public string RightEyeGazeY { get; set; }
    public string RightEyeGazeZ { get; set; }
    public string RightEyeGazeConfidence { get; set; }
    public string RightEyeOpenness { get; set; }
    public string RightEyeOpennessConfidence { get; set; }
    public string RightEyePupilDilation { get; set; }
    public string RightEyePupilDilationConfidence { get; set; }
    public string RightEyePupilPositionX { get; set; }
    public string RightEyePupilPositionY { get; set; }


    public string CombinedGazeX { get; set; }
    public string CombinedGazeY { get; set; }
    public string CombinedGazeZ { get; set; }
    public string CombinedGazeConfidence { get; set; }

    public string CognitiveLoadValue { get; set; }
    public string CognitiveLoadStandardDeviation { get; set; }
}

//Defines the columns struct of csv file saved on the scene MyCsvTriple
public class MyCsvTriple
{
   

    public string Timestamp { get; set; }
    public string Stage { get; set; }
    public string TaskAudio { get; set; }
    public string AnswerAudio { get; set; }
    public string TaskMath { get; set; }
    public string AnswerMath { get; set; }
    public string Stabilization { get; set; }

    public string HeartRate { get; set; }

	public string HeartRateVariabilitySdnn { get; set; }
	public string HeartRateVariabilityRmssd { get; set; }

    public string LeftEyeGazeX { get; set; }
    public string LeftEyeGazeY { get; set; }
    public string LeftEyeGazeZ { get; set; }
    public string LeftEyeGazeConfidence { get; set; }
    public string LeftEyeOpenness { get; set; }
    public string LeftEyeOpennessConfidence { get; set; }
    public string LeftEyePupilDilation { get; set; }
    public string LeftEyePupilDilationConfidence { get; set; }
    public string LeftEyePupilPositionX { get; set; }
    public string LeftEyePupilPositionY { get; set; }

    public string RightEyeGazeX { get; set; }
    public string RightEyeGazeY { get; set; }
    public string RightEyeGazeZ { get; set; }
    public string RightEyeGazeConfidence { get; set; }
    public string RightEyeOpenness { get; set; }
    public string RightEyeOpennessConfidence { get; set; }
    public string RightEyePupilDilation { get; set; }
    public string RightEyePupilDilationConfidence { get; set; }
    public string RightEyePupilPositionX { get; set; }
    public string RightEyePupilPositionY { get; set; }


    public string CombinedGazeX { get; set; }
    public string CombinedGazeY { get; set; }
    public string CombinedGazeZ { get; set; }
    public string CombinedGazeConfidence { get; set; }

    public string CognitiveLoadValue { get; set; }
    public string CognitiveLoadStandardDeviation { get; set; }
}

//Defines the columns struct of csv file saved on the scene MyCsvCalcul
public class MyCsvCalcul
{


    public string Timestamp { get; set; }
    public string Stage { get; set; }
    public string ResponseTime { get; set; }
    public string AnswerMath { get; set; }

    public string HeartRate { get; set; }

    public string HeartRateVariabilitySdnn { get; set; }
    public string HeartRateVariabilityRmssd { get; set; }

    public string LeftEyeGazeX { get; set; }
    public string LeftEyeGazeY { get; set; }
    public string LeftEyeGazeZ { get; set; }
    public string LeftEyeGazeConfidence { get; set; }
    public string LeftEyeOpenness { get; set; }
    public string LeftEyeOpennessConfidence { get; set; }
    public string LeftEyePupilDilation { get; set; }
    public string LeftEyePupilDilationConfidence { get; set; }
    public string LeftEyePupilPositionX { get; set; }
    public string LeftEyePupilPositionY { get; set; }

    public string RightEyeGazeX { get; set; }
    public string RightEyeGazeY { get; set; }
    public string RightEyeGazeZ { get; set; }
    public string RightEyeGazeConfidence { get; set; }
    public string RightEyeOpenness { get; set; }
    public string RightEyeOpennessConfidence { get; set; }
    public string RightEyePupilDilation { get; set; }
    public string RightEyePupilDilationConfidence { get; set; }
    public string RightEyePupilPositionX { get; set; }
    public string RightEyePupilPositionY { get; set; }


    public string CombinedGazeX { get; set; }
    public string CombinedGazeY { get; set; }
    public string CombinedGazeZ { get; set; }
    public string CombinedGazeConfidence { get; set; }

    public string CognitiveLoadValue { get; set; }
    public string CognitiveLoadStandardDeviation { get; set; }
}


public class SensorsData : MonoBehaviour
{

    //To verify if a CSV has to be created
    [SerializeField] public bool WriteCSV;

    //Scene Stage Attribute
    public string Stage { get; set; }

    //Tasks Performance Attributes
    public string ResponseTime { get; set; }
    public string TaskAudio { get; set; }
    public string AnswerAudio { get; set; }
    public string TaskMath { get; set; }
    public string AnswerMath { get; set; }

    public string Stabilization { get; set; }

    //Sensors Attributes
    private string Timestamp;
    private string HeartRate;

	private string HeartRateVariabilitySdnn;
	private string HeartRateVariabilityRmssd;

    private string LeftEyeGazeX;
    private string LeftEyeGazeY;
    private string LeftEyeGazeZ;
    private string LeftEyeGazeConfidence;
    private string LeftEyeOpenness;
    private string LeftEyeOpennessConfidence;
    private string LeftEyePupilDilation;
    private string LeftEyePupilDilationConfidence;
    private string LeftEyePupilPositionX;
    private string LeftEyePupilPositionY;


    private string RightEyeGazeX;
    private string RightEyeGazeY;
    private string RightEyeGazeZ;
    private string RightEyeGazeConfidence;
    private string RightEyeOpenness;
    private string RightEyeOpennessConfidence;
    private string RightEyePupilDilation;
    private string RightEyePupilDilationConfidence;
    private string RightEyePupilPositionX;
    private string RightEyePupilPositionY;

    private string CombinedGazeX;
    private string CombinedGazeY;
    private string CombinedGazeZ;
    private string CombinedGazeConfidence;

    private string CognitiveLoadValue;
    private string CognitiveLoadStandardDeviation;

    // Get average of cognitive load
    [HideInInspector]
    public List<float> simpleTask;
    [HideInInspector]
    public List<float> complexTask;
    private int go = 0;


    //Path to save CSV
    private string path;

    //GLIA
    private GliaBehaviour _gliaBehaviour;
    protected GliaBehaviour gliaBehaviour
    {
        get
        {
            if (_gliaBehaviour == null)
            {
                _gliaBehaviour = FindObjectOfType<GliaBehaviour>();
            }
            return _gliaBehaviour;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        if (true)
        {
            //Create CSV file
            string now = Convert.ToString(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\SensorsCSVData\" + GlobalConfig.name);
            path = Directory.GetCurrentDirectory() + @"\SensorsCSVData\" + GlobalConfig.name + @"\" + GlobalConfig.type + "-" + now + ".csv";
            Debug.Log(path);

            //Add CSV Header
            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = true,
                Delimiter = ";",
            };
            using (FileStream fs = File.Create(path));
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, config))
            {   

                //Verify the task and write the respective header
                if(GlobalConfig.type == "calcul")
                    csv.WriteHeader<MyCsvCalcul>();
                else if (GlobalConfig.type == "doubleTask")
                    csv.WriteHeader<MyCsvDouble>();
                else
                    csv.WriteHeader<MyCsvTriple>();
                writer.Flush();
            }

            // Write a new line to the csv to separate the header and the first data line
            using (var stream = File.Open(path, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine();
            }


            //Defines the Periodic Function called to write in the csv file
            if(GlobalConfig.type == "calcul")
            {
                InvokeRepeating("WriteSensorsDataCalcul", 0f, 0.1f);
            }
            else if (GlobalConfig.type == "doubleTask")
            {
                InvokeRepeating("WriteSensorsDataDouble", 0f, 0.1f);
            }
            else
            {
                InvokeRepeating("WriteSensorsDataTriple", 0f, 0.1f);
            }
        }

        //Sensors' listeners
        gliaBehaviour.OnHeartRate.AddListener(OnHeartRate);
        gliaBehaviour.OnEyeTracking.AddListener(OnEyeTracking);
        gliaBehaviour.OnCognitiveLoad.AddListener(OnCognitiveLoad);
        gliaBehaviour.OnHeartRateVariability.AddListener(OnHeartRateVariability);
    }

    //Get the HeartRate data
    private void OnHeartRate(HeartRate hr)
    {
        if (hr != null)
        {
            HeartRate = hr.Rate.ToString();
        }
        else
        {
            HeartRate = null;
        }
    }

    //Get the HeartRateVariability data
    private void OnHeartRateVariability(HeartRateVariability hrv)
    {
        if (hrv != null)
        {
            HeartRateVariabilitySdnn = hrv.Sdnn.ToString();
			HeartRateVariabilityRmssd = hrv.Rmssd.ToString();
        }
        else
        {
			HeartRateVariabilitySdnn = null;
			HeartRateVariabilityRmssd = null;
        }
    }

    //Get the CognitiveLoad data
    private void OnCognitiveLoad(CognitiveLoad cl)
    {
        if (cl != null)
        {
            CognitiveLoadValue = cl.CognitiveLoadValue.ToString();
            CognitiveLoadStandardDeviation = cl.StandardDeviation.ToString();

            if (Stage == "Partie 1")
                go = 1;
            else if (Stage == "Pause")
                go = 2;
            else if (Stage == "Partie 2")
                go = 3;
            else if (Stage == "FIN")
                go = 4;

            if (go == 1)
            {
                Debug.Log("go = " + go);
                simpleTask.Add(cl.CognitiveLoadValue);
            }
            else if (go == 3)
            {
                complexTask.Add(cl.CognitiveLoadValue);
            }
            
        }
        else
        {
            CognitiveLoadValue = null;
            CognitiveLoadStandardDeviation = null;
        }
    }

    //Get the EyeTracking data
    private void OnEyeTracking(EyeTracking et)
    {
        if (et != null)
        {

            LeftEyeGazeX = et.LeftEye.Gaze.X.ToString();
            LeftEyeGazeY = et.LeftEye.Gaze.Y.ToString();
            LeftEyeGazeZ = et.LeftEye.Gaze.Z.ToString();
            LeftEyeGazeConfidence = et.LeftEye.Gaze.Confidence.ToString();
            LeftEyeOpenness = et.LeftEye.Openness.ToString();
            LeftEyeOpennessConfidence = et.LeftEye.OpennessConfidence.ToString();
            LeftEyePupilDilation = et.LeftEye.PupilDilation.ToString();
            LeftEyePupilDilationConfidence = et.LeftEye.PupilDilationConfidence.ToString();
            LeftEyePupilPositionX = et.LeftEye.PupilPosition.X.ToString();
            LeftEyePupilPositionY = et.LeftEye.PupilPosition.Y.ToString();

            RightEyeGazeX = et.RightEye.Gaze.X.ToString();
            RightEyeGazeY = et.RightEye.Gaze.Y.ToString();
            RightEyeGazeZ = et.RightEye.Gaze.Z.ToString();
            RightEyeGazeConfidence = et.RightEye.Gaze.Confidence.ToString();
            RightEyeOpenness = et.RightEye.Openness.ToString();
            RightEyeOpennessConfidence = et.RightEye.OpennessConfidence.ToString();
            RightEyePupilDilation = et.RightEye.PupilDilation.ToString();
            RightEyePupilDilationConfidence = et.RightEye.PupilDilationConfidence.ToString();
            RightEyePupilPositionX = et.RightEye.PupilPosition.X.ToString();
            RightEyePupilPositionY = et.RightEye.PupilPosition.Y.ToString();

            CombinedGazeX = et.CombinedGaze.X.ToString();
            CombinedGazeY = et.CombinedGaze.Y.ToString();
            CombinedGazeZ = et.CombinedGaze.Z.ToString();
            CombinedGazeConfidence = et.CombinedGaze.Confidence.ToString();

        }
        else
        {
            LeftEyeGazeX = null;
            LeftEyeGazeY = null;
            LeftEyeGazeZ = null;
            LeftEyeGazeConfidence = null;
            LeftEyeOpenness = null;
            LeftEyeOpennessConfidence = null;
            LeftEyePupilDilation = null;
            LeftEyePupilDilationConfidence = null;
            LeftEyePupilPositionX = null;
            LeftEyePupilPositionY = null;


            RightEyeGazeX = null;
            RightEyeGazeY = null;
            RightEyeGazeZ = null;
            RightEyeGazeConfidence = null;
            RightEyeOpenness = null;
            RightEyeOpennessConfidence = null;
            RightEyePupilDilation = null;
            RightEyePupilDilationConfidence = null;
            RightEyePupilPositionX = null;
            RightEyePupilPositionY = null;

            CombinedGazeX = null;
            CombinedGazeY = null;
            CombinedGazeZ = null;
            CombinedGazeConfidence = null;
        }
    }

    public void SetStage(string stage)
    {
        Stage = stage;
    }

    public void SetResponseTime(string rTime)
    {
        ResponseTime = rTime;
    }

    //Write the data in csv
    private void WriteSensorsDataDouble()
    {

        //Get the timestamp 
        Timestamp = Convert.ToString(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        
        //Create a list of MyCsv to add to the file
        var data = new List<MyCsvDouble>
        {

            //Instantiate class filling the attributes values 
            new MyCsvDouble
            {

                Timestamp = Timestamp ,
                Stage = Stage,
                ResponseTime = ResponseTime,
                Stabilisation = Stabilization,
                AnswerMath = AnswerMath,
                HeartRate = HeartRate,
                HeartRateVariabilitySdnn = HeartRateVariabilitySdnn,
                HeartRateVariabilityRmssd = HeartRateVariabilityRmssd,
                LeftEyeGazeX = LeftEyeGazeX,
                LeftEyeGazeY = LeftEyeGazeY,
                LeftEyeGazeZ = LeftEyeGazeZ,
                LeftEyeGazeConfidence = LeftEyeGazeConfidence,
                LeftEyeOpenness = LeftEyeOpenness,
                LeftEyeOpennessConfidence = LeftEyeOpennessConfidence,
                LeftEyePupilDilation = LeftEyePupilDilation,
                LeftEyePupilDilationConfidence = LeftEyePupilDilationConfidence,
                LeftEyePupilPositionX = LeftEyePupilPositionX,
                LeftEyePupilPositionY = LeftEyePupilPositionY,
                RightEyeGazeX = RightEyeGazeX,
                RightEyeGazeY = RightEyeGazeY,
                RightEyeGazeZ = RightEyeGazeZ,
                RightEyeGazeConfidence = RightEyeGazeConfidence,
                RightEyeOpenness = RightEyeOpenness,
                RightEyeOpennessConfidence = RightEyeOpennessConfidence,
                RightEyePupilDilation = RightEyePupilDilation,
                RightEyePupilDilationConfidence = RightEyePupilDilationConfidence,
                RightEyePupilPositionX = RightEyePupilPositionX,
                RightEyePupilPositionY = RightEyePupilPositionY,
                CombinedGazeX = CombinedGazeX,
                CombinedGazeY = CombinedGazeY,
                CombinedGazeZ = CombinedGazeZ,
                CombinedGazeConfidence = CombinedGazeConfidence,
                CognitiveLoadValue = CognitiveLoadValue,
                CognitiveLoadStandardDeviation = CognitiveLoadStandardDeviation,


            },
        };

        // Append a new line to the file.
        var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
            HasHeaderRecord = false,
            Delimiter = ";",

        };
        using (var stream = File.Open(path, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(data);
            writer.Flush();
        }

        // Clean the attributes values after write
        if (AnswerMath != " ")
        {
            AnswerMath = " ";
        }

        if (ResponseTime != " ")
        {
            ResponseTime = " ";
        }

        if (Stabilization != " ")
        {
            Stabilization = " ";
        }

    }
    private void WriteSensorsDataTriple()
    {
        //Get the timestamp 
        Timestamp = Convert.ToString(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        
        //Create a list of MyCsv to add to the file
        var data = new List<MyCsvTriple>
        {
            //Instantiate class filling the attributes values 
            new MyCsvTriple
            {

                Timestamp = Timestamp ,
                Stage = Stage,
                TaskAudio = TaskAudio,
                AnswerAudio = AnswerAudio,
                TaskMath = TaskMath,
                AnswerMath = AnswerMath,
                Stabilization = Stabilization,
                HeartRate = HeartRate,
				HeartRateVariabilitySdnn = HeartRateVariabilitySdnn,
				HeartRateVariabilityRmssd = HeartRateVariabilityRmssd,
                LeftEyeGazeX = LeftEyeGazeX,
                LeftEyeGazeY = LeftEyeGazeY,
                LeftEyeGazeZ = LeftEyeGazeZ,
                LeftEyeGazeConfidence = LeftEyeGazeConfidence,
                LeftEyeOpenness = LeftEyeOpenness,
                LeftEyeOpennessConfidence = LeftEyeOpennessConfidence,
                LeftEyePupilDilation = LeftEyePupilDilation,
                LeftEyePupilDilationConfidence = LeftEyePupilDilationConfidence,
                LeftEyePupilPositionX = LeftEyePupilPositionX,
                LeftEyePupilPositionY = LeftEyePupilPositionY,
                RightEyeGazeX = RightEyeGazeX,
                RightEyeGazeY = RightEyeGazeY,
                RightEyeGazeZ = RightEyeGazeZ,
                RightEyeGazeConfidence = RightEyeGazeConfidence,
                RightEyeOpenness = RightEyeOpenness,
                RightEyeOpennessConfidence = RightEyeOpennessConfidence,
                RightEyePupilDilation = RightEyePupilDilation,
                RightEyePupilDilationConfidence = RightEyePupilDilationConfidence,
                RightEyePupilPositionX = RightEyePupilPositionX,
                RightEyePupilPositionY = RightEyePupilPositionY,
                CombinedGazeX = CombinedGazeX,
                CombinedGazeY = CombinedGazeY,
                CombinedGazeZ = CombinedGazeZ,
                CombinedGazeConfidence = CombinedGazeConfidence,
                CognitiveLoadValue = CognitiveLoadValue,
                CognitiveLoadStandardDeviation = CognitiveLoadStandardDeviation,
                

            },
        };

        // Append a new line to the file.
        var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
            HasHeaderRecord = false,
            Delimiter = ";",

        };
        using (var stream = File.Open(path, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(data);
            writer.Flush();
        }

        // Clean the attributes values after write
        if (TaskAudio != " ")
        {
            TaskAudio = " ";
        }

        if (TaskMath != " ")
        {
            TaskMath = " ";
        }
        if (AnswerAudio != " ")
        {
            AnswerAudio = " ";
        }

        if (AnswerMath != " ")
        {
            AnswerMath = " ";
        }

        if (Stabilization != " ")
        {
            Stabilization = " ";
        }

        if(Stage != " ")
        {
            Stage = " ";
        }
    }

    private void WriteSensorsDataCalcul()
    {
        //Get the timestamp 
        Timestamp = Convert.ToString(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        //Create a list of MyCsv to add to the file
        var data = new List<MyCsvCalcul>
        {
            //Instantiate class filling the attributes values 
            new MyCsvCalcul
            {

                Timestamp = Timestamp ,
                Stage = Stage,
                ResponseTime = ResponseTime,
                AnswerMath = AnswerMath,
                HeartRate = HeartRate,
                HeartRateVariabilitySdnn = HeartRateVariabilitySdnn,
                HeartRateVariabilityRmssd = HeartRateVariabilityRmssd,
                LeftEyeGazeX = LeftEyeGazeX,
                LeftEyeGazeY = LeftEyeGazeY,
                LeftEyeGazeZ = LeftEyeGazeZ,
                LeftEyeGazeConfidence = LeftEyeGazeConfidence,
                LeftEyeOpenness = LeftEyeOpenness,
                LeftEyeOpennessConfidence = LeftEyeOpennessConfidence,
                LeftEyePupilDilation = LeftEyePupilDilation,
                LeftEyePupilDilationConfidence = LeftEyePupilDilationConfidence,
                LeftEyePupilPositionX = LeftEyePupilPositionX,
                LeftEyePupilPositionY = LeftEyePupilPositionY,
                RightEyeGazeX = RightEyeGazeX,
                RightEyeGazeY = RightEyeGazeY,
                RightEyeGazeZ = RightEyeGazeZ,
                RightEyeGazeConfidence = RightEyeGazeConfidence,
                RightEyeOpenness = RightEyeOpenness,
                RightEyeOpennessConfidence = RightEyeOpennessConfidence,
                RightEyePupilDilation = RightEyePupilDilation,
                RightEyePupilDilationConfidence = RightEyePupilDilationConfidence,
                RightEyePupilPositionX = RightEyePupilPositionX,
                RightEyePupilPositionY = RightEyePupilPositionY,
                CombinedGazeX = CombinedGazeX,
                CombinedGazeY = CombinedGazeY,
                CombinedGazeZ = CombinedGazeZ,
                CombinedGazeConfidence = CombinedGazeConfidence,
                CognitiveLoadValue = CognitiveLoadValue,
                CognitiveLoadStandardDeviation = CognitiveLoadStandardDeviation,


            },
        };

        // Append a new line to the file.
        var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
            HasHeaderRecord = false,
            Delimiter = ";",

        };
        using (var stream = File.Open(path, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, config))
        {
            csv.WriteRecords(data);
            writer.Flush();
        }

        // Clean the attributes values after write
        if (AnswerMath != " ")
        {
            AnswerMath = " ";
        }

        if (ResponseTime != " ")
        {
            ResponseTime = " ";
        }
    }

}




