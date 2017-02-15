using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	// There is a Game Manager singleton that keeps track of important game data and manages the flow of the game.
	[HideInInspector]public static GameManager instance;

//	Game Manager Singleton exists and allows easy access to player object and an array of enemy tank objects.
	public AudioListener tempAudioListener;

	[Space(20)]

	public List<GameObject> listOfTanks;
	public bool playerOneHasSpawned = false;
	public bool playerTwoHasSpawned = false;
	public int maxTanksGM;

	public Tag objectTag;

	public float highScore;
	public float playerOneScore;
	public float playerTwoScore;
	public int explorerTankScore;
	public int hunterTankScore;
	public int cowardTankScore;

	public List<Transform> spawnPointsGM;

	// Variables for camera management
	public GameObject[] players;
	public int numPlayers;

	[Space(10)]

	public int tankAINumLives;
	public int playerOneLives;
	public int playerTwoLives;
	public int score;

	[Space(10)]

	// Variables for manipulating the level generation
	[Header("Seed Data (zero means random every time)")]
	public int mapSeed;
	public bool mapOfTheDay;

	// Variable for keep track of the current level (scene)
	public string currentLevel = "Start Screen";

	void Awake () 
	{
		// Set up the Singleton
		// If there is NOT an instance
		if ( instance == null ) 
		{
			// then this is the instance
			instance = this;
			// Don't destroy when I change scenes
			DontDestroyOnLoad (gameObject);
		} 
		else 
		{			
			// Otherwise, there is one, so destroy new one
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		objectTag = GetComponent<Tag> ();

		numPlayers = PlayerPrefs.GetInt ("pp_numPlayers");
		mapSeed = PlayerPrefs.GetInt ("mapSeedEntry");
		highScore = PlayerPrefs.GetFloat ("highScore");
//		AdjustCameras ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		TrimListOfTanks (); // Trim the list if a tank is destroyed (there's gotta be a more efficient way..)
	}

	public void AdjustCameras () 
	{

		// Loop through the list of players
		for ( int i = 0; i < players.Length; i++ ) 
		{
			// Get the camera for the current tank
			Camera theCam;
			theCam = players[i].GetComponentInChildren<Camera> ();

			// If that player is active (based on numPlayers)
			if ( i < numPlayers ) 
			{
				// Set the appropriate values
				float x = (float)i / (float)numPlayers; // (float) for preventing weird rounding with integers
				float y = 0; // Just gonna be at the top
				float width = 1.0f / (float)numPlayers; // So each player has the same camera width
				width -= 0.001f; // For visual clarity (creates a black bar)
				float height = 1; // For regular full height

				players [i].SetActive (enabled);

				// Apply them to the camera's rectangle view
				theCam.rect = new Rect (x, y, width, height);
			} 
			else 
			{
				// Turn the whole tank off
				players[i].SetActive(false);
			}
		}
	}

	// Check to see if the list of tanks has updated
	public void TrimListOfTanks () 
	{
		// Iterate through the tank list
		for ( int i = 0; i < listOfTanks.Count; i++ ) 
		{
//			if (listOfTanks [i] != null) 
//			{
				// If the item and index [i] is null (destroyed)
				if ( listOfTanks [i].gameObject == null ) 
				{
					// Remove that index from the List
					listOfTanks.RemoveAt (i);
					listOfTanks.TrimExcess (); // Trim the list down
				}
//			}
		}
	}

//	for ( int i = 0; i < GameManager.instance.listOfTanks.Count; i++ ) 
//	{
//		if ( GameManager.instance.listOfTanks [i].gameObject == null ) 
//		{
//			GameManager.instance.listOfTanks.RemoveAt (i);
//			GameManager.instance.listOfTanks.TrimExcess ();
//			Debug.Log (GameManager.instance.listOfTanks.Count);
//		}
//	}

	public void CheckHighScore () 
	{
		if ( playerOneScore >= highScore ) 
		{
			highScore = playerOneScore;
		} 
		if ( playerTwoScore >= highScore ) 
		{
			highScore = playerTwoScore;
		}
	}
} 