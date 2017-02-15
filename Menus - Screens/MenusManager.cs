using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour {	

	// Inputfield object for storing the user's input
	public InputField mapSeedEntry; 
	public Scrollbar musicScrollbar;
	public Scrollbar sfxScrollbar;

	string optionsMenu = "Options Menu";
	string startScreen = "Start Screen";
	string levelScene = "Level Scene";
	string testLevel = "test level";

	public void Start () 
	{
		if (mapSeedEntry != null) 
		{
			mapSeedEntry.text = ( PlayerPrefs.GetInt ("mapSeedEntry") ).ToString();
		}
		if (musicScrollbar != null) 
		{
			musicScrollbar.value = ( PlayerPrefs.GetFloat ("musicValue") );
		}
		if (sfxScrollbar != null) 
		{
			sfxScrollbar.value = ( PlayerPrefs.GetFloat ("sfxValue") );
		}
	}

	// Function for starting the main game and updating the currentLevel variable
	public void StartGame()
	{
		SceneManager.LoadScene ("Level Scene");
		// Enable the temp audio listener since there won't be a listener until the player spawns
		GameManager.instance.tempAudioListener.gameObject.SetActive (true);
		GameManager.instance.currentLevel = levelScene;
		AudioManager.instance.musicPlayer.clip = AudioManager.instance.inGameSong; // Swap the song
		AudioManager.instance.musicPlayer.Play (); // Play the new song
		AudioManager.instance.musicPlayer.loop = true;
		GameManager.instance.AdjustCameras (); // Adjust the cameras on load

	}

	// Function for returning to the options menu and updating the currentLevel variable
	public void OptionsMenu() 
	{
		SceneManager.LoadScene ("Options Menu");
		GameManager.instance.currentLevel = optionsMenu;
	}

	// Function for returning to the start screen and updating the currentLevel variable
	public void ReturnToStartScreen()
	{
		SceneManager.LoadScene ("Start Screen");
		GameManager.instance.currentLevel = startScreen;
		AudioManager.instance.musicPlayer.clip = AudioManager.instance.menuSong; // Swap the song
		AudioManager.instance.musicPlayer.Play (); // Play the new song
		GameManager.instance.playerOneHasSpawned = false;

		if (mapSeedEntry != null) 
		{
			mapSeedEntry.gameObject.SetActive (false);
		}
	}

	// Self explanatory
	public void QuitGame()
	{
		Application.Quit();
	}

	public void TestLevel()
	{
		SceneManager.LoadScene ("test level");
		GameManager.instance.currentLevel = testLevel;
		AudioManager.instance.musicPlayer.clip = AudioManager.instance.inGameSong; // Swap the song
		AudioManager.instance.musicPlayer.Play (); // Play the new song
		GameManager.instance.AdjustCameras ();
	}

	#region Button Sound Functions
	// Function for playing a sound when the player mouses over a button
	public void OnHighlight () 
	{
		AudioManager.instance.soundPlayer.PlayOneShot (AudioManager.instance.buttonHighlight, AudioManager.instance.sfxVolume);
	}

	// Function for playing a sound when the player clicks a button
	public void OnClick()
	{
		AudioManager.instance.soundPlayer.PlayOneShot (AudioManager.instance.buttonPress, AudioManager.instance.sfxVolume);
	}

	public void OnOptionsButtonHighlight ()
	{
		AudioManager.instance.soundPlayer.PlayOneShot (AudioManager.instance.optionsBttnHighlight, AudioManager.instance.sfxVolume);
	}

	public void OnOptionsButtonClick()
	{
		AudioManager.instance.soundPlayer.PlayOneShot (AudioManager.instance.optionsBttnClick, AudioManager.instance.sfxVolume);
	}

	public void OnQuitGameButtonHighlight()
	{
		// Play a random death grunt from the array each time the Quit button is highlighted
		AudioManager.instance.soundPlayer.PlayOneShot (AudioManager.instance.deathGruntsArray[Random.Range(0, AudioManager.instance.deathGruntsArray.Length)], AudioManager.instance.sfxVolume);
	}

	public void OnOnePlayerButtonHighlight()
	{
		AudioManager.instance.soundPlayer.PlayOneShot (AudioManager.instance.onePlayerBttnHighlight, AudioManager.instance.sfxVolume);
	}

	public void OnTwoPlayersButtonHighlight()
	{
		AudioManager.instance.soundPlayer.PlayOneShot (AudioManager.instance.twoPlayersBttnHighlight, AudioManager.instance.sfxVolume);
	}
	#endregion

	// Function for setting the game to one player
	public void OnePlayer () {
		GameManager.instance.numPlayers = 1;
		PlayerPrefs.SetInt ("pp_numPlayers", GameManager.instance.numPlayers);
	}

	// Function for setting the game to two players
	public void TwoPlayers () 
	{
		GameManager.instance.numPlayers = 2;
		PlayerPrefs.SetInt ("pp_numPlayers", GameManager.instance.numPlayers);
	}

	// Function for allowing the scroll bar to manipulate music volume
	public void ScrollBarMusic (float musicValue) 
	{
		AudioManager.instance.musicVolume = musicValue;
		PlayerPrefs.SetFloat ( "musicValue", AudioManager.instance.musicVolume );
	}

	// Function for allowing the scroll bar to manipulate SFX volume
	public void ScrollBarSFX (float sfxValue)
	{
		AudioManager.instance.sfxVolume = sfxValue;
		PlayerPrefs.SetFloat ( "sfxValue", AudioManager.instance.sfxVolume );
	}

	// Function for toggling the map of the day checkbox
	public void MapOfTheDay(bool value)
	{		
		GameManager.instance.mapOfTheDay = value;
	}

	// Function for storing the incoming integer from the MapSeed InputField
	public void MapSeedEntry ()
	{
		GameManager.instance.mapSeed = int.Parse (mapSeedEntry.text);
		PlayerPrefs.SetInt("mapSeedEntry", GameManager.instance.mapSeed); 
	}

	// Function for saving all of the Player Prefs changes
	public void Apply () 
	{
		PlayerPrefs.Save ();
	}
}
