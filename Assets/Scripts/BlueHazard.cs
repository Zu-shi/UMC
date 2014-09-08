using UnityEngine;
using System.Collections;

//Blue hazards pause growth
public class BlueHazard : Hazard {
	
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

    public override void Update(){
        base.Update();
        angle = angle + 2f;
    }

    void OnTriggerEnter(Collider other) {
        base.FadeOut();
    }
}
