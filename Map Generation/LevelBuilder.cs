using UnityEngine; // Has its own random
using System.Collections;
using System; // also has its own random
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour {

	public int columns;
	public int rows;
	public float tileWidth = 50f;
	public float tileHeight = 50f;

	public Room[,] grid;
	public GameObject[] roomPrefabs;

	// Use this for initialization
	void Start () {

		if (GameManager.instance.mapOfTheDay == true) {
			UnityEngine.Random.InitState ((int)DateTime.Now.Date.Ticks); // Updates the seed when the day changes
		} 
		else if (GameManager.instance.mapOfTheDay == false) {
			if (GameManager.instance.mapSeed == 0) {
				UnityEngine.Random.InitState ((int)DateTime.Now.Ticks);
			} else {
				UnityEngine.Random.InitState (GameManager.instance.mapSeed);
			}
		}

		// Call the create level function
		CreateLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		
		// Temporary if statement for returning to the main menu
//		if (Input.GetKey (KeyCode.Escape)) 
//		{
//			SceneManager.LoadScene ("Start Screen");
//			GameManager.instance.currentLevel = "Start Screen";
//			AudioManager.instance.musicPlayer.clip = AudioManager.instance.menuSong; // Swap the song
//			AudioManager.instance.musicPlayer.Play (); // Play the new song
//		}
	}

	public void CreateLevel () {
		
		// Resize the grid array so it will fit our number of rooms
		grid = new Room[columns, rows];

		// For each column
		for (int currentColumn = 0; currentColumn < columns; currentColumn++) {			
			// For each row
			for (int currentRow = 0; currentRow < rows; currentRow++) {
				
				// Instantiate a room
				GameObject currentRoom = Instantiate ( GetRandomRoom() ) as GameObject;

				// Give it a meaninful name (column, row) e.g. (7, 3)
				currentRoom.name = "Room (" + currentColumn + "," + currentRow + ")";

				// Move it into the correct position
				Vector3 tilePosition = new Vector3 (currentColumn * tileWidth, 0, currentRow * tileHeight);
				currentRoom.GetComponent<Transform> ().position = tilePosition;

				// Make it child of the level object
				currentRoom.GetComponent<Transform>().parent = gameObject.GetComponent<Transform>();

				// TODO: Open the correct doors
				Room room = currentRoom.GetComponent<Room>();

				if (currentRow != 0) {
					room.doorSouth.SetActive (false);
				}
				if (currentRow != rows - 1) {
					room.doorNorth.SetActive (false);
				}
				if (currentColumn != 0) {
					room.doorWest.SetActive (false);
				}
				if (currentColumn != columns - 1) {
					room.doorEast.SetActive (false);
				}

				// Add it to the grid array
				grid[ currentColumn, currentRow ] = room;
			}
		}
	}

	public GameObject GetRandomRoom () {
		
		// Return a random room
		return roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Length)];
	}
}
