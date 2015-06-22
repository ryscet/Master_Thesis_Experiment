using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class ContainerController : MonoBehaviour {

	bool rotate;
	Vector3 currPos;
	public Animator animator; 
	bool objReturned = false;
	Vector3 startScale;
	Vector3 startPosition;
	List<string> objToGrabList = new List<string>{ "catchLeft", "catchRight"};
	List<string> tags = new List<string>(){"stim_left", "stim_right"};
	List<int> myLeftRight;
	List<trialType> dissapearTrials = new List<trialType>(){trialType.typeB, trialType.typeC, trialType.typeD, trialType.typeI1, trialType.typeI2,  trialType.typeI3};
	string containerMsg;
	public AudioClip collect;
	AudioSource audio;

	void Start () {
		animator = GetComponent<Animator>() as Animator;
		startScale = this.transform.localScale;
		startPosition = new Vector3(3.1f,0.0f, 0.0f);

		myLeftRight = GameObject.Find("Main Camera").GetComponent<Setup>().leftRightShuffle;
		audio = GetComponent<AudioSource>();


	}

	public void resetTrial(){
		transform.localScale = startScale;
		transform.position = startPosition;
		animator.enabled = true;
	}

	public IEnumerator middleMoveContainer(){
		experimentController.eventsLog.LogMessage("movingContainer;");
		objReturned =false;
		animator.SetTrigger("play");
		while(objReturned == false){
			yield return null;
		}

		yield return new WaitForSeconds(1.25f);
	}
//TODO Pseudo randomize this
//This function is repeating (catch left and right very simmilar becaue the animation is different for left and right...)
	void selectObjToGrab(){
		audio.PlayDelayed(0.3f);

		int idx = UnityEngine.Random.Range(0,myLeftRight.Count);


		animator.SetTrigger(objToGrabList[myLeftRight[idx]]);
		GameObject grabOb = GameObject.FindGameObjectWithTag(tags[myLeftRight[idx]]);
		grabOb.GetComponent<stimuliController>().insideContainer = 1;

		experimentController.eventsLog.LogMessage("objGrabbed;" + myLeftRight[idx].ToString() +";"+ tags[myLeftRight[idx]] +";");
		experimentController.msg = "objGrabbed;" + myLeftRight[idx].ToString() +";"+ tags[myLeftRight[idx]] +";";
		myLeftRight.RemoveAt(idx);


	//	Choose what to do with the container based on trial type here
		//if(experimentController.curTrial == trialType.typeB || experimentController.curTrial == trialType.typeC || experimentController.curTrial == trialType.typeD){
		if(dissapearTrials.Contains(experimentController.curTrial)){
				this.GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("containerCover").GetComponent<SpriteRenderer>().enabled = false;
		}

	}


	void disableAnimator(){
		animator.enabled = false;
	}
	
	
	void releaseObject(){
		GameObject stim = GameObject.FindGameObjectWithTag("inside");
		stim.transform.parent = null;
		objReturned =true;
	}

	void catchLeftObj(){
		GameObject grabOb = GameObject.FindGameObjectWithTag("stim_left");
		GameObject occOb = GameObject.FindGameObjectWithTag("stim_right");
		grabOb.transform.parent = this.transform;
		grabOb.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1.0f) ;
		occOb.tag = "outside";
		grabOb.tag = "inside";

		this.GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("containerCover").GetComponent<SpriteRenderer>().enabled = true;
		//experimentController.msg += "LeftGrabbed;"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage("LeftGrabbed;");
		experimentController.msg = "LeftGrabbed;";

	}

	void catchRightObj(){
		GameObject grabOb = GameObject.FindGameObjectWithTag("stim_right");
		GameObject occOb = GameObject.FindGameObjectWithTag("stim_left");
		grabOb.transform.parent = this.transform;
		grabOb.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1.0f) ;
		grabOb.tag = "inside";
		occOb.tag = "outside";

		this.GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("containerCover").GetComponent<SpriteRenderer>().enabled = true;
		//experimentController.msg += "RightGrabbed;"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage("RightGrabbed;");
		experimentController.msg ="RightGrabbed;";
	}
}
