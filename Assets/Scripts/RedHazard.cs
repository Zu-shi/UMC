using UnityEngine;
using System.Collections;

//Red Hazards do extra damage
public class RedHazard : Hazard {

	public int damage;

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