using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour 
{
	// The health of a tank (both player tanks and enemy tanks) is available for designers to edit in the inspector.
	[SerializeField]
	private float currentHealth;

	private Tag currentTankTag;

	string gameOverScene = "Game Over";

	public float CurrentHealth
	{
		get 
		{
			return currentHealth;
		}
		// Set the current health to the assigned value
		private set {
			currentHealth = value;

			// If current health hits or falls below zero, die
			if ( currentHealth <= 0 ) 
			{
				Die ();

				// If lives reaches zero
				if (GameManager.instance.numPlayers == 2) 
				{
					if (GameManager.instance.playerOneLives <= 0 && GameManager.instance.playerTwoLives <= 0) 
					{
						PlayerPrefs.SetFloat ("highScore", GameManager.instance.highScore);
						SceneManager.LoadScene ("Game Over"); // Load game over scene
						GameManager.instance.currentLevel = gameOverScene; // play fail sound
						AudioManager.instance.musicPlayer.clip = AudioManager.instance.gameOverClip; // Swap the audio clip
						AudioManager.instance.musicPlayer.Play (); // Play the clip
						AudioManager.instance.musicPlayer.loop = false;
						PlayerPrefs.Save ();
					}
				}
				else if (GameManager.instance.playerOneLives <= 0) 
				{
					// Set the high score
					PlayerPrefs.SetFloat ("highScore", GameManager.instance.highScore);

					SceneManager.LoadScene ("Game Over"); // Load game over scene
					GameManager.instance.tempAudioListener.gameObject.SetActive (true);
					GameManager.instance.currentLevel = gameOverScene; // play fail sound
					AudioManager.instance.musicPlayer.clip = AudioManager.instance.gameOverClip; // Swap the audio clip
					AudioManager.instance.musicPlayer.Play (); // Play the clip
					AudioManager.instance.musicPlayer.loop = false;
					PlayerPrefs.Save ();
				}
			}
		}
	}

	// Bool for setting whether or not the tank is dead
	public bool IsDead { get; private set; }

	// Function for removing (damage) from current health
	public void RemoveHealth ( float damage ) 
	{
		CurrentHealth -= damage;
	}

	public void AddHealth ( float amount ) 
	{
		CurrentHealth += amount;
	}

	// Function for dying and setting the IsDead bool from above to true
	void Die()
	{
		currentTankTag = this.gameObject.GetComponent<Tag> ();

		Destroy ( gameObject );
		IsDead = true;

		// If the dying tank is Player One
		if (currentTankTag.isPlayerOne == true) 
		{
			// Subtract one life from Player One
			GameManager.instance.playerOneLives -= 1;
		}
		// If the dying tank is Player Two
		if (currentTankTag.isPlayerTwo == true) 
		{
			// Subtract a life from Player Two
			GameManager.instance.playerTwoLives -= 1;
		}
	}
}