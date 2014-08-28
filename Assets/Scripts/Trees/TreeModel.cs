using UnityEngine;
using System.Collections.Generic;

public class TreeModel : _Mono {

	protected const float scale = 1f;
	protected const float SPRITE_WIDTH = 16f / scale;
	protected const float SPRITE_HEIGHT = 64f / scale;
	public Sprite foilageSprite;
	public Sprite angledSprite;
	public Sprite nonangledSprite;

	public Vector2 relativeRoot; //Relative placement of branches in normalized x and y scale.
	public int seed = 1;
	//public bool stopGrowingAfterBranching = true;
	
	public List<TreeModel> branches{ get; set; }
	public int generation{ get; set; }
	public bool isAngled{ get; set; }
	public float myAngle{ get; set; }
	public float angleCentral{ get; set; }
	public float proportion{ get; set; }
	public int[] branchingOptions{ get; set; }
	public float[] branchingProbability { get; set; }
	public float[] branchingAngles { get; set; }
	public bool doneGrowing{ get; set; }
	public GameObject foilage{ get; set; }
	public Vector2 foilagePosition{ get; set; }
	public float foilageXs{ get; set; }
	public float foilageYs{ get; set; }
	public float myAnglePermanant { get; set; }
	protected bool symmetricGrowth{ get; set; }
	protected bool symmetric{ get; set; }
	protected float _age;
	protected float _maxAge;
	protected float maxAge{ get{ return _maxAge; } set{ _maxAge = value; if (maxAge < age){ _age = maxAge; } } }
	protected float age{ get{ return _age; } set{ _age = value; if (maxAge < age){ _age = maxAge; } } }
	protected bool stopGrowingAfterBranching{ get; set; } 
	protected int maxGenerations{ get; set; }
	protected float height{ get; set; }
	protected float ageSinceLastBranch{ get; set;}
	protected float actualAge{ get; set; }
	protected float heightVariation{ get; set; }
	protected bool fastGrowth{ get; set; }
	protected bool growFoilage{ get; set; }
	protected int foilGen{ get; set; }
	protected float angleSway { get; set; }
	protected float rotationTracker { get; set; }
	protected float totalHeight { get; set; }
	protected bool bloomed = false;
	protected float givenHeight { get; set; }
	public TreeModel root { get; set; }
	public TreeModel parent { get; set; }

	// Use this for initialization
	protected virtual void Awake () {

		fastGrowth = false;

		angleSway = 0f;
		rotationTracker = 0f;

		branches = new List<TreeModel> ();
		generation = 0;
		isAngled = true;
		myAngle = 90f;
		angleCentral = 90f;
		symmetricGrowth = true;
		symmetric = true;
		proportion = 0.125f;
		maxAge = 800f;
		stopGrowingAfterBranching = true;
		maxGenerations = 4;
		height = 0f;
		branchingOptions = new int[]{2, 3};
		branchingProbability = new float[]{0.5f, 0.5f};
		branchingAngles = new float[]{ 27f, 40f};
		actualAge = 0f;
		doneGrowing = false;
		age = 0f;
		foilagePosition = new Vector2 (0.5f, 0.6f);
		growFoilage = true;
		foilGen = 1;
		givenHeight = 500.0f;
		root = this;
		parent = null;
		foilageXs = 1f;
		foilageYs = 1f;

		if (fastGrowth) { age = maxAge;}
		heightVariation = Random.Range (0.6f, 1.2f);

		spriteRenderer.sprite = isAngled ? angledSprite : nonangledSprite;
		Orient ();
		
		Utils.Assert ( branchingOptions.Length == branchingProbability.Length, "Checking branching and probability length.");
		Utils.Assert ( branchingOptions.Length == branchingAngles.Length, "Checking branching and angles length.");
		
		float result = 0f;
		foreach (float p in branchingProbability) {result += p;}
		Utils.Assert (result >= 1f, "checking probability adds to 1");
	}

	public Vector2 absoluteRoot{
		get{return xy;}
		set{xy = value;}
	} //Absolute placement of branches in pixels
	
	// Update is called once per frame
	protected virtual void Update () {

		if(generation == 0){
			givenHeight = Globals.heightManager.height;
			ReportHeight ();

			int loopBreakCounter = 0;

			while( totalHeight < givenHeight && loopBreakCounter < 3){
				StartGrowing ();
				ReportHeight ();
				loopBreakCounter += 1;
			}

			Orient();

			if(growFoilage){
				CreateFoilage ();
				PositionFoilage ();
			}
		}

	}

	//Recusive
	protected virtual void StartGrowing() {
		if (generation != 0) {
			float myAngleTemp = myAnglePermanant + Mathf.Sin (rotationTracker) * angleSway;
			myAngle = myAngleTemp;
		}
			
		rotationTracker += Mathf.PI / 150f;
		
		if (!doneGrowing && (age < maxAge)) {
			age += 1f;
		}
		
		while (actualAge < age && !doneGrowing) {
			Grow();
		}
		
		if (generation == 0) {
			PositionBranches ();
		}
		
		Determineheight ();

		if(generation == 0){
			//Do not use foreach loop here because the tree will be modified.
			for(int i = 0; i < branches.Count; i++ ){
				TreeModel branch = branches[i];
				branch.StartGrowing ();
			}
		}
	}
	
	//Recusive
	protected virtual void PositionFoilage() {
		if (foilage != null) {
			_Mono foilMono = foilage.GetComponent<_Mono> ();
			foilMono.xy = GetRootPosition (foilagePosition);
			foilMono.angle = myAngle - 90;
			foilMono.xs = ys * 2 * foilageXs;
			foilMono.ys = ys * foilageYs;
		}

		if(generation == 0){
			foreach (TreeModel branch in branches) {
				branch.PositionFoilage ();
			}
		}
	}
	
	//Recusive
	protected virtual void CreateFoilage(){

		if (generation >= foilGen && foilage == null) {
			foilage = new GameObject ();
			SpriteRenderer sr = foilage.AddComponent<SpriteRenderer> ();
			foilage.AddComponent<_Mono> ();
			sr.sprite = foilageSprite;
			sr.sortingOrder = 1;
		}
		
		if(generation == 0){
			foreach (TreeModel branch in branches) {
				branch.CreateFoilage ();
			}
		}
	}

	//Grows the tree based on age of the tree
	protected virtual void Grow(){
		if (generation == 0) {
			heightVariation = 1f;
		}
		Random.seed = seed;
		actualAge++;
		ageSinceLastBranch++;
		TryBranch();
	}
	
	//Recusive
	protected virtual void Orient(){
		//Debug.Log ("Orient called.");
		xs = proportion * height / SPRITE_WIDTH;
		//Debug.Log (proportion + "," + height + "," + SPRITE_WIDTH + "," + xs);
		ys = height / SPRITE_HEIGHT;
		angle = myAngle - 90;
		
		if(generation == 0){
			foreach (TreeModel branch in branches) {
				branch.Orient ();
			}
		}
	}

	// Decides whether the tree will branch right now (Based on the age of the tree and the random number generator)
	protected virtual bool WillBranch() {
		float threshold = (generation == 0) ? 50f : 100f - generation * 10;

		if (ageSinceLastBranch > threshold && !doneGrowing) {
			if(generation < maxGenerations && !bloomed){bloomed = true; return true;}
			else if(stopGrowingAfterBranching && generation != 0){doneGrowing = true; return false;}
		}

		return false;
	}

	// Takes care of new branch generation, including setting a unique seed for the child based on this seed.
	protected virtual void TryBranch() {
		if (WillBranch()) {
			ageSinceLastBranch = 0f;

			/*
			if(stopGrowingAfterBranching){
				doneGrowing = true;
			}*/

			int ind = DecideIndexForNumberOfBranches();
			int b = branchingOptions[ind];
			float a = branchingAngles[ind];
			float tempA = angleCentral - (b - 1) * a / 2f; //The -1 accounts for that there are b-1 angle gaps.

			for(int i = 0; i < b; i ++){
				TreeModel t = ((GameObject)Instantiate(this.gameObject, new Vector3(-10000f, -10000f, 0f), Quaternion.identity)).GetComponent<TreeModel>();
				Utils.Assert(t != null, "Check branch creating successful");
				//t.Start();
				t.angleCentral = tempA;
				if(t.angleCentral != 90f)
					t.angleCentral += (90f - t.angleCentral)/Mathf.Abs (90f - t.angleCentral) * Random.Range(-2f, 10f);
				t.generation = generation + 1;
				t.proportion = proportion / 1.3f;
				t.relativeRoot = new Vector2( 0.5f, 0.8f );
				t.seed = Random.Range(0, 10000);
				t.doneGrowing = false;
				t.myAngle = t.angleCentral;
				t.myAnglePermanant = t.myAngle;
				t.parent = this;
				t.root = this.root;

				for( int i2 = 0; i2 < branchingAngles.Length; i2++ ){
					t.branchingAngles[i2] = branchingAngles[i2]/2;
				}

				root.branches.Add (t);
				tempA += a;
			}
		}
	}

	//Decides the number of branches to make
	protected virtual int DecideIndexForNumberOfBranches(){
		if(generation >= 0){
			bool decidedBranchNumbers = false;
			int i = 0 ;
			float accumulater = 0f;
			float decider = Random.Range(0f, 1f);
			int decidedNumbers = 0;
			seed += 1;
			while( i < branchingOptions.Length && !decidedBranchNumbers ){
				accumulater += branchingProbability[i];
				if( accumulater >= decider ){
					decidedBranchNumbers = true;
					decidedNumbers = branchingOptions[i];
					break;
				}
				i++;
			}
			//Debug.Log("Decided to make " + decidedNumbers + " branches.");
			return i;
		}else{
			return 1;
		}
	}

	//Decides whether the branching is symmetric
	protected virtual bool IsBranchSymmetric() {
		return false;
	}
	
	//Updates the branch positions based on relative root, and updates absolute root (recursive call)
	protected virtual void PositionBranches() {
		foreach (TreeModel branch in branches) {
			if(branch.parent != null){
				branch.absoluteRoot = branch.parent.GetRootPosition(branch.relativeRoot);
			}

			//branch.PositionBranches();
			//Debug.Log("Generation " + generation + " position branches called.");
		}
	}

	//Determines and sets the new height of the tree as a function of age and seed
	protected virtual void Determineheight() {
		height = Mathf.Min(actualAge, age) * heightVariation;
	}
	
	//Determines and sets the new proportion of the tree as a function of age and seed
	public virtual void DetermineProportion() {
		//proportion;
	}
	
	//Determines and sets the child of the tree as a function of age and seed
	public virtual void DetermineChildAngles(TreeModel child) {

	}

	protected Vector2 GetRootPosition(Vector2 childRelativeRoot){
		Vector2 pointTemp = Utils.GetPointFromAngleAndRadius (xy, myAngle, height * scale * childRelativeRoot.y);
		pointTemp = Utils.GetPointFromAngleAndRadius (pointTemp, myAngle - 90f, height * scale * proportion / 2);
		return Utils.GetPointFromAngleAndRadius (pointTemp, myAngle + 90f, height * scale * proportion * childRelativeRoot.x); 
		//return new Vector2 (childRelativeRoot.x * height * proportion + absoluteRoot.x - height * proportion / 2, 
		//                    childRelativeRoot.y * height + absoluteRoot.y);
	}

	private void ReportHeight(){

		if (generation == 0) {
			float result = 0f;
			result = maxHeight ();
			totalHeight = result;
//			Debug.Log (totalHeight);
		} else {
			totalHeight = root.totalHeight;
		}

	}

	//Recursive method to calculate height.
	public float maxHeight(){
		float result = height * Mathf.Sin (myAnglePermanant) + y;
		float highest = 0f;

		foreach (TreeModel branch in branches) {
			highest = Mathf.Max(highest, branch.maxHeight());
		}

		return result + highest;
	}
}
