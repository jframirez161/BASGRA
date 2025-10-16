using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {
	
	public float speed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Horizontal")<0) {
			Camera.main.transform.Translate(Vector3.left*Time.deltaTime*speed);
			GameObject speed_ = GameObject.FindGameObjectWithTag ("Generator");
			speed_.GetComponent<Generator> ().speed = 1;
		}
		else if (Input.GetAxis("Horizontal")>0) {
			Camera.main.transform.Translate(Vector3.right*Time.deltaTime*speed);
			GameObject speed_ = GameObject.FindGameObjectWithTag ("Generator");
			speed_.GetComponent<Generator> ().speed = 1;
		}
		if (Input.GetAxis("Vertical")<0) {
			Camera.main.transform.Translate(Vector3.back*Time.deltaTime*speed, Space.World);
			GameObject speed_ = GameObject.FindGameObjectWithTag ("Generator");
			speed_.GetComponent<Generator> ().speed = 1;
		}
		else if (Input.GetAxis("Vertical")>0) {
			Camera.main.transform.Translate(Vector3.forward*Time.deltaTime*speed, Space.World);
			GameObject speed_ = GameObject.FindGameObjectWithTag ("Generator");
			speed_.GetComponent<Generator> ().speed = 1;
		}
		
		if (Input.GetAxis("Mouse ScrollWheel") < 0) 
		{
			Camera.main.orthographicSize ++;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0) 
		{
			Camera.main.orthographicSize --;
		}
	}
}
