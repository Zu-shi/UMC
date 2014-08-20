using UnityEngine;
using System.Collections;

public class TreeGrowth : _Mono {

	public int lives = 3;

	public float height = 3f;

	public float growthRate = .1f;

	public GUIText heightText;

	public bool stopped = false;

	public float speedModifier = 1f;

	// Use this for initialization
	void Start () {
	}

	void ChangeSpeed(float modifier){
		speedModifier = modifier;
	}

	void Restart(){
		stopped = false;
	}

	void Stop(){
		stopped = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!stopped){
			height += growthRate * speedModifier * Time.deltaTime;
		}
		heightText.text = "Height: " + height;
	}
}
