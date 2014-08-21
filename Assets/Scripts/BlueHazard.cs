using UnityEngine;
using System.Collections;


//Blue hazards pause growth
public class BlueHazard : Hazard {

	public GameObject targetTree;
	
	public float speed;
	
	public float freezeTime;
	
	// Use this for initialization
	public override void  Start () {
		base.Start ();
	}
	
	public override void Stop(){
		base.Stop ();
	}
	
	public override void Finish(){
		base.Finish ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!isStopped && !hasFinished){
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, targetTree.transform.position, step);
		}
	}
}
