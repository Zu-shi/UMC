using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HealingPowerupScript : _Mono {

    protected bool isStopped;
    protected bool hasFinished;
    protected bool isFading;

    [Tooltip("Speed in terms of half-cameras per second")]
    public float speed = 0.3f;
    
    [Tooltip("Time it takes to reach the player at the starting point")]
    private float time = 20f;
    
    [Tooltip("Starting angle from vertically up.")]
    public float[] startingAngle;
    
    private float fadeOutTime = 3f;
    private float startSize;
    private float startSpeed;
   //private float startRate;

	// Use this for initialization
    void Start () {
        ParticleSystem ps = transform.GetChild(0).particleSystem;
        startSize = ps.startSize;
        //startRate = ps.emissionRate;
        startSpeed = ps.startSpeed;

        float scale = Globals.cameraManager.cameraRatio;
        xys = scale * xys;
        //gameObject.particleSystem.emissionRate = gameObject.particleSystem.emissionRate * scale;
        //gameObject.particleSystem.duration = gameObject.particleSystem.duration * scale;

        isStopped = false;
        hasFinished = false;
        isFading = false;
        
        float angle = Utils.RandomFromArray<float>(startingAngle);
        
        Vector3 cameraPos = Camera.main.transform.position;
        y = cameraPos.y - Camera.main.orthographicSize / 4;
        x = Globals.treeManager.treePos.x;
        
        x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + angle)) * speed * Camera.main.orthographicSize * time);
        y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + angle)) * speed * Camera.main.orthographicSize * time);
	}
	
	// Update is called once per frame
    void Update () {
        if(!isStopped && !hasFinished){
            float step = speed * Time.deltaTime * Camera.main.orthographicSize;
            Vector3 cameraPos = Camera.main.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     cameraPos - new Vector3(0f, Camera.main.orthographicSize / 2, 0f) , step);
            z = 0f;
        }else{
            float step = -speed * 2 * Time.deltaTime * Camera.main.orthographicSize / 6;
            Vector3 cameraPos = Camera.main.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     cameraPos - new Vector3(0f, Camera.main.orthographicSize / 2, 0f) , step);
            z = 0f;
        }
        
        ParticleSystem ps = transform.GetChild(0).particleSystem;
        ps.startSpeed = startSpeed * Globals.cameraManager.cameraRatio;
        ps.startSize = startSize * Globals.cameraManager.cameraRatio;
        //ps.emissionRate = startRate * Globals.cameraManager.cameraRatio;
	}

    public void FadeOut(){
        if (!isFading)
        {
            ParticleSystem ps = transform.GetChild(0).particleSystem;
            isFading = true;
            Stop();
            Sequence sq = DOTween.Sequence();
            sq.Append(DOTween.To(() => ps.startLifetime, x => ps.startLifetime = x, 0f, fadeOutTime/2));
            sq.AppendInterval(fadeOutTime);
            sq.AppendCallback( Kill );
            sq.onComplete = Kill;

            ps.enableEmission = false;

            Sequence sq2 = DOTween.Sequence();
            sq2.Append(DOTween.To(() => ps.emissionRate, x => ps.emissionRate = x, 0f, fadeOutTime/8));
        }
    }

    public virtual void Kill(){
        Destroy(this.gameObject);
    }
    
    public virtual void Finish(){
        hasFinished = true;
    }
    
    public virtual void Stop(){
        isStopped = true;
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Tree" || other.gameObject.tag == "Shield")
        {
            //transform.GetChild(0).particleSystem.emissionRate = 0;
            //transform.GetChild(0).particleSystem.enableEmission = false;
            //transform.GetChild(0).particleSystem.
            //transform.GetChild(0).particleEmitter.maxEmission = 10;
            FadeOut();
        }
    }
}
