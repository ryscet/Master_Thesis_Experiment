using UnityEngine;
using System.Collections;
using System;
public class stimuliController : MonoBehaviour {
	public Animator animator; 

	public bool move = false;
	Vector3 targetPos; 
	Vector3 shiftPos; 
	Vector3 runPos; 
	float speed = 4.0F;
	public bool run = false;
	bool shiftBack = false;
	Vector3 initialPos;

	public Vector3 position;
	public Texture2D currTexture;
	public string myName;
	public string myTag;
	public int pairId;

	public int insideContainer = 0;

	public AudioClip[] appear;
	AudioSource audio;

	void Start () {
		animator = GetComponentInChildren<Animator>() as Animator;
		targetPos = new Vector3(transform.position.x, transform.position.y - 7.0F,0.0F);
		runPos = new Vector3(transform.position.x - 10.0f, transform.position.y, transform.position.z);
		audio = GetComponent<AudioSource>();
	}
	
	void Update () {
		if(move){
			animator.enabled = false;
			this.transform.position = Vector3.Lerp(transform.position, targetPos, UnityEngine.Time.deltaTime * speed);
			if(this.transform.position.y < targetPos.y + 0.5F){
				move = false;

			}
		}

		if(run){
			this.transform.position = Vector3.Lerp(transform.position, runPos, UnityEngine.Time.deltaTime);
			if(this.transform.position.x < runPos.x + 6.0F){
				run = false;
			}
		}		
	}	

	public float reposition(){
		initialPos = new Vector3(-2.5F, 0.0f, transform.position.z);
		transform.position = initialPos;
		shiftPos = new Vector3(1.5F, transform.position.y, transform.position.z);
		animator.SetTrigger("shift");
		return 2.5f;
	}

	public void runFromScreen(){
		runPos = new Vector3(transform.position.x - 15.0f, transform.position.y, transform.position.z);
		run = true;
	}

	
	public void lastShow(){
		initialPos = new Vector3(-2.0F, transform.position.y, transform.position.z);
		transform.position = initialPos;
		shiftPos = new Vector3(1.5F, transform.position.y, transform.position.z);
		animator.SetTrigger("shift");
				
	}
	
	public float wiggle(int _idx){
		audio.PlayOneShot(appear[_idx], 0.5F);
		animator.enabled = true;
		animator.Update(0.0f);
		float length = animator.GetCurrentAnimatorStateInfo(0).length;
		return length;
	}
	
}
