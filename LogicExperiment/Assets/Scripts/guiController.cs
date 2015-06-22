using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;


public class guiController : MonoBehaviour {

	// Use this for initialization
	public bool showGUI = false;
	public Texture2D pauseBckg;
	public Texture2D stim1;
	public Texture2D stim2;
	public Texture2D contQuestion;
	public bool sortQuestionShow = false;
	public bool showGuiITI = false;
	public Texture2D up, down;
	float speededTime = 2.0f;
	Setup setup;
	float buttonSize = Screen.width / 10; 
	float buttonDitance = Screen.height / 25;
	float questionBoxSize = Screen.width / 3;
	Vector2[] boxPos;
	Vector2 pos_a, pos_b;
	public Texture2D[] cannotQuestion;
	public Texture2D[] cannotQuestion_unpaired;

	public AudioClip click;
	AudioSource audio;

	string[] buttons = new string[3] {"Start", "Options", "Exit"};
	int selected = 0;

	void Start(){
		boxPos = new Vector2[2]{new Vector2(Screen.width/2 - buttonSize/2,Screen.height/2.75F), new Vector2(Screen.width/2 - buttonSize/2, Screen.height/2.75F + buttonSize + buttonDitance)};
		setup = GameObject.Find("Main Camera").GetComponent("Setup") as Setup;
		audio = GetComponent<AudioSource>();
	}

	void OnGUI(){
		Cursor.visible = false;

		if(showGUI){
			Cursor.visible = true;



			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), pauseBckg);

			//GUI.DrawTexture(new Rect(Screen.width/2 -50,75, 100, 100), contQuestion, ScaleMode.ScaleToFit);

			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			GUI.Box(new Rect(Screen.width/2 - questionBoxSize/2, Screen.height / 20,questionBoxSize,questionBoxSize/8), "What is in the container?");


			if (GUI.Button(  new Rect(boxPos[0].x, boxPos[0].y - buttonSize - buttonDitance,buttonSize,buttonSize), contQuestion)){
				showGUI = false;
				Debug.Log(setup.s1.insideContainer);
				//experimentController.msg += setup.s1.insideContainer.ToString() + ";"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
				experimentController.eventsLog.LogMessage("dont know" + ";");
				showGuiITI= true;
				audio.PlayOneShot(click, 1.0F);

			}

			if (GUI.Button(  new Rect(pos_a.x,pos_a.y,buttonSize,buttonSize), setup.stimQtextures[setup.s1.name])){
				showGUI = false;
				//experimentController.msg += setup.s1.insideContainer.ToString() + ";"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
				experimentController.eventsLog.LogMessage(setup.s1.insideContainer.ToString() + ";");
				showGuiITI= true;
				audio.PlayOneShot(click, 1.0F);
			}

			if (GUI.Button(  new Rect(pos_b.x , pos_b.y,buttonSize, buttonSize), setup.stimQtextures[setup.s2.name])){
				showGUI = false;
				//experimentController.msg += setup.s2.insideContainer.ToString() + ";"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
				experimentController.eventsLog.LogMessage(setup.s2.insideContainer.ToString() + ";");
				showGuiITI= true;
				audio.PlayOneShot(click, 1.0F);
			}

			if(experimentController.curTrial != trialType.typeI4 && experimentController.curTrial != trialType.typeI1){
				if (GUI.Button(  new Rect(boxPos[1].x, boxPos[1].y + buttonSize + buttonDitance ,buttonSize,buttonSize), cannotQuestion[setup.s1.pairId])){
					showGUI = false;
					//experimentController.msg += "2" + ";"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
					experimentController.eventsLog.LogMessage("cannot know;");
					showGuiITI= true;
					audio.PlayOneShot(click, 1.0F);
				}
			}
			else{
				if (GUI.Button(  new Rect(boxPos[1].x, boxPos[1].y + buttonSize + buttonDitance ,buttonSize,buttonSize), cannotQuestion_unpaired[setup.s1.pairId])){
					showGUI = false;
					//experimentController.msg += "2" + ";"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
					experimentController.eventsLog.LogMessage("cannot know;");
					showGuiITI= true;
					audio.PlayOneShot(click, 1.0F);

				}
			}
		}


		if(sortQuestionShow){
			Cursor.visible = true;

			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), pauseBckg);
			
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			GUI.Box(new Rect(Screen.width/2 - questionBoxSize/2, Screen.height / 20,questionBoxSize,questionBoxSize/8), "Was the movie good or bad?");
			GUI.SetNextControlName(buttons[2]);
			if (GUI.Button( new Rect(Screen.width/4 - buttonSize,Screen.height/2,buttonSize*2,buttonSize*2), up)){
				//experimentController.msg += "ans1;"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
				experimentController.eventsLog.LogMessage( "answerGood");
				sortQuestionShow= false;
				showGuiITI= true;
				audio.PlayOneShot(click, 1.0F);
			}
			
			if (GUI.Button( new Rect(Screen.width/2 + Screen.width/4- buttonSize,Screen.height/2,buttonSize*2,buttonSize*2), down)){
				//experimentController.msg += "ans2;"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
				experimentController.eventsLog.LogMessage( "answerBad");
				sortQuestionShow= false;
				showGuiITI= true;
				audio.PlayOneShot(click, 1.0F);
			}
		}

		if(showGuiITI){
			Cursor.visible = false;

			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), pauseBckg);

		}
	}

	public IEnumerator sortQuestion(){
		sortQuestionShow = true;
		experimentController.eventsLog.LogMessage( "AskingSortingQuestion");

		yield return new WaitForSeconds(speededTime);
		//Play Audio here
		

		while(sortQuestionShow == true){
			yield return null;
		}
		yield return StartCoroutine(showITI());
	}
	
	
	public IEnumerator showITI(){
		yield return new WaitForSeconds(setup.itiDistribution[UnityEngine.Random.Range(0, setup.itiDistribution.Count)]);
		showGuiITI= false;

	}

	public IEnumerator interruptTrial(){
		//experimentController.msg += "asking_Q;" + DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage( "InterruptionQuestion");

		//updateTexture(setup.stimQtextures[setup.s1.name],setup.stimQtextures[setup.s2.name]);
		int randomPos = UnityEngine.Random.Range(0,2);
		pos_a = boxPos[randomPos];
		pos_b = boxPos[zeroToone(randomPos)];
		showGUI = true;
		while(showGUI == true){
			yield return null;
		}
		yield return StartCoroutine(showITI());
	}

	int zeroToone(int x){
		if(x == 1){
			return 0;
		}
		
		if(x == 0){
			return 1;
		}
		else{
			Debug.Log("Wrong input for 0 to 1 switch");
			return 100;
		}
	}

}
