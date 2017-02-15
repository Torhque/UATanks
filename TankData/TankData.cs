using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//	The player's tank will use a "Tank Data" component to hold all data about the tank.
//	Tank Variables stored in separate TankData component 
public class TankData : MonoBehaviour {

	[HideInInspector]public TankMover mover; // For TankMover access
	[HideInInspector]public Shooter shooter; // For Shooter access
	[HideInInspector]public PlayerData playerData;

	[HideInInspector]public Transform tf; // transform of AI

	[Range(0, 10.0f)]public float fireRate; // Stores value for the rate of fire
	[HideInInspector]public float nextShotTime;
	[HideInInspector]public Tag objectTag;

	// The point value for destroying a tank is a property of each tank and is available for designers to set in the inspector.
	public float bounty;

	// Each player tracks their own score
	public float score;

	public int numLives;

	// The amount of damage done by a shell is a property of the entity firing the shell and can be edited by designers in the inspector.
	public float firepower;

	// The speed at which the player moves forward is available for designers to edit in the inspector and is in meters per second.
	[Range(0, 10.0f)]public float forwardSpeed;

	// The speed at which the player moves backwards is available for designers to edit in the inspector and is in meters per second.
	[Range(0, 10.0f)]public float reverseSpeed;

	// The speed at which the player rotates is available for designers to edit in the inspector and is in degrees per second.
	[Range(1.0f, 360.0f)]public float turnSpeed;

	// Use this for initialization
	void Start ()
	{
		// Load my components
		tf = gameObject.GetComponent<Transform> ();
		mover = GetComponent<TankMover>(); // Set type
		shooter = GetComponent<Shooter>(); // Set type
		objectTag = GetComponent<Tag>();

		// Set the next shot time to [now] plus timeBetweenShots
		nextShotTime = Time.time + fireRate;

		if (this.objectTag.isAITank == true) {
			numLives = GameManager.instance.tankAINumLives;
		}

		score = 0;
	}
}