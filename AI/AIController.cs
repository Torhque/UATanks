using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TankData))]

public class AIController : MonoBehaviour {

    #region variables
    [Header("Personality Data")]
	public AIStates currentState; // create an AIStates object
    public enum AIStates { Idle, Patrol, Engage, Chase, Flee }; // Establish AI states and store them in an enum
    public float timeInCurrentState;
	public enum PatrolTypes { Stop, Loop, PingPong, Random }; // Establish various patrol states
	public enum EngageTypes { Hunter, Sentry, Coward };
	public EngageTypes engageType;
	public bool isFleeing;

    [Header("Obstacle Avoidance")]
    public AIAvoidanceStates avoidState;
	public enum AIAvoidanceStates { Normal, TurnToAvoid, MoveToAvoid};
	public float clearanceDistance; // How close to an obstacle before avoiding
	public float clearanceTime; // How long to try to avoid before going back to normal chase
	private float timeInAvoidState; // How long have I been in this avoidance state
	public LayerMask avoidMask;
//	public float sphereCastRadius;

	[Header("Patrol Data")]
	public PatrolTypes patrolType; // object for storing type of patrol
	public float closeEnough; // How close enough to an object is close enough
	public int patrolDirection = 1; // Value for manipulating AI's direction
	public float idleTime = 3.0f;

	[Header("Target Data")]
	public GameObject target; // Who's gun die
	private Tag tankTag;
	public float hearingDistance; // How far away can my AI sense a target
	public float fieldOfView = 60.0f;
	public float viewDistance = 8.0f;
	public float fleeDistance = 5.0f;

	[Header("Waypoints")]
	public Room tempRoom;
	public Room parentRoom;
	public List<Transform> waypoints;
	public int currentWaypoint;


	//public Transform playerTransform; // transform of player
	public TankData aiTank;
	public Shooter shooterComponent;
    #endregion

    // Use this for initialization
    void Start () {

		shooterComponent = 	GetComponent<Shooter>(); // Load our shooter component
		aiTank = GetComponent<TankData> ();

		parentRoom = GetComponentInParent<Room> (); // Load the Room component

		// Store the Room's waypoints List in the AI's waypoints List
		waypoints = parentRoom.roomWaypointsList;

		currentWaypoint = 0;
	}
	
	// Update is called once per frame
	void Update () {

		// Have the AI find their target (is there a better way than doing this in Update?)
		FindTargets (); 

		#region AI state switches
		// Track the time spent in current state
		timeInCurrentState += Time.deltaTime;

		switch (currentState) {
		case AIStates.Idle:
			isFleeing = false;
			// Check for transitions
			DoIdle ();
			// Check for transitions
			if (timeInCurrentState > idleTime) {
				ChangeAIState (AIStates.Patrol);
			}
			break;
		
		case AIStates.Patrol:
			isFleeing = false;
			DoPatrol ();
			// Check for transitions
			// Checks if the tank is heard
			if ( CanHear(target) ) {
				if ( engageType == EngageTypes.Hunter ) 
				{
					ChangeAIState( AIStates.Chase );
				} 
				else if ( engageType == EngageTypes.Sentry ) 
				{
					ChangeAIState( AIStates.Engage );
				}
				else if (engageType == EngageTypes.Coward) 
				{
					ChangeAIState( AIStates.Flee );
				}
			}
			break;

		case AIStates.Engage:
			isFleeing = false;
			if (target != null) {
				DoEngage(target);
			} else {
				ChangeAIState (AIStates.Idle);
			}
			break;

		case AIStates.Chase:
			isFleeing = false;
			DoChase ();

			if (target != null) {
				if ( CanSee(target) ) {				

					// The player's fire rate is limited. Designers can set the number of seconds between shots in the inspector.
					if (Time.time >= aiTank.nextShotTime) {

						shooterComponent.Shoot ();
						aiTank.nextShotTime = Time.time + aiTank.fireRate;
					}
				} 
				// Checks if the tank CAN NO LONGER be heard
				else if (Vector3.Distance (target.GetComponent<Transform> ().position, aiTank.tf.position) > hearingDistance) {
					ChangeAIState (AIStates.Idle);
				}
			}
			break;
		case AIStates.Flee:
			if (target != null) {
				DoFlee ();
			} else {
				DoPatrol();
			}
			break;
		}
	}
	#endregion

	// Accelerates towards the desired target
	void AccelerateTowards (Vector3 targetPosition) {
		
		// Turn towards our current waypoint
		Quaternion newRotation; // New direction I will face when we are done calculating
		Quaternion goalRotation; // The rotation towards my waypoint

		// Find the vector from the AI to the target
		Vector3 vectorFromAIToTarget = targetPosition - aiTank.tf.position;

		// Find a quaternion that looks down that vector
		goalRotation = Quaternion.LookRotation(vectorFromAIToTarget);

		// Find a rotation "partway to" that goal rotation
		newRotation = Quaternion.RotateTowards( aiTank.tf.rotation, goalRotation, aiTank.turnSpeed * Time.deltaTime);

		// Turn down that new rotation
		aiTank.tf.rotation = newRotation;

		// Move forward
		if (CanMoveForward (clearanceDistance)) {
			aiTank.mover.Accelerate (aiTank.forwardSpeed);
		} else {
			ChangeAvoidState (AIAvoidanceStates.TurnToAvoid);			
		}
	}

	#region AI senses

	// Assign the target to the AI
	public void FindTargets ()
	{
		if (GameManager.instance.listOfTanks != null) 
		{
			// Sift through the tank list
			for ( int i = 0; i < GameManager.instance.listOfTanks.Count; i++ ) 
			{
				if (GameManager.instance.listOfTanks [i] != null) 
				{
					tankTag = GameManager.instance.listOfTanks [i].gameObject.GetComponent<Tag> ();

					// If player one is found
					if ( tankTag.isPlayerOne == true ) 
					{
						// Make them the target
						target = GameManager.instance.listOfTanks [i];
					}
				}
			}
		}
	}

	// Function for simply changing our AI's state
	public void ChangeAIState (AIStates newState) {

		// reset time in state
		timeInCurrentState = 0;

		ChangeAvoidState (AIAvoidanceStates.Normal);
		currentState = newState;
	}

	public void ChangeAvoidState (AIAvoidanceStates newAvoidState) {
		avoidState = newAvoidState;
		timeInAvoidState = 0;
	}

	// Function for checking if the tank can move forward
	public bool CanMoveForward ( float distance ) {
		
		RaycastHit hit;
//		Physics.SphereCast (data.tf.position, sphereCastRadius, data.tf.forward, out hit, distance, avoidMask); 
		Physics.Raycast (aiTank.tf.position, aiTank.tf.forward, out hit, distance, avoidMask); 
		if (hit.collider != null) {
			return false;
		}
		return true;
	}

	public bool CanHear (GameObject target) {
		if (target != null) {
			if (Vector3.Distance (target.GetComponent<Transform> ().position, aiTank.tf.position) <= hearingDistance) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	public bool CanSee (GameObject target) {
		
		Vector3 vectorToTarget = target.GetComponent<Transform> ().position - aiTank.tf.position;
		float angleToTarget = Vector3.Angle (aiTank.tf.forward, vectorToTarget);

		// If target is within FOV
		if (angleToTarget <= fieldOfView) {

			// Define the ray by storing an empty Ray() into the new Ray object. Used to check for obstructions
			Ray obstructionCheckRay = new Ray();
			obstructionCheckRay.origin = aiTank.tf.position;
			obstructionCheckRay.direction = aiTank.tf.forward;

			RaycastHit hitInfo;

			// AND if they are within our view distance
			if (Physics.Raycast (obstructionCheckRay, out hitInfo, viewDistance)) {
				
				// AND if the first thing our raycast hits is the target tank
				if (hitInfo.collider.gameObject == target) {

//					render.material = seenMaterial;
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	#endregion

	#region AI state functions
	// Do all the work for the Idle state
	void DoIdle () {
		// Do nothing
	}

	// TODO: Do all the work for the Patrol state
	void DoPatrol()	{
		
		// Keep track of how long I've been in the current avoidance state
		timeInAvoidState += Time.deltaTime;

		switch (avoidState) {
		case AIAvoidanceStates.Normal: // When in normal state..
			
			// ..accelerate towards the current waypoint when in a normal state
			AccelerateTowards (waypoints[currentWaypoint].GetComponent<Transform> ().position );
			break;

		case AIAvoidanceStates.TurnToAvoid: // When in turn to avoid state..

			// .. if the clearance distance isn't clear..
			if (!CanMoveForward (clearanceDistance)) {

				// .. rotate until it is clear
				aiTank.mover.TurnRight (aiTank.turnSpeed); // TODO: Add functionality for rotating in any direction
			} else {

				// If it is clear then move forward
				ChangeAvoidState (AIAvoidanceStates.MoveToAvoid);
			}
			break;

		case AIAvoidanceStates.MoveToAvoid:

			// Move forward for (clearanceTime) seconds
			AccelerateTowards (aiTank.tf.position + aiTank.tf.forward);
			if ( timeInAvoidState >= clearanceTime ) 
			{
				ChangeAvoidState (AIAvoidanceStates.Normal); // return to normal state
			}
			break;
		}
		
		// If we reach our waypoint, move to next waypoint
		if (Vector3.Distance (aiTank.tf.position, waypoints [currentWaypoint].position) < closeEnough) {

			// If I am a random patroller, move to a random waypoint
			if (patrolType == PatrolTypes.Random) {
				currentWaypoint = Random.Range (0, waypoints.Count);
			}
			// Otherwise
			else {
				// Set our "current" waypoint to the next waypoint in the array
				currentWaypoint += patrolDirection;

				// Check if past last waypoint AND moving forward
				if (currentWaypoint >= waypoints.Count && patrolDirection == 1) {

					// If I am a loop Patrol Type
					if (patrolType == PatrolTypes.Loop) {						
						// Then loop back to the zeroth waypoint
						currentWaypoint = 0;

					} else if (patrolType == PatrolTypes.Stop) {						
						// Then change to idle state
						ChangeAIState (AIStates.Idle);

					} else if (patrolType == PatrolTypes.Random) {
						// Do nothing!
					} else if (patrolType == PatrolTypes.PingPong) {
						patrolDirection = -1;
						currentWaypoint += patrolDirection; // Make sure to go back to previous value
					}
				} // Otherwise, if I am past the first waypoint and moving backward
				else if (currentWaypoint < 0 && patrolDirection == -1) {
					if (patrolType == PatrolTypes.PingPong) {
						patrolDirection = 1;
						currentWaypoint += patrolDirection;
					}
				}
			}
		}
	}

	void DoEngage(GameObject target) {

		// Turn towards our current waypoint
		Quaternion newRotation; // New direction I will face when we are done calculating
		Quaternion goalRotation; // The rotation towards my waypoint

		// Find the vector from the AI to the target
		Vector3 vectorFromAIToTarget = target.GetComponent<Transform>().position - aiTank.tf.position;

		// Find a quaternion that looks down that vector
		goalRotation = Quaternion.LookRotation(vectorFromAIToTarget);

		// Find a rotation "partway to" that goal rotation
		newRotation = Quaternion.RotateTowards( aiTank.tf.rotation, goalRotation, aiTank.turnSpeed * Time.deltaTime);

		// Turn down that new rotation
		aiTank.tf.rotation = newRotation;

		if ( CanSee(target) ) {

			// The player's fire rate is limited. Designers can set the number of seconds between shots in the inspector.
			if (Time.time >= aiTank.nextShotTime) {

				shooterComponent.Shoot (); //Then fire!

				// Set the time for the next shot
				aiTank.nextShotTime = Time.time + aiTank.fireRate;
			}
		} else if ( !CanHear(target) ) {
			ChangeAIState (AIStates.Idle);
		}
	}

	// Do work for the chase state
	void DoChase () {

		// Keep track of how long I've been in the current avoidance state
		timeInAvoidState += Time.deltaTime;

		switch (avoidState) {
		case AIAvoidanceStates.Normal:
			if (target != null) {

				// Accelerate towards the target's position
				AccelerateTowards (target.GetComponent<Transform> ().position);
				CanSee (target);
			} else {
				ChangeAIState (AIStates.Idle);
			}
			break;
		case AIAvoidanceStates.TurnToAvoid:
			if (!CanMoveForward (clearanceDistance)) {
				aiTank.mover.TurnRight (aiTank.turnSpeed); // TODO: Add functionality for rotating in any direction
			} else {
				ChangeAvoidState (AIAvoidanceStates.MoveToAvoid);
			}
			break;
		case AIAvoidanceStates.MoveToAvoid:
			AccelerateTowards (aiTank.tf.position + aiTank.tf.forward);

			if (timeInAvoidState >= clearanceTime) {
				ChangeAvoidState (AIAvoidanceStates.Normal);
			}
			break;
		}
	}

	void DoFlee() {
		
		// Keep track of how long I've been in the current avoidance state
		timeInAvoidState += Time.deltaTime;

		// Find vector from the AI to the target
		Vector3 vectorFromAIToTarget = target.GetComponent<Transform>().position - aiTank.tf.position;

		// Make a vector pointing in the opposite direction
		Vector3 vectorAwayFromTarget = -vectorFromAIToTarget;

		//Make it a size of fleeDistance
		vectorAwayFromTarget = vectorAwayFromTarget.normalized * fleeDistance;

		// Find a point that (direction + distance) from my current location
		Vector3 targetPoint = aiTank.transform.position + vectorAwayFromTarget;

		switch (avoidState) {
		case AIAvoidanceStates.Normal:
			if (target != null) {
				
				isFleeing = true;

				// Accelerate towards the targetpoint set above
				AccelerateTowards (targetPoint);
			} else {
				ChangeAIState (AIStates.Idle);
			}
			break;
		case AIAvoidanceStates.TurnToAvoid:
			if (!CanMoveForward (clearanceDistance)) {
				aiTank.mover.TurnRight (aiTank.turnSpeed); // TODO: Add functionality for rotating in any direction
			} else {
				ChangeAvoidState (AIAvoidanceStates.MoveToAvoid);
			}
			break;
		case AIAvoidanceStates.MoveToAvoid:
			AccelerateTowards (aiTank.tf.position + aiTank.tf.forward);

			if (timeInAvoidState >= clearanceTime) {
				ChangeAvoidState (AIAvoidanceStates.Normal);
			}
			break;
		}
	}
	#endregion
}
