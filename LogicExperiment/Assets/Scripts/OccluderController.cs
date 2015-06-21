using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;


public class OccluderController : MonoBehaviour {

	public Animator animator; 
	AudioSource audio;

	void Start() {
		audio = GetComponent<AudioSource>();
		animator = GetComponent<Animator>() as Animator;

	}

	public float twoStepUp(){
		animator.SetTrigger("playTwoStep");
		audio.Play();
//		experimentController.msg += "occluderUp;"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage("occluderUp;");
		experimentController.msg = "occluderUp;";
		return 1.0f;
	}	

	public float topDown(){
		//experimentController.msg += "occluderDown;"+ DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
		experimentController.eventsLog.LogMessage("occluderDown;");
		experimentController.msg = "occluderDown;";
		return 2.0f;
	}

}
