using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	Tag powerupTag;

	public HealthPowerup healthPowerup;
	public FirepowerPowerup firepowerPowerup;
	public FireRatePowerup fireRatePowerup;

	public AudioClip sound;
	private Transform tf;

	// Use this for initialization
	void Start () {
		tf = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter ( Collider other ) {

		PowerupController otherPUC; // PUC = PowerupController
		otherPUC = other.GetComponent<PowerupController>();

		if (otherPUC == null) {
			// Then it doesn't have a powerup controller -- do nothing
		} 
		else {

			// Store the Tag of the current powerup
			powerupTag = this.gameObject.GetComponent<Tag> ();

			// If it is a health powerup
			if (powerupTag.isHealthPU == true) 
			{
				otherPUC.AddPowerup (healthPowerup); // Add the health powerup
//				Debug.Log ("Health PU added!");

				// Play sound (if it exists)
				if (sound != null) {
					AudioSource.PlayClipAtPoint (sound, tf.position, AudioManager.instance.sfxVolume);
				}
				Destroy (gameObject); // Destroy the powerup
			} 

			// If it is a firepower powerup
			else if (powerupTag.isFirepowerPU == true) 
			{
				otherPUC.AddPowerup (firepowerPowerup); // Add the firepower powerup
//				Debug.Log ("Firepower PU added!");

				// Play sound (if it exists)
				if (sound != null) {
					AudioSource.PlayClipAtPoint (sound, tf.position, AudioManager.instance.sfxVolume);
				}
				Destroy (gameObject); // Destroy the powerup
			}

			// If it is a fire rate powerup
			else if (powerupTag.isFireRatePU == true) 
			{
				otherPUC.AddPowerup (fireRatePowerup); // Add the fire rate powerup
//				Debug.Log ("Fire rate PU added!");

				// Play sound (if it exists)
				if (sound != null) {
					AudioSource.PlayClipAtPoint (sound, tf.position, AudioManager.instance.sfxVolume);
				}
				Destroy (gameObject); // Destroy the powerup
			}
		}
	}
}