# Connection to the headset

## HP Develloppers

First you need to create an account in [hp developers portal](https://developers.hp.com/), then login into the [developer's console](https://omnicept-console.hpbp.io/) and into the app menu you can access to the cresentials of your app. The licence type should be on academic.

![](/Instructions/Images/HP_Omnicept_Developer’s_Console.png)


## Unity

In order to setup the app for Unity follow this instruction https://developers.hp.com/omnicept/docs/unity/getting-started <br>
Make sure that you choose rev share in the Licence type otherwise cognitive load datas will not be accessed.<br>

![](/Instructions/Images/Licence_type.png)

**NOTE** : an internet connection is required to access cognitive load.

## Glia

Finally make sure that the Glia prefab is present on your scene

![](/Instructions/Images/glia.png)

## Controllers

In order to to link the controllers to unity and be capable of reading inputs, you can follow this tutorial : https://developers.hp.com/omnicept-xr/blog/reverb-g2-tracking-openxr-unity

For this project we have created our own medels using Catia and then converted in fbx (usable format for Unity) with Autocad Fusion.


# Sensors Data

At all scenes developed there are an Object called VrRig with a script component called "Sensors Data". The script "SensorData.cs", located at "Assets/Scripts/Sensors", has the objective to read and save all the sensors' value of the Headset in a csv file during an execution of the game.

To facilitate the way to save the csv file, a library called CsvHelper was used, you can access the documentation here https://joshclose.github.io/CsvHelper/getting-started/ .

In the script there are 4 classes. The first three classes("MyCsvDouble", "MyCsvTriple", "MyCsvCalcul") are used to struct the columns of the csv file generated. So, in these classes, just attributes of type "String" are declared. These classes are used by CsvHelper functions to write the columns values in the csv file.

The fourth class, "SensorsData", will have funcionality to create the csv file and write the sensors' data read in there from time to time. For this, the class has one attribute for each column in the csv file, and a periodic method is invoked to write all the attributes values in there respective columns in the csv file. 

A GliaBehavior object is used to listen from their UnityEvents the sensors' data received from the Glia service and fill the attributes values related to the sensors.

The attributes values related to the columns that indicates the Scene's Stage and the Tasks' performance, are filled by UnityEvents thrown by other scripts.

A public boolean attribute called "WriteCSV" is used by the component at  VrRig, to decide if the csv file will be created or not.

# Scenes
The game is launched on a menu where global config is loaded and store all variables needed for choosing experiment and then thart the choosed scene.

## Triple Task

### presentation

The Triple Task Scenes are controlled by a main script called main course (Assets/Interaction/MainCourse.cs). A variant of this scene is present in order to ask for for a survey betweeen the differents parts.

The experiment is separeted in 4 phases : The tutorial, Simple Task, Double Task and Triple Task. The main Course have to trigger each phase, launch and control the mini games scripts (One time calculomatic, One Time Audio Interactable and Lacet Interaction)

In the "Start" function, the script get the location of the others scripts it need to control and launch the tutorial.

The tutorial will go through each mini game with 5 iterations for the calcul and the audio and then 20 seconds of stabilisation, the whole thing will last about a minute (time to start recording the cognitive load) during all of this explanations about the inputs and how to play are shown on a prompt.

At the "Update" function, the script will go through each phase (simple, doube and triple task) with the calcul only for one minute, the calcul and the audio for one and a half minute and finally the calcul, the audio and the stabilisation for two minutes the time is set by the coroutines "phaseX waiter".

Each time the user answer a task he will have less time to answer the next (valid for audio and calcul task) the fonction choosed to handle this is **-ln(iterations) + 8** rounded to the nearest integer.

![](/Instructions/Images/fonctions.png)

The mini game will always wait for this formula before launching again this mini game, the mini games are offseted to not start at the same time.

The script OneTimeAudiointeractable is launched by calling triggerStart which will allow to get in update choose an audioSource among the 4 possible setup the timer and play the source. Then it will listen to the controller and as soon as the joystick is fully pushed into a direction(up, right, down or left) the method chooseDirection will select the associated source.Then in update the answer will be verified, and a timer is triggered in order to block incoming inputs for 0.5 second (to avoid multiple entries). If the answer is correct the timer is stopped and the script idle again, if wrong an error is logged in the csv.

The script OneTimecalculomatic is launched by calling triggerStart which will  trigger a timer and generate a random calcul (2 numbers between [-9,9] and an operator[+,-,*]), it will create a list of text (buttons) and shuffle it then calculate the answer for each operator and show it on the buttons. Each time the user push a button the script read the the value and check it, if correct the timer is stopped else an error is send to the csv ans the script idle again (idle is caused by not checking an impossible result : here 1.2 impossible to obtain with only intergers).

Lacet Interaction have for objective to turn randomly a canvas using the rotate() method. 3 times per second (every 20 frames) the acceleration is ajusted by adding and randomm between [-1,1] to the actual turning speed of the canvas. The canvas is capped at a max speed of 5. If the canvas is beyond 60° or -60° the initial position of the canvas is reseted and an error event is triggered. The actual rotation take into account the state of the two levers who can offset this speed by 3 to a maximum of 6 depending the position of the lever.

If the selected scene is **tripleTaskNASAtlx** a ready button will appear between each major phase (not in tutorial) will allow to pause the game and give time to answer questions like nasatlx.

A prompt is present on the right to see active games, the led is active when the game is triggered and switch off when the correct answer is given

Timers are set to show the user how much time he have to answer the one for calcul is located between the prompts and the one for audio is located on the left controller. they are setup to be stopped if the user answer before the end or reset if all the time is gone, when the timer hit 0 it throw an event "wrong" for the related task. each task have a timer associated. 

Each time an important action happen an event is send to SensorData in ordor to log this action, this include correct ans wrong answers for the calcul and the audio, audioTask and calculTask to know when these task are asked, Stabilisation to see if the user interact succesfully with the levers and finally Stage to get the current phase of the game.


### descriptions of the additionnals scripts present in this scene

**Spatial Sound Enabler**<br>
Allow us to spatialize sound calling the HPVRSpatialAudioEnabler plugin.
https://developers.hp.com/omnicept/docs/unity/spatial-audio

**XR Rig**<br>
Allow us to track the headset and reproduct the movements of the head in the application

**Lacet Interaction**<br>
Rotate the background image continuously with a variable speed and get the position of the levers to offset this speed. The offset can be null (lever is green) 3 (lever is yellow) or 6 (lever is red). the maximum speed of the background image is 5 the obective is to keep the image as stable as possible.

**Vr Activator**<br>
Allow us to activate the vr on the choosed scenes(deactivated for the menus)

**[Sensors data](#sensors-data)**<br>
Placed on "VrRig" allow us to get and save the data from sensors.

**lever interaction**<br>
trigger an event each time the the subject interact with the levers(when he release the lever).
The event call VerifyLever in Lacet Interaction which check if the subject pull the lever in the correct direction.

**artificial horizon**<br>
Follow the movement of the background (which is now disable because it was causing nausea) and reproduct it on the instruments on the cockpit.

**One time Calculomatic**<br>
OneTimeAudioInteractable is the script responsible for creating a calcul, creating three possible answers, show them listen to the buttons and check if the answer is correct.

**One Time Audio Interactable**
OneTimeAudioInteractable is the script responsible for choosing an audioSource play, listen to the controller and check if the answer is correct.

**Error Counter**<br>
Placed on the object "Prompt" in "cockpit" get the errors from all mini games. A public boolean Deadly decide if the application quit after a certain 4 errors.

## Progressive Task

The Double Task Scene is controlled by a main script called Calculomatic (Assets/Interaction/Calculomatic.cs) that uses the parameters in the config file to set the preferences of the scene: Time of each Phase, time of pauses, execution of the tutotiral and Nasa-TLX Scale.

The scene is composed by 3 phases: Tutorial, Simple Calcul and Complex Calcul. The Calculomatic Script is responsible to throw each phase, generate the calcul that has be answered, show the typed digits, validate the answer, throw the Audio Task and save the response time and set the pause between the phases with or without the Nasa-TLX Scale.

### **Tutorial**

In the "Start" function, the script checks the prefereces setted at the config file to decide if the tutorial was activated, if yes the "TutorialWaiter" coroutine starts this phase.

### **Phase 1**
At the "Update" function, the script wait until the user press "go". After the "go" is pressed the "Phase1Waiter" coroutine is thrown to count the Phase Time setted in the config file. 

While the phase time is not finished, the script will use the Config File to decide if a simple or a complex calcul has to be generated, write the calcul at the display, using the function "CalcAndShow()" and verify the user answer using the function "VerificationCalcul()". If correct, another calcul is generated. The attribute "AnswerMath" of the SensorsData script is setted at this point to write the user answer in the csv file.

In parallel the "TimerAudio" coroutine is used to control when the Audio Task must be thrown and get the user answer, calling the function "Listen" to verify the buttons pressed by the user. The "rapidite" coroutine is used to calculate, save and set the attribute "ResponseTime" of SensorsData script to write in the csv file the audio task response time of the user.


### **Pause**
When the Phase Time is finished, the script verify if a pause and the Nasa-TLX scale was activated in the preferences of the config file. If yes another coroutine starts to count the pause time, and the Nasa-TLX scale is showed. The program continues after the pause time finish and the Nasa-TLX scale is anwered.

### **Phase 2**
When the pause ends, the "Phase2Waiter" coroutine is started, and the same proccess to manage the scene done at the Phase 1 is performed. When the phase time ends, the Nasa-TLX is showed again, after the answer the scene is over. 

### **Nasa-TLX Scale**
The Nasa-TLX Scale has your own script called "Scale" (Assets/Scripts/Scale.cs). This script is used to get the values setted by the user in the scale and create a txt file to register this values.

At the end of the pauses' coroutine, when the Nasa-TLX scale is anwered and the user press confirm, the Scale script calculates and write the avereage of Audio Task Response Time in a txt, even ass the answers of the Nasa-TLX.

### **Digit Buttons**
The buttons of each digit in the scene uses the property "OnClik()" to call the function Calculomatic.Button_digit_Activated related to the digit. This functions write the digits in the answer. When the button "=" is pressed, the function called verify the answer and set the attribute "AnswerMath" of the SensorsData script to write the user answer in the csv file.

The button that shows the typed digits and the "go" text ("Button(12)" element in the scene) uses the property "OnClick()" to call the function Calculomatic.Button_DEL_Activated. This function is used to starts the scene when "go" is showed or delete a digit when the scene phases are already running. 

## Double Task

The Progressive Task Scene is controlled by a main script called MainTripleTask (Assets/Interaction/artificial horizon/MainTripleTask.cs) that uses the parameters in the config file to set the preferences of the scene: Time of each Phase, time of pauses, execution of the tutotiral and Nasa-TLX Scale.

The MainTripleTask script is similar to the Calculomatic script of Double Task Scene. The difference between these scenes is the Line Task that was added. All the others Tasks, Phase Routines and functions are executed as the Calculomatic Script.

At this scene exists 3 phases: Tutorial, one phase with the Line and Audio tasks and the third phase with the Line and Audio tasks and a simple calcul. 

### **Line Task**
When a phase starts the Line task function "StartLineTask()" is thrown. This function will reposition the line in the center and decide the direction of the line. 

While the phase is not finished, the function "MoveLine()" is called. In this function the line will move to the direction setted at "StartLineTask()". The line position will be checked, if the line already crossed the boundary edge (when the line is in the red area), if yes the attribute "Stabilization" of the SensorsData script will be setted as "Wrong". 

This function will also check if the A button of the controller was pressed by the user, using the variable "check". This variable is setted "True" at the function "Listen()" when the user press the A button. If the button was pressed, and the line is in the green area, the direction of the line is change and the attribute "Stabilization" of the SensorsData script will be setted as "Correct". If the button was pressed, and the line is not in the green area, the attribute "Stabilization" of the SensorsData script will be setted as "Wrong".

# PPE Charts

The application developed in Python to plot the graphs of data acquired by the exectuion of the scenes, used the concept of class in python and the libraries TKinter to create the User Interface, Pandas to manipulate the sensors' data and Matplotlib to plot the graphs.

The idea is to use the application directly by an executable, without install python and the libraries in the system, so that non-developer users can also use the tool.

To create the executable, another python library called py2exe was used. You can follow the tutorial here https://www.py2exe.org/index.cgi/Tutorial#Step1 .