using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

	// Display number of lives on the UI
	public Text numLivesUIText;
	public Text scoreUIText;
	public Text highScoreUIText;
	[HideInInspector]public Tag currentTankTag;
	public TankData currentTank;

	// Use this for initialization
	void Start () 
	{
		currentTankTag = GetComponent<Tag> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Set the player scores and lives at Start
		SetPlayerScores ();
		SetPlayerLives ();
		DisplayHighScore ();
	}

	void SetPlayerLives() 
	{
		for (int i = 0; i < GameManager.instance.listOfTanks.Count; i++) 
		{
			// Grab the tag of the current tank this script is attached to
			currentTankTag = this.gameObject.GetComponent<Tag> ();

			// If the current tank is player one
			if (currentTankTag.isPlayerOne == true) 
			{
				// Display player one's lives
				numLivesUIText.text = "Lives: " + GameManager.instance.playerOneLives.ToString ();
			}

			// If the current tank is player two
			if (currentTankTag.isPlayerTwo == true) 
			{
				// Display player two's lives
				numLivesUIText.text = "Lives " + GameManager.instance.playerTwoLives.ToString();
			}
		}
	}

	void SetPlayerScores()
	{
		// Iterate through the tank list
		for ( int i = 0; i < GameManager.instance.listOfTanks.Count; i++ ) 
		{
			currentTankTag = this.gameObject.GetComponent<Tag> ();

			// If the current tank is Player One
			if (currentTankTag.isPlayerOne == true) 
			{
				// Update the score UI for Player One
				scoreUIText.text = "Score " + GameManager.instance.playerOneScore.ToString ();

				//Debug.LogFormat ("Current tank: {0}\tCurrent tank's Tag: {1}\tCurrent tank's score: {2}  ", currentTank, currentTankTag.isPlayerOne, currentTank.score);
			}

			// If the current tank is Player Two
			if (currentTankTag.isPlayerTwo == true) 
			{
				// Update the score UI text for Player Two
				scoreUIText.text = "Score " + GameManager.instance.playerTwoScore.ToString ();
			}
		}
	}

	// Function for displaying the high score on the HUD
	void DisplayHighScore() 
	{
		highScoreUIText.text = "High Score: " + GameManager.instance.highScore.ToString();
	}
}