using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
	
	public GameObject[] tankPrefabsArray;
	public GameObject playerOnePrefab;
	public GameObject playerTwoPrefab;
	private GameObject tankToSpawn;
	public float respawnTime;
	private float timer;
	public TankData tank;

	public int maxTanks; // Setting the max amount of tanks

	int random; // variable for storing a random value

	private Transform tf;

	// Use this for initialization
	void Start () 
	{
		timer = respawnTime;
		tf = GetComponent<Transform> ();
	}

	// Update is called once per frame
	void Update () 
	{
		// Check the spawner's children for tanks
		tank = GetComponentInChildren<TankData> ();

		// Set the max tanks variable to the Game Manager's value
		maxTanks = GameManager.instance.maxTanksGM;

		// If nothing is spawned
		if (tankToSpawn == null) 
		{
			// If the current number of tanks is less than the desired max
			if (GameManager.instance.listOfTanks.Count < maxTanks) 
			{			
				// Countdown the timer
				timer -= Time.deltaTime;

				// If timer expired
				if (timer <= 0) 
				{
					GameObject tankToInstantiate;

					// If the game is set to two players
					if (GameManager.instance.numPlayers == 2)
					{
						// If Player One hasn't spawned yet..
						if (GameManager.instance.playerOneHasSpawned == false)
						{
							// ..spawn spawn Player One
							tankToInstantiate = playerOnePrefab;
							GameManager.instance.playerOneHasSpawned = true;
							GameManager.instance.tempAudioListener.gameObject.SetActive (false);

							// If there is no tank child object (no tank spawned here yet)
							if (tank == null) 
							{
								// Spawn one
								SpawnTank (tankToInstantiate);
							}
						}
						if (GameManager.instance.playerTwoHasSpawned == false) 
						{
							// If Player Two hasn't spawned yet..
							if (GameManager.instance.playerTwoHasSpawned == false) 
							{
								// .. spawn Player Two
								tankToInstantiate = playerTwoPrefab;
								GameManager.instance.playerTwoHasSpawned = true;

								// If there is no tank child object (no tank spawned here yet)
								if (tank == null) 
								{
									// Spawn one
									SpawnTank (tankToInstantiate);
								}
							}
						}
//						if (GameManager.instance.playerOneHasSpawned == true && GameManager.instance.playerOneHasSpawned == true) 
//						{
//							// Select a random tank from the array of AI tanks
//							tankToInstantiate = tankPrefabsArray [ Random.Range ( 0, tankPrefabsArray.Length) ];
//
//							// If there is no tank child object (no tank spawned here yet)
//							if (tank == null) {
//								SpawnTank (tankToInstantiate);
//							}
//						}
					}
					// If Player One hasn't spawned yet..
					if (GameManager.instance.playerOneHasSpawned == false)
					{
						{
							// .. spawn spawn Player One
							tankToInstantiate = playerOnePrefab;
							GameManager.instance.playerOneHasSpawned = true;
							GameManager.instance.tempAudioListener.gameObject.SetActive (false);

							// If there is no tank child object (no tank spawned here yet)
							if (tank == null) 
							{
								// Spawn one
								SpawnTank (tankToInstantiate);
							}
						}
					}

					else 
					{
						tankToInstantiate = tankPrefabsArray [ Random.Range ( 0, tankPrefabsArray.Length) ];

						// If there is no tank child object (no tank spawned here yet)
						if (tank == null) {
							SpawnTank (tankToInstantiate);
						}
					}

					// Reset the timer
					timer = respawnTime;
				}
			}
		}
	}

	void SpawnTank (GameObject tankToInstantiate) 
	{
		// Spawn a random tank from the array
		tankToSpawn = Instantiate (tankToInstantiate, tf.position, tf.rotation) as GameObject;

		// Set its parent to the Spawner's parent (which is the Room object)
		tankToSpawn.transform.parent = this.transform;

		// Add the tank to the Game Manager's list of tanks
		GameManager.instance.listOfTanks.Add (tankToSpawn);
	}
}