using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public AudioSource audioECG, audioVentilator;
	public float heartRate, respiRate;
	private float elapsedTimeECG, elapsedTimeVentilator;

	// Use this for initialization
	void Start () {
		elapsedTimeECG = 0.0f;
		elapsedTimeVentilator = 0.0f;
		audioECG.Play ();
		audioVentilator.Play ();
	}

	// Update is called once per frame
	void Update () {
		elapsedTimeECG += Time.deltaTime; 
		elapsedTimeVentilator += Time.deltaTime;
		if (elapsedTimeECG >= 60f / heartRate) {
			elapsedTimeECG -= 60f / heartRate;
			audioECG.Play ();
		}
		if (elapsedTimeVentilator >= 60f / respiRate) {
			elapsedTimeVentilator -= 60f / respiRate;
			if (audioVentilator.isPlaying) {
				audioVentilator.Stop ();
			}
			audioVentilator.Play ();
		}
	}
}
