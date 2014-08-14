using UnityEngine;
using System.Collections;

public class TreeGrowth : _Mono {

	public float height = 3f;

	public float growthRate = .1f;

	public GUIText heightText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		height += growthRate * Time.deltaTime;

		heightText.text = "Height: " + height;
	}
}
