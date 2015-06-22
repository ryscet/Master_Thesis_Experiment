using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public enum trialType
{
	typeA = 0,
	typeB = 1,
	typeC = 2,
	typeD = 3,
	typeI1 = 4,
	typeI2 = 5,
	typeI3 = 6,
	typeI4= 7
}


public class experimentController : MonoBehaviour {

	public static trialType curTrial;
	public static string msg; //t
	public static int trialCounter = 0;
	public static int repetitions = 2;
	int block_idx = 0;
	float betweenShiftDelay = 2.0f;

	public static LogMsg eventsLog;
	StreamLog streamLog;
	//array of 0,2,4 for choosing the random stimulus then plus one, or maybe a modulus logic
	float startTime;
	float endTime;

	public OccluderController occluder; 
	public ContainerController container; 
	public guiController guiCtrl; 
	Setup setup;
	IndependentController indCtrl;


	void Awake () {
		Application.targetFrameRate = 60;
	}

	void Start () {

		eventsLog = new LogMsg("eventsLog", "eventsData");
		streamLog = new StreamLog("streamLog", "streamData");

		eventsLog.LogMessage("Experiment Started;");
		occluder = GameObject.Find("occlusion").GetComponent("OccluderController") as OccluderController;
		container = GameObject.Find("container").GetComponent("ContainerController") as ContainerController;
		guiCtrl = GameObject.Find("background").GetComponent("guiController") as guiController;

		setup = GetComponent<Setup>() as Setup;
		indCtrl = GetComponent<IndependentController>() as IndependentController;
	}

	void Update () {

		if(Input.GetKeyUp("1")){ 
			myMain();
		}

		if(Input.GetKeyUp("2")){ 
			Application.LoadLevel(Application.loadedLevel);
		}


		streamLog.LogMessage(msg);


		if(Input.GetKeyUp(KeyCode.Escape)){
			Application.Quit();
		}
	}


	private void myMain(){
		trialCounter++;
		//Debug.Log("length: " +trialCounter.ToString() + " " + setup.globalTrials.Count.ToString());
	
		msg = trialCounter + ";";
		int idx = UnityEngine.Random.Range(0,setup.globalTrials.Count);
		if(setup.globalTrials.Count > 0){

			//msg += setup.globalTrials[idx] + ";";
			StartCoroutine(setup.globalTrials[idx]);
			Debug.Log(setup.globalTrials[idx]);
			setup.globalTrials.RemoveAt(idx);

		}
		else{
			Debug.Log("end bloclk");
			//Application.LoadLevel("block_2");
			block_idx += 1;
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	


//No inference trial, the object is grabbed by the container and then the occluder moves up
	IEnumerator runTrial_A(){
		curTrial = trialType.typeA;

		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_A, setup.pairedStimuli));
		yield return StartCoroutine(container.middleMoveContainer());
		yield return new WaitForSeconds (occluder.twoStepUp());
		yield return StartCoroutine(indCtrl.shiftStim(GameObject.FindGameObjectWithTag("outside")));
		yield return new WaitForSeconds(betweenShiftDelay);
		yield return StartCoroutine(indCtrl.endShow(GameObject.FindGameObjectWithTag("outside"), setup.outcomes[curTrial.ToString()]));
		yield return new WaitForSeconds(indCtrl.lastPresentationDelay -1.0f);
		yield return StartCoroutine(guiCtrl.sortQuestion());
		chooseTrialType();
	}
//Inference trial, first the occluder appears, then the object is grabbed, then one appears from behind the occluder
	IEnumerator runTrial_B(){
		curTrial = trialType.typeB;

		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_B, setup.pairedStimuli));
		yield return new WaitForSeconds (occluder.twoStepUp());
		yield return StartCoroutine(container.middleMoveContainer());
			eventsLog.LogMessage("InferenceStarting");
		yield return StartCoroutine(indCtrl.shiftStim(GameObject.FindGameObjectWithTag("outside")));
			eventsLog.LogMessage("InferenceEnding");
		yield return new WaitForSeconds(betweenShiftDelay);
			eventsLog.LogMessage("OutcomeStarting");
		yield return StartCoroutine(indCtrl.endShow(GameObject.FindGameObjectWithTag("outside"), setup.outcomes[curTrial.ToString()]));
		yield return new WaitForSeconds(indCtrl.lastPresentationDelay -1.0f);
			eventsLog.LogMessage("OutcomeEnding");
		yield return StartCoroutine(guiCtrl.sortQuestion());

		chooseTrialType();
	}
//Inference trial, first the occluder appears, then the one object is grabbed, then the occluder dissapears and the object runs. Lastly the object in the container is shown
	IEnumerator runTrial_C(){
		curTrial = trialType.typeC;
		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_C, setup.pairedStimuli));
		yield return new WaitForSeconds (occluder.twoStepUp());
		yield return StartCoroutine(container.middleMoveContainer());
			eventsLog.LogMessage("InferenceStarting");
		yield return new WaitForSeconds (occluder.topDown() + 1.0f);
		yield return StartCoroutine(indCtrl.runFromScr(GameObject.FindGameObjectWithTag("outside"), setup.outcomes[curTrial.ToString()]));
			eventsLog.LogMessage("InferenceEnding");
			eventsLog.LogMessage("OutcomeStarting");
		container.animator.SetTrigger("dissapear");
		yield return new WaitForSeconds(2.0f);
			eventsLog.LogMessage("OutcomeEnding");
		yield return StartCoroutine(guiCtrl.sortQuestion());

		chooseTrialType();
	}
//No inference trial, two stimuli are different
	IEnumerator runTrial_D(){
		curTrial = trialType.typeD;
		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_D, setup.unpairedStimuli));
		yield return new WaitForSeconds (occluder.twoStepUp() + 1.0f);
		yield return StartCoroutine(container.middleMoveContainer());
		yield return StartCoroutine(indCtrl.shiftStim(GameObject.FindGameObjectWithTag("outside")));
		yield return new WaitForSeconds(betweenShiftDelay);
		yield return StartCoroutine(indCtrl.endShow(GameObject.FindGameObjectWithTag("outside"), setup.outcomes[curTrial.ToString()]));

		yield return StartCoroutine(guiCtrl.sortQuestion());


		chooseTrialType();
	}
//Ask a question about container content after the different-top stimulus was grabbed
	IEnumerator runTrial_I1(){
		curTrial = trialType.typeI1;
		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_I1, setup.unpairedStimuli));
		yield return new WaitForSeconds (occluder.twoStepUp());
		yield return StartCoroutine(container.middleMoveContainer());
		yield return StartCoroutine(guiCtrl.interruptTrial());

		chooseTrialType();
	}

//Ask a question about container content after the same-top stimulus was grabbed and the other one was revealed
	IEnumerator runTrial_I2(){
		curTrial = trialType.typeI2;
		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_I2, setup.pairedStimuli));
		yield return new WaitForSeconds (occluder.twoStepUp());
		yield return StartCoroutine(container.middleMoveContainer());
		yield return StartCoroutine(indCtrl.shiftStim(GameObject.FindGameObjectWithTag("outside")));
		yield return StartCoroutine(guiCtrl.interruptTrial());
		chooseTrialType();
	}
//Ask a question about container content wehn it is impossible to know
	IEnumerator runTrial_I3(){
		curTrial = trialType.typeI3;
		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_I3, setup.pairedStimuli));
		yield return new WaitForSeconds (occluder.twoStepUp());
		yield return StartCoroutine(container.middleMoveContainer());
		yield return StartCoroutine(guiCtrl.interruptTrial());
		chooseTrialType();
	}

	IEnumerator runTrial_I4(){
		curTrial = trialType.typeI4;
		yield return StartCoroutine (setup.createStimuli(setup.stimuliShuffle_I1, setup.unpairedStimuli));
		yield return new WaitForSeconds (occluder.twoStepUp());
		yield return StartCoroutine(container.middleMoveContainer());
		yield return StartCoroutine(indCtrl.shiftStim(GameObject.FindGameObjectWithTag("outside")));
		yield return StartCoroutine(guiCtrl.interruptTrial());
		chooseTrialType();
	}
		

	void chooseTrialType(){
		clearTrial();
		myMain();

	}


	void clearTrial(){
		occluder.animator.SetTrigger("dissapear");
		container.resetTrial();
		DestroyAllObjects();
	}		
	

	void  OnApplicationQuit(){
		eventsLog.myClose();
	}

	void DestroyAllObjects()
	{
		GameObject[] cp = GameObject.FindGameObjectsWithTag("inside");
		foreach (GameObject stim in cp)
		{
			Destroy(stim);
		}

		GameObject[] op = GameObject.FindGameObjectsWithTag("outside");
		foreach (GameObject stim in op)
		{
			Destroy(stim);
		}
		foreach (GameObject stim in setup.currStim)
		{
			Destroy(stim);
		}
	}	
}




//TODO check if switching objects correctly, why reversed in trial type c?


