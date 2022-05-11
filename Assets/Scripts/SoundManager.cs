using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource efxSource;                 //effect music
	public AudioSource musicSource;               //background music
	public static SoundManager instance = null;   //a singleton pattern

	public float lowPitchRange = 0.95f;           
	public float highPitchRange = 1.05f;          

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	
	}

	public void PlaySingle(AudioClip clip){
		efxSource.clip = clip;
		efxSource.Play ();
	}

	//the params parameter allows us to add multiple source separated by a comma
	public void RandomizeSfx(params AudioClip[] clips){
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips[randomIndex];
		efxSource.Play ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
