using UnityEngine;
using System.Collections;

public class Experiment : MonoBehaviour {

	public PlayerController player;
	public GiveReward reward;
	public GameClock clock;
	public GameObject target;
	public GameObject sampleColor;
	public Camera colorCamera;


	//Target Settings
	public float targetSize = 1.0f;
	public float targetDisplayTime = 3.0f;
	public float targetX;
	public float targetY;
	public bool isSample = false;
	public float timeOnTarget = 1.0f;

	private float countdown;

	//Experiment is a singleton
	private static Experiment _instance;
	public static Experiment Instance{
		get{
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {
		reward = GameObject.FindGameObjectWithTag ("Reward").GetComponent<GiveReward> ();
		clock = GameObject.FindGameObjectWithTag ("Clock").GetComponent<GameClock> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		NewTrial ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((DistanceToTarget()<targetSize)&(player.timestopped>timeOnTarget)){
			reward.RewardAndFreeze (1);
			StartCoroutine (RewardEndTrial ());
		}
	
	}

	IEnumerator RewardEndTrial(){
		Vector3 tempPos = player.transform.position;
		while (reward.isFrozen) {
			player.transform.position = tempPos;
			yield return new WaitForSeconds(.01f);
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
		targetX = player.startpos.x;
		targetY = player.startpos.y;
		if (player.allowLeftRight) {
			targetX = Random.Range (0.0f, 10.0f);
		}
		if (player.allowUpDown) {
			targetY = Random.Range (0.0f, 10.0f);
		}
		target.transform.position =  new Vector3(targetX, targetY, 0);
	}

	float DistanceToTarget(){
		return Mathf.Sqrt (Mathf.Pow (player.transform.position.x - targetX, 2) + Mathf.Pow (player.transform.position.y - targetY, 2));
	}
		
}
