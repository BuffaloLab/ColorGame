using UnityEngine;
using System.Collections;

public class Experiment : MonoBehaviour {

	public GameObject player;
	public GiveReward reward;
	public GameClock clock;
	public GameObject target;
	public GameObject sampleColor;
	public Camera colorCamera;

	//Target Settings
	public float targetSize = 3.0f;
	public float targetDisplayTime = 3.0f;
	private float targetX;
	private float targetY;
	private bool isSample = false;

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
		NewTrial ();
	}
	
	// Update is called once per frame
	void Update () {
		if (DistanceToTarget()<targetSize){
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
		targetX = Random.Range (0.0f,10.0f);
		targetY = Random.Range (0.0f,10.0f);
		target.transform.position =  new Vector3(targetX, targetY, 0);
		StartCoroutine (DisplaySample ());
		StartCoroutine (FreezeForSample ()); 
	}

	float DistanceToTarget(){
		return Mathf.Sqrt (Mathf.Pow (player.transform.position.x - targetX, 2) + Mathf.Pow (player.transform.position.y - targetY, 2));
	}
		
}
