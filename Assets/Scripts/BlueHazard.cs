using UnityEngine;
using System.Collections;


//Blue hazards pause growth
public class BlueHazard : Hazard {
	
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
            Vector3 cameraPos = Camera.main.transform.position;
			transform.position = Vector3.MoveTowards(transform.position, 
                 cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f) , step);
		}

	}

    /*
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger entered");
    }*/
}
