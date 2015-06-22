using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class Setup : MonoBehaviour {

	List<int> randomPair;
	public static int pairs = 3;
	public static int conditions = 4;
	public static int pairRepetitions = 2;
	public static int questionTrials = 4;


	public GameObject[] currStim = new GameObject[2];
	Vector3[] startingPos = new Vector3[2];//t
	List<string> tags = new List<string>(){"stim_left", "stim_right"}; //t

	List<string> stimNames = new List<string>(){"ball", "snake", "boy", "umbrella", "bear", "star"};
	public Texture2D[] stimuliTextures;

	public Dictionary<string, Texture2D> stimQtextures =new Dictionary<string, Texture2D>();


	public GameObject[] pairedStimuli;
	public GameObject[] unpairedStimuli;

	experimentController expCtrl;


	public List<string> globalTrials = new List<string>(); 

	public List<int> stimuliShuffle_A = new List<int>(){};
	public List<int> stimuliShuffle_B = new List<int>(){};
	public List<int> stimuliShuffle_C = new List<int>(){};
	public List<int> stimuliShuffle_D = new List<int>(){};

	public List<int> stimuliShuffle_I1 = new List<int>(){};
	public List<int> stimuliShuffle_I2= new List<int>(){};
	public List<int> stimuliShuffle_I3= new List<int>(){};
	public List<int> stimuliShuffle_I4= new List<int>(){};

	public List<int> leftRightShuffle = new List<int>(){};
	AudioSource audio;
	public AudioClip appear;

	float afterWiggleDelay = 1.0f;
// New attempt to simplify the structure of setup;
	public stimuliController s1;
	public stimuliController s2;

	public Dictionary<string, List<int>> outcomes = new Dictionary<string,List<int>>();
	Dictionary<string, List<int>> trialTypes = new Dictionary<string,List<int>>();
	List<string> trialNames = new List<string>(){"runTrial_A", "runTrial_B", "runTrial_C","runTrial_D"}; //t
	public List<float> itiDistribution = new List<float>(){};


	 

	// Use this for initialization
	void Start() {
		audio = GetComponent<AudioSource>();
		expCtrl = GetComponent<experimentController>();
		startingPos[0] = new Vector3(-4.8f, 6.5f, 0.0F);
		startingPos[1] = new Vector3(-2.5f, 6.5f, 0.0F);
		
		startingPos[0].z = 0.0f;
		startingPos[1].z = 0.0F;

		for(int i = 0; i < stimNames.Count; i++){
			stimQtextures.Add(stimNames[i] + "(Clone)", stimuliTextures[i]);
		}

		itiDistribution.Add(1.5f);
		for(int i = 0; i < (pairs * conditions * pairRepetitions + (questionTrials * pairs)); i++){
			//itiDistribution.Add(itiDistribution[itiDistribution.Count - 1] * 1.1f);
			itiDistribution.Add(1.5f);
		}
		shuffleTrials ();
	}


//Creates a pseudo-random pair of stimuli (from shuffle) and positions each stimuli at a random (left or right) position in random order
	public IEnumerator createStimuli(List<int> stimuliShuffle, GameObject[] stimuli){
//Pick a random index for the stimuli pair
		int stimIdx = UnityEngine.Random.Range(0, stimuliShuffle.Count); 
		int randomPosIdx = UnityEngine.Random.Range(0,2);

//Create first random pair and random stimuli from the pair at random position 
		currStim[0] = Instantiate(stimuli[stimuliShuffle[stimIdx]],startingPos[randomPosIdx], Quaternion.identity) as GameObject;
		currStim[0].tag = tags[randomPosIdx];
		s1 = annotateStimulus(currStim[0], stimuliShuffle[stimIdx]/2);

		string setupMsg =  experimentController.trialCounter + ";" + experimentController.curTrial.ToString() + ";" + s1.myName +";"+stimuliShuffle[stimIdx].ToString() + ";"+ randomPosIdx.ToString()+";";
		experimentController.eventsLog.LogMessage(setupMsg);
		experimentController.msg = setupMsg;

		s1.move = true;
		audio.PlayOneShot(appear, 0.5F);

		while(s1.move == true){
			yield return null;
		}
		audio.PlayOneShot(appear, 0.5F);

//Create the remaining stimulus in the remaining position
		currStim[1] = Instantiate(stimuli[stimuliShuffle[stimIdx] + 1], startingPos[zeroToone(randomPosIdx)], Quaternion.identity) as GameObject;
		currStim[1].tag = tags[zeroToone(randomPosIdx)];

		s2 = annotateStimulus(currStim[1], 0);
//remove from the stimuli list to reduce a trial count
			
		stimuliShuffle.RemoveAt(stimIdx);

		s2.move = true;		

		yield return new WaitForSeconds(1.0F);	
		int audioIdx = UnityEngine.Random.Range(0, 3);
		yield return new WaitForSeconds(GameObject.FindGameObjectWithTag("stim_left").GetComponent<stimuliController>().wiggle(audioIdx));
		yield return new WaitForSeconds (GameObject.FindGameObjectWithTag("stim_right").GetComponent<stimuliController>().wiggle(audioIdx)  + afterWiggleDelay);

		string setupEndMsg =  "SetupFinished;";
		experimentController.eventsLog.LogMessage(setupEndMsg);
		experimentController.msg ="SetupFinished;";
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
	void shuffleTrials(){


		//for(int i = 0; i < experimentController.repetitions; i++){

		for(int z = 0; z < pairRepetitions; z++){
//For each trial type add 3 * 2 stimulus pairs
				stimuliShuffle_A.Add(0);
				stimuliShuffle_A.Add(2);
				stimuliShuffle_A.Add(4);
				
				stimuliShuffle_B.Add(0);
				stimuliShuffle_B.Add(2);
				stimuliShuffle_B.Add(4);
				
				stimuliShuffle_C.Add(0);
				stimuliShuffle_C.Add(2);
				stimuliShuffle_C.Add(4);
				
				stimuliShuffle_D.Add(0);
				stimuliShuffle_D.Add(2);
				stimuliShuffle_D.Add(4);
			}

//Add all the trial for each condition (trial type) to be run
		for(int j = 0; j < pairRepetitions * pairs; j++){

				globalTrials.Add("runTrial_A");
				globalTrials.Add("runTrial_B");
				globalTrials.Add("runTrial_C");
				globalTrials.Add("runTrial_D");
			}


		//
//Add the pseudorandomized right vs left grab and possible vs impossible outcome
// * 12 is the multibplier to get the absolute amount of trials, i.e. trials per type of stimulus (3) * amount of blocks (4)
//		for(int i = 0; i < (experimentController.repetitions * pairs * conditions+ 9); i++){
		for(int i = 0; i < pairRepetitions * pairs * conditions + (questionTrials * pairs); i++){

			if(i%2 == 0){
				leftRightShuffle.Add(0);
			}
			
			else{
				leftRightShuffle.Add(1);
			}

		}


//Make lists for each trial type for equal distribution of possible and impossible outcomes
		foreach (string _trialName in System.Enum.GetNames(typeof(trialType))) {
			outcomes.Add(_trialName,returnList());
		}


	
//Now populate the interruption questions trials
		stimuliShuffle_I1.Add(0);
		stimuliShuffle_I1.Add(2);
		stimuliShuffle_I1.Add(4);
		
		stimuliShuffle_I2.Add(0);
		stimuliShuffle_I2.Add(2);
		stimuliShuffle_I2.Add(4);
		
		stimuliShuffle_I3.Add(0);
		stimuliShuffle_I3.Add(2);
		stimuliShuffle_I3.Add(4);

		stimuliShuffle_I4.Add(0);
		stimuliShuffle_I4.Add(2);
		stimuliShuffle_I4.Add(4);

		for(int w = 0; w < 3; w++){
			
			globalTrials.Add("runTrial_I1");
			globalTrials.Add("runTrial_I2");
			globalTrials.Add("runTrial_I3");
		}

}

	List<int> returnList(){
		List<int> possibleImpossible = new List<int>(){};

		for(int i = 0; i < pairRepetitions * pairs; i++){
			
			if(i%2 == 0){
				possibleImpossible.Add(0);
			}
			
			else{
				possibleImpossible.Add(1);
			}
		}
		return possibleImpossible;
	}



stimuliController annotateStimulus(GameObject _stimulus, int pair_idx){
	
	stimuliController st = _stimulus.GetComponent<stimuliController>();
	st.currTexture = _stimulus.GetComponentInChildren<SpriteRenderer>().sprite.texture;
	st.myName =_stimulus.GetComponentInChildren<SpriteRenderer>().sprite.texture.name;
	st.myTag = _stimulus.tag;
	st.pairId = pair_idx;
	
	return st;
}

//Make logs depending on paired or unpared trial	
	void saveSetupMsg(string _name, string _tag){
		experimentController.msg += _name + ";"+ _tag +";" + System.DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";";
	}

}
