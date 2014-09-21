using UnityEngine;
using System.Collections;

public class TreeGrowth : _Mono {

    public int lives {
        get{return Globals.stateManager.lives;} 
        set{Globals.stateManager.lives = value;}
    }

	public GameObject lifeIndicator;
    
    public Vector2 leftMostLifeIndicatorPos;
    public float lifeSymbolOffset;
    
    public float invincibleTimer;
    
    public float invincibleOnHitTime;

	private GameObject[] lifeSymbols;
    private BoxCollider col;

	// Use this for initialization
	void Start () {
        col = GetComponent<BoxCollider>();
        Utils.Assert(col != null);
		DisplayLives ();
	}

	void DisplayLives(){
		lifeSymbols = new GameObject[lives];
		for(int i = 0; i < lifeSymbols.Length; i++){
			Vector2 pos = leftMostLifeIndicatorPos;
			pos.x += i * lifeSymbolOffset;
			lifeSymbols[i] = Instantiate(lifeIndicator, pos, Quaternion.identity ) as GameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(invincibleTimer > 0f){
			invincibleTimer -= Time.deltaTime;
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
		int lastLifeIndex = lives - 1;
		for(int i = 0; i < lifeSymbols.Length; i++){
			if(i > lastLifeIndex){
				lifeSymbols[i].gameObject.SetActive(false);
			}
		}
	}


	void OnTriggerEnter(Collider col){
        Debug.Log("Trigger");

		if (col.gameObject.tag == "RedHazard") {
			int damage = col.gameObject.GetComponent<RedHazard> ().damage;
			if(invincibleTimer <= 0f){
				if (damage > lives) {
					lives = 0;
				} 
				else {
					lives -= damage;
					invincibleTimer = invincibleOnHitTime;
				}

                Globals.treeManager.mainTree.startShake();
			}
		}

		else if(col.gameObject.tag == "BlueHazard"){
			if(lives > 0 && invincibleTimer <= 0f){
				lives--;
				invincibleTimer = invincibleOnHitTime;
                
                Globals.treeManager.mainTree.startShake();
			}
			//Logic for freezing goes here
		}

		else if(col.gameObject.tag == "OrangeHazard"){
			if(lives > 0 && invincibleTimer <= 0f){
				lives--;
				invincibleTimer = invincibleOnHitTime;
                
                Globals.treeManager.mainTree.startShake();
			}
			//Logic for slowing goes here
		}
	}
}
