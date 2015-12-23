using UnityEngine;
using System.Collections;

public class Experiment : MonoBehaviour {

	public PlayerController player;
	public GiveReward reward;
	public GameClock clock;
	public GameObject target;
	public GameObject sampleColor;
	public Camera colorCamera;

	[HideInInspector] public bool isReplay = false;

	//Target Settings
	public float targetSize = 1.0f;
	public float targetDisplayTime = 3.0f;
	public float targetX;
	public float targetY;
	public bool isSample = false;
	public float timeOnTarget = 1.0f;
	public float closestTarget = 2.0f;

	public TrainingMode mode = TrainingMode.custom;
	public enum TrainingMode{
		custom,
		one,
		two,
		full
	}

	//Experiment settings: are left public so that they can be set manualy if 
	public bool resetToStart = false;
	//Sets up a fixed target. Only uses specified location if useFixedTarget is true;
	public bool useFixedTarget = false;
	public Vector2 fixedTarget = new Vector2(1.0f,0.0f);
	//Only one direction allowed; Must be set to fail if dimension!=1

	//Only works with Reset, or you get stuck
	public bool useFixedTargetDirection = false;
	//must be -1 (left, down) or 1(right,up)
	public int targetDirection = 1;


	/*List of trainin steps:
	 * Fixed target
	 * One fixed direction of motion
	 * Speed control
	 * Code to check that task is not impossible
*/
	//Experiment is a singleton
	private static Experiment _instance;
	public static Experiment Instance{
		get{
			return _instance;
		}
	}

	public void Awake(){
		if (_instance != null) {
			Debug.Log ("Instance already exists!");
			return;
		}
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		reward = GameObject.FindGameObjectWithTag ("Reward").GetComponent<GiveReward> ();
		clock = GameObject.FindGameObjectWithTag ("Clock").GetComponent<GameClock> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		SetParameters ();
		CheckThatTaskWorks ();
		NewTrial ();
	}

	void SetParameters(){
		switch (mode) {
		case(TrainingMode.custom):
			print ("Training Mode is Custom: using user input :)");
			break;
		//Up and down, always max bound.
		case(TrainingMode.one):
			resetToStart = true;
			useFixedTarget = true;
			player.allowUpDown = true;
			player.allowLeftRight = false;
			if (targetDirection == 1) {
				fixedTarget = new Vector2 (player.startpos.x, player.maxbound);
			} else {
				fixedTarget = new Vector2 (player.startpos.x, 0.0f);
			}
			break;
		//Left and right, always max bound.
		case(TrainingMode.two):
			resetToStart = true;
			useFixedTarget = true;
			player.allowUpDown = false;
			player.allowLeftRight = true;
			if (targetDirection == 1) {
				fixedTarget = new Vector2 (player.maxbound,player.startpos.y);
			} else {
				fixedTarget = new Vector2 (0.0f,player.startpos.y);
			}
			break;
		//Full Version of the game: all setting to default
		case(TrainingMode.full):
			resetToStart = false;
			useFixedTarget = false;
			useFixedTargetDirection = false;
			player.allowUpDown = true;
			player.allowLeftRight = true;
			break;
		}
	}

	void CheckThatTaskWorks (){
		if ((player.lockDirection || useFixedTarget||useFixedTargetDirection)&(!resetToStart)) {
			throw new UnityException("If fixing direction of motion or setting a fixed target location, must reset to center or you get stuck.");
		}
		if (player.dimension ==1){
			if ((player.allowLeftRight & useFixedTarget)&(player.startpos.y != fixedTarget.y))  {
				throw new UnityException ("Y error:Player Will never be able to access fixed target; Please reassign!");
			}
			if ((player.allowUpDown & useFixedTarget)&(player.startpos.x != fixedTarget.x))  {
				throw new UnityException ("Player Can only move in Y; Please reassign X!");
			}
		}

		if (useFixedTargetDirection&(player.dimension!=1)){
			throw new UnityException ("You have specified the target direction, but it can appear in 2 dimensions. This won't work.");
		}
	}

	// Update is called once per frame
	void Update () {
		if ((DistanceToTarget()<targetSize)&(player.timestopped>timeOnTarget)){
			reward.RewardAndFreeze (3);
			StartCoroutine (RewardEndTrial ());
		}

		//Check to see if we should quit now!
		if (Input.GetKey("escape")){
			Application.Quit();
		}
	}

	IEnumerator RewardEndTrial(){
		Vector3 tempPos = player.transform.position;
		while (reward.isFrozen) {
			player.transform.position = tempPos;
			yield return new WaitForSeconds(.01f);
		}
		if (resetToStart) {
			player.transform.position = player.startpos;
		}
		NewTrial ();
	}

	IEnumerator DisplaySample(){
		isSample = true;
		sampleColor.SetActive (true);
		//colorCamera.backgroundColor = Color.black;
		yield return new WaitForSeconds (targetDisplayTime);
		isSample = false;
		sampleColor.SetActive (false);
	}

	IEnumerator FreezeForSample(){
		Vector3 tempPos = player.transform.position;
		while (isSample) {
			player.transform.position = tempPos;
			yield return new WaitForSeconds(.01f);
		}
	}
		

	void NewTrial(){
		SetTarget ();
		StartCoroutine (DisplaySample ());
		StartCoroutine (FreezeForSample ()); 
	}

	void SetTarget(){
		//If a fixed target is specified, use it. Otherwise set a target automatically.
		if (useFixedTarget) {
			targetX = fixedTarget.x;
			targetY = fixedTarget.y;
		} else if (useFixedTargetDirection) {
			do {
				targetX = player.startpos.x;
				targetY = player.startpos.y;
				if ((player.allowLeftRight)&(targetDirection == 1)) {
					targetX = Random.Range (player.startpos.x+closestTarget, 10.0f);
				}else if ((player.allowLeftRight)&(targetDirection == -1)) {
					targetX = Random.Range (0.0f,player.startpos.x-closestTarget);
				}else if ((player.allowUpDown)&(targetDirection == 1) ) {
					targetY = Random.Range (player.startpos.y+closestTarget, 10.0f);
				}else{
					targetY = Random.Range (0.0f,player.startpos.y-closestTarget);
				}
			} while(DistanceToTarget()<closestTarget);
		}else {
			do {
				targetX = player.startpos.x;
				targetY = player.startpos.y;
				if (player.allowLeftRight) {
					targetX = Random.Range (player.startpos.x, 10.0f);
				}
				if (player.allowUpDown) {
					targetY = Random.Range (0.0f, 10.0f);
				}
			} while(DistanceToTarget()<closestTarget);
		} 
		target.transform.position =  new Vector3(targetX, targetY, 0);
	}

	float DistanceToTarget (){
		return Mathf.Sqrt (Mathf.Pow (player.transform.position.x - targetX, 2) + Mathf.Pow (player.transform.position.y - targetY, 2));
	}
		
}
