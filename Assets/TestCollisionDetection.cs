using UnityEngine;
using System.Collections;

public class TestCollisionDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("Script is working");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		print ("detected collision");
	}
}
