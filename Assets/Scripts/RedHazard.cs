using UnityEngine;
using System.Collections;

public class RedHazard : Hazard {

	public GameObject targetTree;

	public float speed;

	// Use this for initialization
	public override void Start () {
		hasStarted = true;
		isStopped = false;
		hasFinished = false;
		isHarmful = true;
	}

	public override void Stop(){
		isStopped = true;
	}

	public override void Finish(){
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!isStopped){
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, targetTree.transform.position, step);
		}
	}
}



