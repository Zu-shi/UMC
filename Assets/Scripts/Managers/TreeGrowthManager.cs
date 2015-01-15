using UnityEngine;
using System.Collections;

public class TreeGrowthManager : _Mono {

    public float lives {
        get{return Globals.stateManager.lives;} 
        set{Globals.stateManager.lives = value;}
    }

    public GameObject healRing;
    public GameObject healLeaf;
    
    public Vector2 leftMostLifeIndicatorPos;
    public Vector2 initialSize;
    public float lifeSymbolOffset;
    
    public float invincibleTimer;
    
    public float invincibleOnHitTime;
    
    public float clearAllTimer;
    public float clearAllTime {get; set;}

	private GameObject[] lifeSymbols;
    private BoxCollider col;

	// Use this for initialization
	void Start () {
        clearAllTime = 0.3f;
        col = GetComponent<BoxCollider>();
        Utils.Assert(col != null);
	}
	
	// Update is called once per frame
	void Update () {
		if(invincibleTimer > 0f){
			invincibleTimer -= Time.deltaTime;
		}

        if(clearAllTimer > 0f){
            clearAllTimer -= Time.deltaTime;
        }

		UpdateLives ();
        UpdateCollisionBox();

	}

    void UpdateCollisionBox(){
        float csize = Globals.target.y;
        col.center =  new Vector3(Globals.treeManager.treePos.x,  Globals.target.y / 2, 0f);
        col.size = new Vector3(csize / 6, Globals.target.y, 1000f);

        //Old approach is based upon tree height, which may not be suitable for game style where the camera moves
        /*
        float csize = Globals.treeManager.mainTree.totalHeight;
        col.center =  new Vector3(Globals.treeManager.treePos.x,  csize / 2, 0f);
        col.size = new Vector3(csize / 6, csize, 1000f);
        */
    }

	void UpdateLives(){
        if(lives<0){lives = 0f;}
	}

    public void ClaimReward() {
        //Debug.LogError("HEALPOWERUP");
        Instantiate(healRing, Globals.target, Quaternion.identity);
        
        lives = Mathf.Clamp(lives + 0.1f, 0f, 1f);
        GameObject leafFlash;
        leafFlash = Instantiate(healLeaf, Globals.stateManager.leafLifeIndicator.gameObject.transform.position, 
                                Quaternion.identity) as GameObject;
        leafFlash.GetComponent<_Mono>().xys = Globals.stateManager.leafLifeIndicator.xys * 1.1f;
        Globals.treeManager.GrowthSpurt();
        clearAllTimer = clearAllTime;
    }

	void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "StreamBugHazard") {
            StreamBugHazardScript sbh = col.gameObject.GetComponent<StreamBugHazardScript> ();
            if(sbh.isHarmful)
            {
                float damage = sbh.damage;
                //damage = 0;
                if(invincibleTimer <= 0f){
                    if (damage > lives) {
                        lives = 0;
                    } 
                    else {
                        lives -= damage;
                        invincibleTimer = invincibleOnHitTime;
                    }

                    Globals.treeManager.mainTree.GlowRed();
                    //Debug.Log("glowing red called");
                    //Globals.treeManager.mainTree.startShake();
                }
                sbh.isHarmful = false;
            }
        }
	}
}
