using UnityEngine;
using System.Collections;

public class DisplayScript : MonoBehaviour {
	public Camera ColorCamera;
	public Camera MapCamera;


	// Use this for initialization
	void Start () {
		//Enabable multiple displays
		if (Display.displays.Length > 1)
			Display.displays [1].Activate ();

		//Assing each display to a camera
		//ColorCamera.targetDisplay=0;
		//MapCamera.targetDisplay=1;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
