using UnityEngine;
using System.Collections;

//Orange Hazards slow growth
public class OrangeHazard : Hazard {

	public float slowFactor;

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
	
}
