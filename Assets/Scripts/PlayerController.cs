using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 1;
	public float maxbound = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float updown = Input.GetAxis ("Vertical");
		float leftright = Input.GetAxis ("Horizontal");
		MoveMe (updown, leftright);
	}

	void MoveMe(float updown, float leftright){
		transform.Translate (new Vector3 (leftright*speed*Time.deltaTime, updown * speed * Time.deltaTime, 0));; 

		Vector3 boundedPos =transform.position;

		if (boundedPos.x>maxbound){
			boundedPos.x = maxbound;
		} else if (boundedPos.x<0){
			boundedPos.x = -0;
		}

		if (boundedPos.y>maxbound){
			boundedPos.y = maxbound;
		} else if (boundedPos.y<0){
			boundedPos.y = -0;
		}

		transform.position = boundedPos;
	}
}
