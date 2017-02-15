using UnityEngine;
using System.Collections;

public class Tag : MonoBehaviour {

	public static Tag instance;

	public bool isWaypoint; // Tag for waypoint objects
	public bool isSpawnPoint;
	public bool isPlayer; // Tag for player objects
	public bool isPlayerOne;
	public bool isPlayerTwo;
	public bool isAITank;
	public bool isFirepowerPU;
	public bool isHealthPU;
	public bool isFireRatePU;

	void Awake () {


	}
}
