using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class Hazard : _Mono {

	protected bool hasStarted;
	protected bool isStopped;
	protected bool hasFinished;
    protected bool isHarmful;

    public float speed = 250f;
	public float fadeOutTime = 1f;

    public virtual void Update(){
        if(!isStopped && !hasFinished){
            float step = speed * Time.deltaTime;
            Vector3 cameraPos = Camera.main.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f) , step);
            z = 0f;
        }
    }

	public void FadeOut(){
		Stop ();

        Sequence sq = DOTween.Sequence();
        sq.Append( DOTween.To(()=> alpha, x=> alpha = x, 0f, fadeOutTime) );
        sq.AppendCallback( Destroy );
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
