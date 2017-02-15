using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioSource musicPlayer; // AudioSource object for storing and swapping songs
	public AudioSource soundPlayer; // AudioSource object for storing and playing SFX

	// Music clips
	public AudioClip menuSong;
	public AudioClip inGameSong;
	public AudioClip winSong;
	public AudioClip gameOverClip;

	// SFX clips
	public AudioClip tankFire;
	public AudioClip tankDeath;
	public AudioClip shellHit;
	public AudioClip buttonPress;
	public AudioClip buttonHighlight;
	public AudioClip optionsBttnHighlight;
	public AudioClip optionsBttnClick;
	public AudioClip[] deathGruntsArray;
	public AudioClip onePlayerBttnHighlight;
	public AudioClip twoPlayersBttnHighlight;

	// Variables for storing volume
	public float sfxVolume;
	public float musicVolume;

	void Awake () 
	{
		if ( instance == null ) 
		{
			instance = this;

			// Don't destroy this AudioManager!
			DontDestroyOnLoad ( gameObject );
		} else {
			Destroy ( gameObject );
		}

		musicVolume = PlayerPrefs.GetFloat ("musicValue");
		sfxVolume = PlayerPrefs.GetFloat ("sfxValue");
	}

	// Use this for initialization
	void Start () {
		musicPlayer = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		musicPlayer.volume = musicVolume;
	}
}
