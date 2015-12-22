using UnityEngine;
using System.Collections;

public class PlayerLogger: MotherOfLogs {

	void Awake(){
		//exp = GameObject.FindGameObjectWithTag ("Experiment").GetComponent<Experiment>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () { 
		if (!exp.isReplay) {
			LogPosition ();
		}
	}

	void LogPosition(){
		//experimentLog.Log("Test_position");
		experimentLog.Log (GameClock.Instance.SystemTime_Milliseconds, 
		                   "Avatar " + ",POSITION," + gameObject.transform.position.x + "," + gameObject.transform.position.y);
	}
}