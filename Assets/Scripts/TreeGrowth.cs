using UnityEngine;
using System.Collections;

public class TreeGrowth : _Mono {

    public float lives {
        get{return Globals.stateManager.lives;} 
        set{Globals.stateManager.lives = value;}
    }

    public GameObject healRing;
    public GameObject growRing;
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
        clearAllTime = 1f;
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
        float csize = Globals.treeManager.mainTree.totalHeight;
        col.center =  new Vector3(Globals.treeManager.treePos.x,  csize / 2, 0f);
        col.size = new Vector3(csize / 6, csize, 1000f);
    }

	void UpdateLives(){
        if(lives<0){lives = 0f;}
	}


	void OnTriggerEnter(Collider col){
        //Debug.Log("Trigger");
        
        if (col.gameObject.tag == "StreamBugHazard") {
            StreamBugHazard sbh = col.gameObject.GetComponent<StreamBugHazard> ();
            if(sbh.harmful)
            {
                float damage = sbh.damage;
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
                sbh.harmful = false;
            }
        }

		else if (col.gameObject.tag == "RedHazard") {
			float damage = col.gameObject.GetComponent<RedHazard> ().damage;
			if(invincibleTimer <= 0f){
				if (damage > lives) {
					lives = 0;
				} 
				else {
					lives -= damage;
					invincibleTimer = invincibleOnHitTime;
				}

                //Globals.treeManager.mainTree.startShake();
			}
		}

		else if(col.gameObject.tag == "BlueHazard"){
			if(lives > 0 && invincibleTimer <= 0f){
				lives -= 0.3f;
				invincibleTimer = invincibleOnHitTime;
                
                //Globals.treeManager.mainTree.startShake();
			}
			//Logic for freezing goes here
		}

		else if(col.gameObject.tag == "OrangeHazard"){
            if(lives > 0 && invincibleTimer <= 0f){
                lives -= 0.3f;
				invincibleTimer = invincibleOnHitTime;
                
                //Globals.treeManager.mainTree.startShake();
			}
			//Logic for slowing goes here
		}

        if(col.gameObject.tag == "HealPowerup"){
            //Debug.LogError("HEALPOWERUP");
            Instantiate(healRing, col.transform.position - new Vector3(0f, 0f, 1f), Quaternion.identity);

            lives = Mathf.Clamp(lives + 0.1f, 0f, 1f);
            GameObject leafFlash;
            leafFlash = Instantiate(healLeaf, Globals.stateManager.leafLifeIndicator.gameObject.transform.position, 
                                    Quaternion.identity) as GameObject;
            leafFlash.GetComponent<_Mono>().xys = Globals.stateManager.leafLifeIndicator.xys;
            clearAllTimer = clearAllTime;
        }

        
        if(col.gameObject.tag == "GrowPowerup"){
            Instantiate(growRing, col.transform.position, Quaternion.identity);
            clearAllTimer = clearAllTime;
        }
	}
}
