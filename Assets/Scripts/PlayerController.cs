using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 1.0f;
	public float maxbound = 10.0f;
	public float timestopped = 0.0f;

	public bool allowLeftRight = true;
	public bool allowUpDown = true;
	public Vector3 startpos;
	public int dimension;

	// Use this for initialization
	void Start () {
		if (allowLeftRight & allowUpDown) {
			dimension = 2;
		} else if (allowLeftRight || allowUpDown) {
			dimension = 1;
		} else {
			throw new UnityException("You need 1 or 2 dimensions of motion!");
		}

		startpos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		bool isMoved = GetInput ();
		if (!isMoved) {
			timestopped += Time.deltaTime;
		} else {//Player has moved, reset stop timer.
			timestopped = 0.0f;
		}
	}

	bool GetInput(){
		float updown = Input.GetAxis ("Vertical");
		if ((Mathf.Abs(updown) < .001f)||(!allowUpDown)) {
			updown = 0.0f;
		}
		float leftright = Input.GetAxis ("Horizontal");
		if ((Mathf.Abs(leftright) < .001f)||(!allowLeftRight)){
			leftright = 0.0f;
		}

		MoveMe (updown, leftright);

		if ((updown == 0) & (leftright == 0)) {
			return false;
		} else {
			return true;
		}
	}

	void MoveMe(float updown, float leftright){
		transform.Translate (new Vector3 (leftright*speed*Time.deltaTime, updown * speed * Time.deltaTime, 0));; 
		print (new Vector3 (leftright * speed * Time.deltaTime, updown * speed * Time.deltaTime, 0));
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
