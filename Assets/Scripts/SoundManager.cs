using UnityEngine;
using System.Collections;

public class SoundManager:MonoBehaviour{

	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	public float lowPitch = .95f;
	public float highPitch = 1.05f;

	void Awake(){
		if(instance == null){
			instance = this;
		}else if(instance != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
	
	public void PlaySingle(AudioClip clip){
		efxSource.clip = clip;
		efxSource.Play();
	}

	public void RandomizeSfx(params AudioClip[] clips){
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitch, highPitch);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips[randomIndex];
		efxSource.Play();
	}
}