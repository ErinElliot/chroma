using UnityEngine;
using System.Collections;

public class KeyboardCameraControl : MonoBehaviour {

	private Quaternion desiredRotation;
	private Quaternion currentRotation;
	private Quaternion rotation;
	private float distance = 11.3f;
	private Vector3 position;
	private float dampening = 5.0f;
	private Vector3 targetPosition;
	// Use this for initialization
	void Start () {
		desiredRotation = transform.rotation;
		currentRotation = transform.rotation;
		position = transform.position;
		targetPosition = new Vector3(0.0f, 0.0f, 4.0f);//transform.position + (transform.forward * distance);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("a")) 
		{
			setCameraRotation(35.0f, 315.0f);
		}
		if (Input.GetKeyDown ("s")) 
		{
			setCameraRotation(0.0f, -90.0f);
		}
		if (Input.GetKeyDown ("d")) 
		{
			setCameraRotation(35.0f, 225.0f);
		}
		if (Input.GetKeyDown ("f")) 
		{
			setCameraRotation(0.0f, 90.0f);
		}
	}

	void LateUpdate()
	{

		currentRotation = transform.rotation;
		rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * dampening);
		transform.rotation = rotation;
		position = targetPosition - (rotation * Vector3.forward * distance);
		transform.position = position;
	}

	void setCameraRotation(float xDeg, float yDeg)
	{
		desiredRotation = Quaternion.Euler(xDeg, yDeg, 0);
	}
}
