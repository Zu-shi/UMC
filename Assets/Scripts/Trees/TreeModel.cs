using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class TreeModel : _Mono {

	protected const float scale = 0.5f;
	protected const float SPRITE_WIDTH = 16f / scale;
	protected const float SPRITE_HEIGHT = 64f / scale;
	public Sprite foilageSprite;
    public Sprite angledSprite;
    public Sprite foilageSpriteRed;
    public Sprite angledSpriteRed;
	public Sprite nonangledSprite;
    public FruitScript fruitPrefab;
    public GameObject blankGameObject;

	public Vector2 relativeRoot; //Relative placement of branches in normalized x and y scale.
	public int seed = 1;
	//public bool stopGrowingAfterBranching = true;
    
    //Root only
    public List<Globals.HazardColors> foilageColorPool { get; set; }
    public Sprite foilageRed;
    public Sprite foilageYellow;
    public Sprite foilageBlue;
    public Sprite foilagePurple;
    public Sprite foilageWhite;

	public List<TreeModel> branches{ get; set; }
	public List<TreeModel> allBranches{ get; set; }
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
	public TreeModel root { get; set; }
	public TreeModel parent { get; set; }
	public int highestGrowingSpeed { get; set; }
    public float targetAge;
    public float age;
    public float totalHeight;
    public float height;
    public int foilAccessCounter = 0;
    public List<FruitScript> fruits;
    public int numFruits { get; set; }

    private bool growFoilage{ get; set; }
	protected bool symmetricGrowth{ get; set; }
	protected bool symmetric{ get; set; }
	protected float _age;
	protected float _targetAge;
	protected bool stopGrowingAfterBranching{ get; set; } 
	protected int maxGenerations{ get; set; }
	protected float ageSinceLastBranch{ get; set;}
	protected float actualAge{ get; set; }
	protected float heightVariation{ get; set; }
	protected bool fastGrowth{ get; set; }
	protected int foilGen{ get; set; }
	protected float angleSway { get; set; }
	public float rotationTracker { get; set; }
	protected bool bloomed = false;
	protected float givenHeight { get; set; }

	// Use this for initialization
	protected virtual void Awake () {
        foilageColorPool = new List<Globals.HazardColors>();
		growFoilage = true;
		fastGrowth = false;

		angleSway = 10f;

		branches = new List<TreeModel> ();
		allBranches = new List<TreeModel> ();
        fruits = new List<FruitScript> ();
		generation = 0;
		isAngled = true;
		myAngle = 90f;
		angleCentral = 90f;
        myAnglePermanant = 90f;
		symmetricGrowth = true;
		symmetric = true;
		proportion = 0.125f;
		targetAge = 0f;
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
		foilGen = 1;
		givenHeight = 250.0f;
		root = this;
		parent = null;
		foilageXs = 1f;
		foilageYs = 1f;
		highestGrowingSpeed = 30;

        if (fastGrowth) { age = targetAge;}
        heightVariation = Random.Range (0.6f, 0.8f);

		spriteRenderer.sprite = isAngled ? angledSprite : nonangledSprite;
		Orient ();
		
		Utils.Assert ( branchingOptions.Length == branchingProbability.Length, "Checking branching and probability length.");
		Utils.Assert ( branchingOptions.Length == branchingAngles.Length, "Checking branching and angles length.");
		
		float result = 0f;
		foreach (float p in branchingProbability) {result += p;}
		Utils.Assert (result >= 1f, "checking probability adds to 1");
	}

    public void AddToColorPool(Globals.HazardColors color){
        foilageColorPool.Add(color);
        foilageColorPool.Add(color);
        foilageColorPool.Add(color);
    }

	public Vector2 absoluteRoot{
		get{return xy;}
		set{xy = value;}
	} //Absolute placement of branches in pixels
	
	// Update is called once per frame
    protected virtual void Update () {

		if (generation == 0)
        {
            heightVariation = 1f;
            //givenHeight = Globals.heightManager.height;
            ReportHeight();

            if (Globals.fixedHeightMode)
            {
                int loopBreakCounter = 0;
                while (totalHeight < givenHeight && loopBreakCounter < highestGrowingSpeed)
                {
                    StartGrowing();
                    ReportHeight();
                    loopBreakCounter += 1;
                }
            } else
            {
                int loopBreakCounter = 0;
                while (age < targetAge && loopBreakCounter < highestGrowingSpeed)
                {
                    StartGrowing();
                    loopBreakCounter += 1;
                }
            }

            Orient();
            PositionFruits();

            if (growFoilage)
            {
                CreateFoilage();
                PositionFoilage();
            }
        } else {
            rotationTracker += Mathf.PI / 150f;
            float myAngleTemp = myAnglePermanant + Mathf.Sin (rotationTracker) * angleSway;
            myAngle = myAngleTemp;
        }
        
        PositionBranches ();
	}

	//Recusive
    //Only called for generation = 0;
	protected virtual void StartGrowing() {
		//Debug.Log ("Age = " + age);
		//Debug.Log ("targetAge = " + age);

        /*
		if (!doneGrowing && (age < targetAge)) {
			age += 1f;
		}*/

        while ((age < targetAge) && !doneGrowing) {
			Grow();
		}
	}
	
	//Recusive
	protected virtual void PositionFoilage() {
        if(generation == 0){
            foilAccessCounter = 0;
        }

		if (foilage != null) {
			_Mono foilMono = foilage.GetComponent<_Mono> ();
			foilMono.xy = GetRootPosition (foilagePosition);
			foilMono.angle = myAngle - 90;
            foilMono.xs = ys * 2 * foilageXs;
            foilMono.ys = ys * foilageYs;

            //THIS NEEDS ONE MORE CHECK
            Globals.HazardColors color = Globals.HazardColors.NONE;
            if(root.foilageColorPool.Count > 0){
                if(foilMono.spriteRenderer.sprite == foilageSprite){
                    Sprite spr = foilageSprite;
                    color = root.foilageColorPool[0];
                    switch (color){
                        case(Globals.HazardColors.RED):{spr = foilageRed; break;}
                        case(Globals.HazardColors.BLUE):{spr = foilageBlue; break;}
                        case(Globals.HazardColors.YELLOW):{spr = foilageYellow; break;}
                        case(Globals.HazardColors.PURPLE1):{spr = foilagePurple; break;}
                        case(Globals.HazardColors.WHITE):{spr = foilageWhite; break;}
                    }
                    foilMono.spriteRenderer.sprite = spr;
                    root.foilageColorPool.RemoveAt(0);
                }
            }
		}

		foreach (TreeModel branch in branches) {
			branch.PositionFoilage ();
		}
	}
	
	//Recusive
	protected virtual void CreateFoilage(){

		if (generation >= foilGen && foilage == null) {

			foilage = new GameObject ();
			SpriteRenderer sr = foilage.AddComponent<SpriteRenderer> ();
            sr.sortingLayerName = "Foilage";
			_Mono foilMono = foilage.AddComponent<_Mono> ();
			sr.sortingOrder = 1;
            foilMono.spriteRenderer.sprite = foilageSprite;
             
		}

		foreach (TreeModel branch in branches) {
			branch.CreateFoilage ();
		}
	}

	//Grows the tree based on age of the tree
	protected virtual void Grow(){
		if (generation == 0) {
            heightVariation = 1f;
		}
        Random.seed = seed;
        Determineheight ();

		age++;
		ageSinceLastBranch++;
		TryBranch();

        foreach( TreeModel branch in branches ){
            branch.Grow ();
        }
	}
	
	//Recusive
	protected virtual void Orient(){

        //Debug.Log ("Orient called. Height = " + height);

		//Debug.Log ("Orient called.");
        xs = proportion * height / SPRITE_WIDTH;
        ys = height / SPRITE_HEIGHT;
		//Debug.Log (proportion + "," + height + "," + SPRITE_WIDTH + "," + xs);
		angle = myAngle - 90;

		foreach (TreeModel branch in branches) {
			branch.Orient ();
		}
	}

	// Decides whether the tree will branch right now (Based on the age of the tree and the random number generator)
	protected virtual bool WillBranch() {
		//float threshold = (generation == 0) ? 100f : 100f - generation * 10;
        float threshold = (generation == 0) ? 45f : 100f - generation * 10;
        //Debug.Log("age " + age);
        //Debug.Log("ageSinceLastBranch" + ageSinceLastBranch);

		if (ageSinceLastBranch > threshold && !doneGrowing) {
			if(generation < maxGenerations && !bloomed){bloomed = true; return true;}
			else if(stopGrowingAfterBranching && generation!=0){doneGrowing = true; return false;}
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
                t.rotationTracker = Random.Range(0f, 180f);
				t.generation = generation + 1;
				t.proportion = proportion / 1.3f;
				t.relativeRoot = new Vector2( 0.5f, 0.8f );
				t.seed = Random.Range(0, 10000);
				t.doneGrowing = false;
				t.myAngle = t.angleCentral;
				t.myAnglePermanant = t.myAngle;
				t.parent = this;
				t.root = this.root;
				t.foilageXs = foilageXs;
				t.foilageYs = foilageYs;

				for( int i2 = 0; i2 < branchingAngles.Length; i2++ ){
					t.branchingAngles[i2] = branchingAngles[i2]/2;
				}

				branches.Add (t);
				root.allBranches.Add (t);
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
			seed += 1;
			while( i < branchingOptions.Length && !decidedBranchNumbers ){
				accumulater += branchingProbability[i];
				if( accumulater >= decider ){
					decidedBranchNumbers = true;
					//decidedNumbers = branchingOptions[i];
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
			branch.absoluteRoot = branch.parent.GetRootPosition(branch.relativeRoot);

			branch.PositionBranches();
            //Debug.Log("Generation " + generation + " position branches called. Relative root y " + branch.relativeRoot.y);
            //Debug.Log("Generation " + generation + " position branches called. Absolute root y " + branch.absoluteRoot.y);
            //Debug.Log("Generation " + generation + " position branches called. y " + branch.y);
		}
	}

	//Determines and sets the new height of the tree as a function of age and seed
	protected virtual void Determineheight() {
        //Debug.Log("DetermineHeight called age = " + age + " hv = " + heightVariation);
		height = age * heightVariation;
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
			float result = maxHeight ();
            totalHeight = result - y;
			//Debug.Log (totalHeight);
		}
	}

	//Recursive method to calculate height.
	private float maxHeight(){
        float highest = height * scale * Mathf.Sin (myAnglePermanant) + y;

		foreach (TreeModel branch in branches) {
			highest = Mathf.Max(highest, branch.maxHeight());
		}

		//Debug.Log ("maxheight returned " + highest);
		return highest;
	}

    public void startShake(){

        //Debug.Log("Shaking");

        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To( ()=>angleSway, x=> angleSway = x, 100f, 0.5f));
        seq.AppendInterval(1f);
        seq.Append(DOTween.To( ()=>angleSway, x=> angleSway = x, 10f, 3f));
        
        foreach (TreeModel branch in branches) {
            branch.startShake();
        }
    }
    
    public void GlowRed(){
        //Debug.Log("glowing red");
        /*
        _Mono branchMono = GetComponent<_Mono> ();
        GameObject branchGlow = Instantiate(blankGameObject, this.transform.position, this.transform.rotation) as GameObject;
        _Mono bm = branchGlow.AddComponent<_Mono> ();
        branchGlow.AddComponent<SpriteRenderer> ();
        bm.spriteRenderer.sortingLayerName = "TreeGlow";
        bm.spriteRenderer.sprite = angledSpriteRed;
        bm.xys = GetComponent<_Mono>().xys * 1f;
        //bm.angle = GetComponent<_Mono>().angle;
        FadeAndDestroy fd = branchGlow.AddComponent<FadeAndDestroy>();
        fd.alpha = 0.1f;
        fd.rate = 0.005f;

        if(foilage != null){
            _Mono foilMono = foilage.GetComponent<_Mono> ();
            GameObject foilGlow = Instantiate(blankGameObject, foilage.transform.position, foilage.transform.rotation) as GameObject;
            _Mono fgm = foilGlow.AddComponent<_Mono> ();
            foilGlow.AddComponent<SpriteRenderer> ();
            fgm.spriteRenderer.sortingLayerName = "TreeGlow";
            fgm.spriteRenderer.sprite = foilageSpriteRed;
            fgm.xys = foilage.GetComponent<_Mono>().xys * 1f;
            //fgm.angle = foilGlow.GetComponent<_Mono>().angle;
            FadeAndDestroy fd2 = foilGlow.AddComponent<FadeAndDestroy>();
            fd2.alpha = 0.1f;
            fd2.rate = 0.005f;
        }
                
        foreach (TreeModel branch in branches) {
            branch.GlowRed();
        }
        */
    }

    public void setAlphaRecursive(float a){
        alpha = a;

        if(foilage != null){
            _Mono foilMono = foilage.GetComponent<_Mono> ();
            foilMono.alpha = a * 1.5f;
        }

        
        foreach (FruitScript f in fruits) {
            f.alpha = a * 1.5f;
        }

        foreach (TreeModel branch in branches) {
            branch.setAlphaRecursive(a);
        }
    }

    
    public void DestroyRecursive(){
        if(foilage != null){
            Destroy(foilage);
        }
        
        foreach (TreeModel branch in branches) {
            branch.DestroyRecursive();
        }

        Destroy();
    }

    public void GrowFruit(){
        bool growHere = false;
        while( !growHere ){
            int branch = Random.Range(0, allBranches.Count);

            if( Random.Range(0, (int)Mathf.Pow(3, allBranches[branch].generation)) == 0 ) {
                allBranches[branch].AddFruitToThisBranch();
                growHere = true;
                numFruits++;
            }
        }
    }

    public void AddFruitToThisBranch(){
        fruits.Add(Instantiate(fruitPrefab, new Vector3(-10000f, -10000f, 0), Quaternion.identity) as FruitScript);
    }

    private void PositionFruits(){
        foreach (TreeModel branch in branches) {
            branch.PositionFruits();
        }

        foreach (FruitScript f in fruits){
            //Debug.Log("added fruit");
            f.xy = GetRootPosition (f.position);
        }
    }
}
