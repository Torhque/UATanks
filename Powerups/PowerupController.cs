using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupController : MonoBehaviour {

	public List<Powerup> powerupsList;
	private TankData tankData;
	private Health tankHealth;

	void Start () {
		
		tankData = GetComponent<TankData> ();
		tankHealth = GetComponent<Health> ();
	}

	void Update () {

		CheckForExpiredPowerups ();

//		TEST STUFF
//		HealthPowerup powerup = new HealthPowerup ();
//		powerup.healthBonus = 100;

//		if ( Input.GetKeyDown (KeyCode.P) ) {
//			AddPowerup(powerup);
//		}
//		END TEST STUFF
	}

	public void CheckForExpiredPowerups () {

		// Make list of expired powerups
		List<Powerup> expiredPowerups = new List<Powerup>();

		// Look at each powerup on our controller
		foreach (Powerup powerup in powerupsList) {
			
			// Subtract from its timer
			powerup.duration -= Time.deltaTime;

			// if the timer hit zero
			if ( powerup.duration <= 0 ) {
				
				// add it to the list of expired powerups
				expiredPowerups.Add (powerup);
			}
		}

		// Go through the list of expired powerups and remove them from powerups
		foreach (Powerup powerup in expiredPowerups) {
			RemovePowerup (powerup);
		}
	}

	public void AddPowerup ( Powerup powerupToAdd ) {
		
		// Apply the powerup changes
		if (powerupToAdd.name == "FirepowerPU") {
			powerupToAdd.Apply (tankData);
		}
		if (powerupToAdd.name == "HealthPU") {
			powerupToAdd.Apply (tankHealth);
		}
		if (powerupToAdd.name == "FireRatePU") {
			powerupToAdd.Apply (tankData);
		}

		// Add the powerup to a list of powerups
		// *** Do not add it to the list if you want it to be permanent
		powerupsList.Add ( powerupToAdd );
	}

	public void RemovePowerup ( Powerup powerupToRemove ) {

		// If it is a firepower powerup
		if (powerupToRemove.name == "FirepowerPU") {
			powerupToRemove.Remove(tankData); // Use the appropriate Tank Data parameter
		}
		if (powerupToRemove.name == "FireRatePU") {
			powerupToRemove.Remove (tankData); // use the appropriate tank data parameter
		}

		// REMOVED TO MAKE IT PERMANENT POWERUP - If it is a health powerup
//		if (powerupToRemove.name == "HealthPU") {
//			powerupToRemove.Remove(tankHealth); // Use the appropriate Health parameter
//		}

		// Remove the powerup from the list
		powerupsList.Remove (powerupToRemove);
	}
}
