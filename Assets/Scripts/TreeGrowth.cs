using UnityEngine;
using System.Collections;

public class TreeGrowth : _Mono {

	public float height;
	public float startingHeight = 3f;

	public float growthRate = .1f;

	// Use this for initialization
	void Start () {
		height = startingHeight;
	}
	
	// Update is called once per frame
	void Update () {
		height += growthRate * Time.deltaTime;
	}
}
