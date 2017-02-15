using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Require that object has TankData, else make one
[RequireComponent(typeof(TankData))]

public class PlayerController : MonoBehaviour {

	public PlayerData player;
	[HideInInspector]public TankData data;
	public InputScheme inputType;

	public Shooter shooterComponent;


	// Use this for initialization
	void Start () {

		// Load the appropriate components
		data = GetComponent<TankData> ();
		shooterComponent = gameObject.GetComponent<Shooter> ();
	}
	
	// Update is called once per frame
	void Update () {

		// Process key presses
		switch (inputType) {			
		case InputScheme.WASD:	
			ProcessWASD ();
			break;

		case InputScheme.Arrows:
			ProcessArrows ();
			break;
		}

	}

	//The player uses the WASD keys to drive the tank.  
	//The player will press W to move forward, S to move in reverse, and A & D to turn left and right. 
	public void ProcessWASD () 
	{
		
		if (Input.GetKey (KeyCode.W)) // Move forward
		{
			if (data.mover != null) 
			{
				data.mover.Accelerate (data.forwardSpeed); // Trigger the Move function [at desired speed]
			}
		}
		if (Input.GetKey (KeyCode.S)) // Move backward
		{
			if (data.mover != null) 
			{
				data.mover.Reverse (data.reverseSpeed);
			}
		}
		if (Input.GetKey (KeyCode.D)) // Turn right
		{
			if (data.mover != null) 
			{
				data.mover.TurnRight (data.turnSpeed);
			}
		}
		if (Input.GetKey (KeyCode.A)) // Turn left
		{
			if (data.mover != null) 
			{
				data.mover.TurnLeft (data.turnSpeed);
			}
		}

		// Return to the Start Screen when the user hits Escape
//		if (Input.GetKey (KeyCode.Escape)) 
//		{
//			SceneManager.LoadScene ("Start Screen");
//			GameManager.instance.currentLevel = "Start Screen";
//			AudioManager.instance.musicPlayer.clip = AudioManager.instance.menuSong; // Swap the song
//			AudioManager.instance.musicPlayer.Play (); // Play the new song
//			GameManager.instance.playerOneHasSpawned = false;
//			GameManager.instance.tempAudioListener.gameObject.SetActive (true);
//		}

		// The player will press the space bar to fire a "shell" in the direction their tank is facing.
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			if (Time.time >= data.nextShotTime) {
				
				// The player's fire rate is limited. Designers can set the number of seconds between shots in the inspector.
				shooterComponent.Shoot ();

				// Set the time for the next shot
				data.nextShotTime = Time.time + data.fireRate;
			}
		}
	}

	// Same idea as above but with Arrows keys and KeyPad0 for fire instead of Space
	public void ProcessArrows() 
	{
		if (Input.GetKey (KeyCode.UpArrow)) // Move forward
		{
			if (data.mover != null) {
				data.mover.Accelerate (data.forwardSpeed); // Trigger the Move function [at desired speed]
			}
		}
		if (Input.GetKey (KeyCode.DownArrow)) // Move backward
		{
			if (data.mover != null) {
				data.mover.Reverse (data.reverseSpeed);
			}
		}
		if (Input.GetKey (KeyCode.RightArrow)) // Turn right
		{
			if (data.mover != null) 
			{
				data.mover.TurnRight (data.turnSpeed);
			}
		}
		if (Input.GetKey (KeyCode.LeftArrow)) // Turn left
		{
			if (data.mover != null) 
			{
				data.mover.TurnLeft (data.turnSpeed);
			}
		}

		// The player will press the KeyPad0 to fire a "shell" in the direction their tank is facing.
		if (Input.GetKeyDown (KeyCode.Keypad0)) 
		{
			if (Time.time >= data.nextShotTime) {

				// The player's fire rate is limited. Designers can set the number of seconds between shots in the inspector.
				shooterComponent.Shoot ();

				// Set the time for the next shot
				data.nextShotTime = Time.time + data.fireRate;
			}
		}
	}
}

public enum InputScheme { WASD, Arrows};
