using UnityEngine;
using System.Collections;

public class TreeGrowth : _Mono {

	public int lives = 3;

	public float height = 3f;

	public float growthRate = .1f;

	public GUIText heightText;

	public bool stopped = false;

	public float speedModifier = 1f;

	public GameObject lifeIndicator;

	private GameObject[] lifeSymbols;

	public Vector2 leftMostLifeIndicatorPos;
	public float lifeSymbolOffset;

	public float invincibleTimer;

	public float invincibleOnHitTime;


	// Use this for initialization
	void Start () {
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

	void ChangeSpeed(float modifier){
		speedModifier = modifier;
	}

	void Restart(){
		stopped = false;
	}

	void Stop(){
		stopped = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!stopped){
			height += growthRate * speedModifier * Time.deltaTime;
		}
		if(invincibleTimer >= 0f){
			invincibleTimer -= Time.deltaTime;
		}
		heightText.text = "Height: " + height;

		UpdateLives ();
	}

	void UpdateLives(){
		int lastLifeIndex = lives - 1;
		for(int i = 0; i < lifeSymbols.Length; i++){
			if(i > lastLifeIndex){
				lifeSymbols[i].gameObject.SetActive(false);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "RedHazard") {
			if(invincibleTimer >= 0f){
					int damage = col.gameObject.GetComponent<RedHazard> ().damage;
					if (damage > lives) {
							lives = 0;
					} 
					else {
							lives -= damage;
					}
			}
		}

		else if(col.gameObject.tag == "BlueHazard"){
			if(lives > 0 && invincibleTimer >= 0f){
				lives--;
			}
			//Logic for freezing goes here
		}
		else if(col.gameObject.tag == "OrangeHazard"){
			if(lives > 0 && invincibleTimer >= 0f){
				lives--;
			}
			//Logic for slowing goes here
		}
	}
}
