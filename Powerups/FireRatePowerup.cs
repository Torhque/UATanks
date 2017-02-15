using UnityEngine;
using System.Collections;

[System.Serializable] // Make it show up in Inspector!
public class FireRatePowerup : Powerup {

	public float fireRateBonus;

	public override void Apply ( TankData tankData ) {
		tankData.fireRate -= fireRateBonus; // Boost the fire rate accordingly
	}

	public override void Remove ( TankData tankData ) {
		tankData.fireRate += fireRateBonus; // Remove the boost accordingly
	}
}
