using UnityEngine;
using System.Collections;

//Red Hazards do extra damage
public class RedHazard : Hazard {

	public GameObject targetTree;

	public float speed;

	public int damage;

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
		hasFinished = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isStopped && !hasFinished){
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, targetTree.transform.position, step);
		}
	}
}



