/* 
 Contains functions to move objects that cannot be inside the object controllers and also controls events not related to any object (These are the probe questions events)
 */


using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System;

public class IndependentController : MonoBehaviour {

	public float lastPresentationDelay = 2.0f;

	public IEnumerator shiftStim(GameObject stim){
		stimuliController stimCtrl = stim.GetComponent<stimuliController>() as stimuliController;
		float shiftDur = stimCtrl.reposition();
		//experimentController.msg += "shifting;"+ System.DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage( "Repositioning");
		experimentController.msg = "Repositioning";
		yield return new WaitForSeconds(shiftDur);
	}

	public IEnumerator endShow(GameObject stim, List<int> outcomes){  
//Pseudo Randomly select if the object showing from behind the occluder is the possible or impossible one
	//	Debug.Log("Outcomes length: "+ outcomes.Count);
		int idx = UnityEngine.Random.Range(0,outcomes.Count);
		int possible = outcomes[idx];
		outcomes.RemoveAt(idx);

		GameObject stimCopy;
		stimuliController stimCtrl;
		if(possible == 0){
			GameObject otherStim = GameObject.FindGameObjectWithTag("inside");
			stimCopy = Instantiate(otherStim, stim.transform.position, stim.transform.rotation) as GameObject;
			stimCtrl = stimCopy.GetComponent<stimuliController>() as stimuliController;
			DestroyObject(stim);
		}
		else{
			stimCtrl = stim.GetComponent<stimuliController>() as stimuliController;
		}
		stimCtrl.animator.SetTrigger("lastShow");
		//experimentController.msg += "EndAppear;"+ possible.ToString() + ";" + System.DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage("EndAppear;"+ possible.ToString() + ";" + System.DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";");
		experimentController.msg = "EndAppear;"+ possible.ToString() + ";"; 

		//0.5f is the duration of the lastShow animation
		yield return new WaitForSeconds(lastPresentationDelay + 0.5f);
	}
//The object moves left and container dissapears is removed
	public IEnumerator runFromScr(GameObject stim, List<int> outcomes){
		Debug.Log("Outcomes length: "+ outcomes.Count);

		int idx = UnityEngine.Random.Range(0,outcomes.Count);
		int possible = outcomes[idx];
		outcomes.RemoveAt(idx);

		experimentController.eventsLog.LogMessage("RunFromScr;");
		experimentController.msg = "RunFromScr;";
		stimuliController stimCtrl = stim.GetComponent<stimuliController>() as stimuliController;
		stimCtrl.runFromScreen();
		while(stimCtrl.run == true){
			yield return null;
		}
//Pseudo Randomly select if the object show remaining on the screen is the possible or impossible one
		GameObject stimCopy;

		if(possible == 0){
			GameObject otherStim = GameObject.FindGameObjectWithTag("inside");
			stimCopy = Instantiate(stim, otherStim.transform.position, otherStim.transform.rotation) as GameObject;
			stimCopy.GetComponentInChildren<Animator>().enabled = false;
			DestroyObject(otherStim);
		}

		//experimentController.msg += "After_RunFromScr;" + ";" + possible.ToString() + ";"+ System.DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage( "After_RunFromScr;" + ";" + possible.ToString() + ";");
		experimentController.msg = "After_RunFromScr;" + ";" + possible.ToString() + ";";
		yield return new WaitForEndOfFrame();
	}
}
