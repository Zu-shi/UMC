using UnityEngine;
using System.Collections;

public class CloudScript : _Mono {

    private float xMoveSpeed;
	
    // Use this for initialization
	void Start () {
        alpha = Random.Range(0.6f, 0.8f);   
        xMoveSpeed = Random.Range(-0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
        x += xMoveSpeed;
	}
}
