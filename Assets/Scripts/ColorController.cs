﻿using UnityEngine;
using System.Collections;

public class ColorController : MonoBehaviour {
	public GameObject Player;
	public Camera colorCamera;
	Experiment exp;
	public Color color;
	public float maxbound = 10;

	public Vector3 zero = new Vector3 (1.0f, 0.0f,0.0f);
	public Vector3 xVector = new Vector3(0.0f,1.0f,0.0f);
	public Vector3 yVector = new Vector3(0.0f,0.0f,1.0f);



	// Use this for initialization
	void Start () {
		colorCamera = GetComponent<Camera>();
		colorCamera.clearFlags = CameraClearFlags.SolidColor;
		exp = GameObject.FindGameObjectWithTag ("Experiment").GetComponent<Experiment>();
	}
	
	// Update is called once per frame
	void Update () {
		float X = Player.transform.position.x;
		float Y = Player.transform.position.y;
		if (exp.isSample) {
			colorCamera.backgroundColor = getColor (exp.targetX, exp.targetY);
		} else {
			colorCamera.backgroundColor = getColor (X, Y);
		}
/*
		if (!exp.isReplay) {
			LogColor ();
		}
        */
	}

	public Color getColor(float X, float Y) {
		Vector3 xComponent = zero + (X / maxbound) * (xVector-zero);
		Vector3 yComponent = zero + (Y / maxbound) * (yVector-zero);
		Vector3 newColor = xComponent + yComponent;
		return new Color (newColor.x, newColor.y, newColor.z, 1);
	}
    /*
	void LogColor(){
		//experimentLog.Log("Test_position");
		experimentLog.Log (GameClock.Instance.SystemTime_Milliseconds, 
		                   "Color" + ",POSITION," + colorCamera.backgroundColor);
	}*/
}
