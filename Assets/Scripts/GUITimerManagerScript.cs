﻿using UnityEngine;
using System.Collections;

public class GUITimerManagerScript : MonoBehaviour {

	public int totalSeconds;
	public int secondsToFlash;
	public float flashesPerSecond;
	public Color nonFlashingColor = Color.white;
	public Color flashingColor = Color.red;
	private float flashesTracker; //Keeps track of amount of time passed since last flash.
	private bool flashing = false;
	private GUIText guiText;

	// Use this for initialization
	void Start () {
		guiText = gameObject.GetComponent<GUIText> ();
	}

	public void SetTotalSeconds(int s){
		totalSeconds = s;
	}

	// Update is called once per frame
	void Update () {

		//Calculate string to show.
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;
		string result = "Time Left: ";
		if(minutes >= 10){
			result = result + minutes.ToString();
		}else{
			result = result + "0" + minutes.ToString();
		}
		result += ":";
		if(seconds >= 10){
			result = result + seconds.ToString();
		}else{
			result = result + "0" + seconds.ToString();
		}
		guiText.text = result;

		//Flashes
		flashesTracker += Time.deltaTime;
		// /2 accounts for the fact that two flashing switches counts as one flash.
		if (flashesTracker >= (1.0f / flashesPerSecond / 2)) {
			flashing = !flashing;
			flashesTracker = 0.0f;
		}
		if (flashing & (totalSeconds <= secondsToFlash)) {
			guiText.color = flashingColor;
		}else{
			guiText.color = nonFlashingColor;
		}
	}
}