using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class Hazard : _Mono {

	protected bool hasStarted;
	protected bool isStopped;
	protected bool hasFinished;
    protected bool isFading;
    protected bool isBlockableByOuterShield = true;
    protected bool isBlockableByInnerShield = true;
    
    public bool isHarmful;
    public int streamMember;
    public int streamTotal;
    public Globals.HazardColors color;

    [Tooltip("Speed in terms of half-cameras per second")]
    public float speed = 1f;
    
    [Tooltip("Time it takes to reach the player at the starting point")]
    public float time = 3f;

    [Tooltip("Starting angle from vertically up.")]
    public float[] startingAngle;

	private float fadeOutTime = 0.5f;
    private float originalXs, originalYs;

    public virtual void Start(){
        originalXs = xs;
        originalYs = ys;
    }

    public virtual void Update(){        

        float sizeRatio = Utils.halfScreenHeight / Globals.INITIAL_HEIGHT;
        xs = sizeRatio * originalXs;
        ys = sizeRatio * originalYs;

        if(!isStopped && !hasFinished){
            float step = speed * Time.deltaTime * Camera.main.orthographicSize;
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

        //Debug.Log(Globals.treeGrowthManager.clearAllTimer);
        if( (Globals.stateManager.inCutscene || Globals.stateManager.isGameOver) && !isFading){
            FadeOut();
        }

        if (Globals.treeGrowthManager.clearAllTimer > 0f && !isFading){
            if(x < Utils.cameraPos.x + Utils.halfScreenWidth && 
               x > Utils.cameraPos.x - Utils.halfScreenWidth &&
               y < Utils.cameraPos.y + Utils.halfScreenHeight && 
               y > Utils.cameraPos.y - Utils.halfScreenHeight)
                FadeOut();
        }
    }

	public void FadeOut(){
        //Debug.Log("Call to fadeout");
        if (!isFading)
        {
            isFading = true;
            Stop();

            Sequence sq = DOTween.Sequence();
            sq.Append(DOTween.To(() => alpha, x => alpha = x, 0f, fadeOutTime));
            sq.AppendCallback( Kill );
            sq.onComplete = Kill;
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
    
    public virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Shield")
        {
            if(!isFading){
                if(Globals.shieldManager.outerShield && !isBlockableByOuterShield){
                }else if(Globals.shieldManager.innerShield && !isBlockableByInnerShield){
                }else{
                    Globals.comboManager.ProcessCombo(streamMember, streamTotal, color, other.gameObject.GetComponent<CenterLeafScript>());
                    FadeOut();
                }
            }

        }

        if (other.gameObject.tag == "Tree"){
            FadeOut();
        }
    }
}
