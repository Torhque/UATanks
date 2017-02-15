using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public GameObject doorNorth;
	public GameObject doorSouth;
	public GameObject doorEast;
	public GameObject doorWest;

	// Store the room's waypoints in a list
	public List<Transform> roomWaypointsList; 

	// Store the room's spawn points in a list
	public List<Transform> spawnPointsList;

	void Start () {
		AddWaypoints ();
		AddSpawnPoints ();
	}

	public void AddWaypoints () {

		// Create a List of type Tag
		List<Tag> tagList = new List<Tag>();

		// Grab the children that have the Tag script attached
		tagList.AddRange( GetComponentsInChildren<Tag>() ); 

		// Loop through Tag List
		for (int i = 0; i < tagList.Count; i++) {

			// Transform object for storing the incoming waypoint
			Transform tempWaypoint; 

			// If the object at index [i] is a waypoint
			if (tagList[i].isWaypoint == true) 
			{
				// Store the waypoint (actually its Transform)
				tempWaypoint = tagList [i].GetComponent<Transform> ();

				// Add that waypoint (its Transform) to the roomWaypointsList
				roomWaypointsList.Add(tempWaypoint);
			}
		}
	}

	public void AddSpawnPoints()
	{
		// Create a list of type Tag
		List<Tag> spawnTagList = new List<Tag> ();

		// Grab the children that have the Tag script attached
		spawnTagList.AddRange ( GetComponentsInChildren<Tag>() );

		// Loop through the Tag List
		for (int i = 0; i < spawnTagList.Count; i++) {

			// Transform object for storing the incoming spawn point
			Transform tempSpawnpoint;

			// If the object at index [i] is a spawnpoint
			if (spawnTagList [i].isSpawnPoint == true) 
			{				
				// Store the spawnpoint's Transform
				tempSpawnpoint = spawnTagList[i].GetComponent<Transform> ();

				// Add that spawnpoint's Transform to the spawnPointList
				spawnPointsList.Add(tempSpawnpoint);
			}
		}
	}
}