using UnityEngine;
using System.Collections;

[System.Serializable] // Make it show up in Inspector!
public class Powerup {
	
	public float duration; 
	public string name;

	public virtual void Apply ( TankData tankData ) {
		Debug.Log ("TankData Apply!");
	}

	public virtual void Remove ( TankData tankData ) {
		Debug.Log ("TankData Remove!");
	}

	public virtual void Apply ( Health tankHealth ) {
		Debug.Log ("Health Apply!");
	}

	// REMOVED TO MAKE IT A PERMANENT POWERUP
//	public virtual void Remove ( Health tankHealth ) {
//		Debug.Log ("Health Apply!");
//	}
}
