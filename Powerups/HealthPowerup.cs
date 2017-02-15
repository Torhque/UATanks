using UnityEngine;
using System.Collections;

[System.Serializable] // Make it show up in Inspector!
public class HealthPowerup : Powerup {

	public float healthBonus;

	public override void Apply ( Health tankHealth ) 
	{
		tankHealth.AddHealth(healthBonus);
	}

	// Remove this if you want it to be permanent
//	public override void Remove ( Health tankHealth ) {
//		tankHealth.RemoveHealth(healthBonus);
//		Debug.Log ("Health override Remove!");
//	}
}
