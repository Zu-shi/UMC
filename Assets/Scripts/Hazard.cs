using UnityEngine;
using System.Collections;

public abstract class Hazard : _Mono {

	protected bool hasStarted;
	protected bool isStopped;
	protected bool hasFinished;
	protected bool isHarmful;
	protected int damage;

	public abstract void Start ();

	public abstract void Finish();

	public abstract void Stop();


}
