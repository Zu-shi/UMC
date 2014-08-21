using UnityEngine;
using System.Collections;

public abstract class Hazard : _Mono {

	protected bool hasStarted;
	protected bool isStopped;
	protected bool hasFinished;
	protected bool isHarmful;
	protected int damage;

	public void Start (){
		hasStarted = true;
		isStopped = false;
		hasFinished = false;
		isHarmful = true;
	}

	public void Finish(){
		hasFinished = true;
	}

	public void Stop(){
		isStopped = true;
	}


}
