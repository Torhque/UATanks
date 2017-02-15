using UnityEngine;
using System.Collections;

// Requires anything trying to use this to have a CC, otherwise it makes one for it
[RequireComponent(typeof (CharacterController))]

public class TankMover : MonoBehaviour {

	private CharacterController cc; // [characterController]
	private Transform tf;


	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		tf = GetComponent<Transform> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Movement uses SimpleMove() and Rotate() in a separate mover component. 
	// Function to move forward
	public void Accelerate (float speed) 
	{
		cc.SimpleMove ( tf.forward * speed); // Move at [speed] per second
	}

	public void Reverse (float speed)
	{
		cc.SimpleMove (tf.forward * (speed * -1)); // Reverse at [speed]/second
	}

	public void TurnRight (float speed)
	{
		cc.transform.Rotate (Vector3.up * speed); // Turn right at speed/second
	}

	public void TurnLeft (float speed)
	{
		cc.transform.Rotate (Vector3.up * (speed * -1)); // turn left at speed/second
	}
}
