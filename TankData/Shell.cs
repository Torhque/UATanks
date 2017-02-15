using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Shell : MonoBehaviour {

	[HideInInspector] public TankData owner;
	Tag objectTag;

	// Bullets are destroyed after a timeout period that can be set by designers in the inspector.
	public float shellLifespan;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, shellLifespan);

		objectTag = GetComponent<Tag> ();
	}

	// The shells are able to hit and destroy enemy tanks.
	void OnCollisionEnter (Collision hitInfo) {

		// Grab the target's Tag
		objectTag = hitInfo.gameObject.GetComponent<Tag> ();

		//	If the object the bullet is colliding with has a Tag
		if (objectTag != null) 
		{
			// And if the object is a player
			if (objectTag.isPlayer == true) {

				// Grab the target's health component for modifying health values
				Health targetTankHealth = hitInfo.gameObject.GetComponent<Health> (); 

				// Grab the target's tank data for accessing it's bounty value
				TankData targetTankData = hitInfo.gameObject.GetComponent<TankData> ();

				// Grab the owner's transform for playing the soundclip on the player
				Transform ownerTransform = owner.GetComponent<Transform> ();

				if (targetTankHealth != null) 
				{
					// Remove the appropriate amount of health from the target tank
					targetTankHealth.RemoveHealth (owner.firepower);

					// Play the shell impact sound
					AudioSource.PlayClipAtPoint (AudioManager.instance.shellHit, ownerTransform.position, AudioManager.instance.sfxVolume);

					// If the target is killed
					if (targetTankHealth.IsDead) {

						// Give the tank that killed it points
						owner.score += targetTankData.bounty;

						// Play a sound when the tank dies
						AudioSource.PlayClipAtPoint (AudioManager.instance.tankDeath, ownerTransform.position, AudioManager.instance.sfxVolume);

						// Check the owner's tag
						objectTag = owner.gameObject.GetComponent<Tag> ();

						// If the owner is Player One
						if (objectTag.isPlayerOne == true) 
						{
							// Update Player One score
							GameManager.instance.playerOneScore += targetTankData.bounty;
						}
						// If the owner is Player One
						if (objectTag.isPlayerTwo == true) 
						{
							// Update Player Two score
							GameManager.instance.playerTwoScore += targetTankData.bounty;
						}
						
						// (maybe move this up) Check the target tank's tag
						objectTag = targetTankData.gameObject.GetComponent<Tag> ();

						if (objectTag.isPlayerOne == true) 
						{
							GameManager.instance.playerOneHasSpawned = false;
							GameManager.instance.tempAudioListener.gameObject.SetActive (true);
						}
						if (objectTag.isPlayerTwo == true) 
						{
							GameManager.instance.playerTwoHasSpawned = false;
						}
					}
				} 
				else 
				{
					Debug.Log ("Player Object does not have a Health script attached.");
				}
			}
		}
		GameManager.instance.CheckHighScore ();
		Destroy (gameObject);
	}
}