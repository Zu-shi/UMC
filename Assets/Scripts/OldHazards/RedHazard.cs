using UnityEngine;
using System.Collections;

//Red Hazards do extra damage
public class RedHazard : Hazard {

	public float damage;

	public float radius;
	public float rotationSpeed;

	private Vector3 center;


	// Use this for initialization
	public override void  Start () {
		base.Start ();
		center = transform.position;
	}

	public override void Update(){
		if(!isStopped && !hasFinished){
			float step = speed * Time.deltaTime * Camera.main.orthographicSize;
			Vector3 cameraPos = Camera.main.transform.position;
			center = Vector3.MoveTowards(center, 
			                                         cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f) , step);
			z = 0f;
		}else{
			float step = -speed * Time.deltaTime * Camera.main.orthographicSize / 6;
			Vector3 cameraPos = Camera.main.transform.position;
			center = Vector3.MoveTowards(center, 
			                                         cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f) , step);
			z = 0f;
		}
		
		if( (Globals.stateManager.inCutscene || Globals.stateManager.isGameOver) && !isFading){
			//FadeOut();
		}
		float x = center.x + Mathf.Sin (Time.time * rotationSpeed) * radius;
		float y = center.y + Mathf.Cos (Time.time * rotationSpeed) * radius;
		transform.position = new Vector3 (x, y, transform.position.z);
		//Debug.Log ("x: " + (x - center.x) + " y: " + (y - center.y));

	}
}