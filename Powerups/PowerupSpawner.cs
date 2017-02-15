using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour {

	public GameObject prefabToSpawn;
	private GameObject currentSpawn;
	public float respawnTime;
	private float timer;

	private Transform tf;

	// Use this for initialization
	void Start () {
	
		timer = respawnTime;
		tf = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {

		// If nothing is spawned
		if (currentSpawn == null) {
			// Countdown the timer
			timer -= Time.deltaTime;
			// If timer expired
			if (timer <= 0) {
				// tf.rotation preserves spawner's rotation
				currentSpawn = Instantiate (prefabToSpawn, tf.position, tf.rotation) as GameObject;
				// rest our timer
				timer = respawnTime;
			}
		}
	}
}
