using UnityEngine;
using System.Collections;

[System.Serializable] // Make it show up in Inspector!
public class FirepowerPowerup : Powerup {

	public float firepowerBonus;

	public override void Apply ( TankData tankData ) {
		tankData.firepower += firepowerBonus;
	}

	public override void Remove ( TankData tankData ) {
		tankData.firepower -= firepowerBonus;
	}
}
