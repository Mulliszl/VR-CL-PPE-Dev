!! This project is only compatible with the HP Omnicept reverb G2 VR Headset.
!! If you are using a different device the data from the sensors will not be recorded and the controllers may not be recognized.


This project aims to use the sensors of the hp omnicept reverb G2 headset in order to facilitate communication in virtual reality.

Sensor data is saved in the location "ApplicationDataPath\SensorsCSVData\"

The recorded sensors are :
-HeartRate
-EyeGaze                        [L+R]   [x,y,z]
-EyeGazeConfidence              [L+R]
-EyeOpenness                    [L+R]
-EyeOpennessConfidence          [L+R]
-EyePupilDilation               [L+R]
-EyePupilDilationConfidence     [L+R]	
-EyePupilPosition               [L+R]   [x,y]
-CombinedGaze                           [x,y,z]
-CombinedGazeConfidence
-CognitiveLoadValue
-CognitiveLoadStandardDeviation

[L+R] a sensor is present for each eye
[x,y,z] the number of dimensions contained in the vector

From the game we are saving several more informations
-Stage                          get the current stage of the game
-TaskAudio                      when an audio task start
-TaskMath                       when an calcul task start
-Stabilization                  when the player is interacting with levers 
-AnswerAudio                    errors and correct answer with the audio task
-AnswerMath                     errors and correct answer with the calcul task




<<<   startup   >>>
[userid]
fill up the name to get a folder corresponding after the experiment

[experiment]

In order to launch the right experiment change the "type" in the config file

-calcul is the progressive task with <SimpleCalcul> and then <ComplexCalcul> maths with <Audio>
-doubleTask is the <SimpleCalcul> and <VerticalArtificialHorizon> with <Audio>
-tripleTaskNoInteruption is the <ButtonCalcul>,<AudioOrigin> and <RotationArtificialHorizon>
-tripleTaskNASAtlx is the same as tripleTaskNoInteruption with a pause between task to answer a survey



On the triple task experiment the errors are displayed and the game stops at the fifth error.
The first four errors will not be eliminatory
Each time you give the wrong answer, an error is counted.
Each time the counter drops to 0 an error is counted
Each time the artificial horizon exceeds 60 degrees an error is counted.

<SimpleCalcul>
answer simple digit calculations on calculator type input using ray on controllers
[0-9] to write numbers
[-] for negatives numbers
[=] to validate
["current entry"] for deleting one character
"trigger" to select button

<ComplexCalcul>
answer double digits calculations on calculator type input using ray on controllers
[0-9] to write numbers
[-] for negatives numbers
[=] to validate
["current entry"] for deleting one character
"trigger" to select button

<Audio>
"joystick click" when you hear a sound

<VerticalArtificialHorizon>
"joystick up" and "joystick down" to place the background on the line and "A" to validate

<ButtonCalcul>
press the correct answer using the controller

<AudioOrigin>
use "joystick up", "joystick down","joystick left" and "joystick right" to guess to origin of the sound

<RotationArtificialHorizon>
use the levers to offset the rotation speed of the background

		Good luck

    
[a relative link](Instructions\test.md)