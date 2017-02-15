using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public GameObject shellPrefab;
	[HideInInspector] public int numShells; // Keep track of the # of bullets for organization/timeout reasons
	public Transform shellParent; // Transform for storing the temporary shells
	[HideInInspector]public Transform tf;
	public float shootForce; // amount of force is available for designers in inspector

	public TankData tankData; // Create tankData variable for assigning the shell to a tank

	// Use this for initialization
	void Start () {

		tankData = GetComponent<TankData> ();
		tf = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		// Don't need to add keypress in update. This a global shoot function/component
	}

//	Shooter component fires shell in direction tank is facing. 
	public void Shoot() {

		GameObject tempShell;
		
		// Instantiate a bullet: the prefab, in front of the object, don't rotate
		GameObject theShell; // store the bullet in a variable
		tempShell = theShell = Instantiate (shellPrefab, tf.position + tf.forward, Quaternion.identity) as GameObject;

		// Play a sound for when the shell is fired
		AudioSource.PlayClipAtPoint (AudioManager.instance.tankFire, tf.position, AudioManager.instance.sfxVolume);

		// Create an object of type Shell, and assign the owner of the fired shell
		Shell firedShell = theShell.GetComponent<Shell> ();
		firedShell.owner = tankData;

		// The firing of shells uses Unity's Rigidbody physics system.
		// Get the rigidbody of the bullet
		Rigidbody bulletRigidBody;
		bulletRigidBody = theShell.GetComponent<Rigidbody> ();

		//The amount of force at which the "shell" is pushed is available for designers to edit in the inspector.
		// Add force in the forward direction
		bulletRigidBody.AddForce(tf.forward * shootForce);
		numShells++; // Add to the number of shells after a shell is shot to keep track of them

		// For each bullet that exists in the world, use the naming convention Shell[i], and store it in the Shells
		//	parent object for organization.
		for (int i = 0; i < numShells; i++) 
		{
			tempShell.name = "Shells" + i.ToString ();
			tempShell.GetComponent<Transform> ().parent = shellParent;
		}
	}
}
