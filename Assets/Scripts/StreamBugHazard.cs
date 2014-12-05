using UnityEngine;
using System.Collections;

//Red Hazards do extra damage
public class StreamBugHazard : Hazard {
    
    public float damage;
    public bool harmful = true;
    private Vector3 center;


    // Use this for initialization
    public override void  Start () {
        hasStarted = true;
        isStopped = false;
        hasFinished = false;
        isHarmful = true;
        center = transform.position;
    }

    /*
    public override void Update(){
        if(!isStopped && !hasFinished){
            float step = speed * Time.deltaTime * Camera.main.orthographicSize;
            //Debug.Log(step);
            Vector3 cameraPos = Camera.main.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f) , step);
            z = 0f;
        }else{
            float step = -speed * Time.deltaTime * Camera.main.orthographicSize / 6;
            Vector3 cameraPos = Camera.main.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f) , step);
            z = 0f;
        }
        
        if( (Globals.stateManager.inCutscene || Globals.stateManager.isGameOver) && !isFading){
            FadeOut();
        }
        
    }
    
    public override void Stop(){
        base.Stop ();
    }
    
    public override void Finish(){
        base.Finish ();
    }
    */

}