using UnityEngine;
using System.Collections;

public abstract class Hazard : _Mono {

	protected bool hasStarted;
	protected bool isStopped;
	protected bool hasFinished;
	protected bool isHarmful;


	public float fadeOutRate;

	public void FadeOut(){
		Stop ();
		//Destroy ();
	}

	public virtual void Start (){
		hasStarted = true;
		isStopped = false;
		hasFinished = false;
		isHarmful = true;
	}

	public virtual void Finish(){
		hasFinished = true;
	}

	public virtual void Stop(){
		isStopped = true;
	}

    void OnTriggerEnter2D(Collider2D col){
		Debug.LogError("Collision");

		if(col.gameObject.tag == "Tree" || col.gameObject.tag == "Shield"){
			FadeOut();
		}
	}

}
