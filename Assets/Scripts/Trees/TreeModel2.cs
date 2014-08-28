using UnityEngine;
using System.Collections.Generic;

public class TreeModel2 : TreeModel {

	private int blooms = 0;
	private float bloomVariation;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		proportion = 0.0525f;
		branches = new List<TreeModel> ();
		stopGrowingAfterBranching = false;
		maxGenerations = 1;
		height = 0f;
		branchingOptions = new int[]{2, 2, 2};
		branchingProbability = new float[]{0.34f, 0.33f, 0.33f};
		branchingAngles = new float[]{ 170f, 200f, 180f};
		foilGen = 1;
		foilagePosition = new Vector2 (0.5f, 0.1f);
	}

	protected override void PositionFoilage() {
		_Mono foilMono = foilage.GetComponent<_Mono> ();
		foilMono.xy = GetRootPosition (foilagePosition);
		foilMono.angle = myAngle - 90;
		foilMono.xs = xs * 3;
		foilMono.ys = ys;
	}

	// Decides whether the tree will branch right now (Based on the age of the tree and the random number generator)
	protected override bool WillBranch() {
		float threshold;

		if (blooms == 0) {
			threshold = (generation == 0) ? 70f : 100f - generation * 10;
		}else{
			threshold = (generation == 0) ? 20f : 100f - generation * 10 * bloomVariation;
		}

		if (ageSinceLastBranch > threshold && !doneGrowing) {
			bloomVariation = Random.Range(0.7f, 1.5f);
			if (ageSinceLastBranch > threshold && !doneGrowing) {
				if(generation < maxGenerations){blooms++; return true;}
				else if(stopGrowingAfterBranching){doneGrowing = true; return false;}
			}
		}
		
		return false;
	}
	
	// Takes care of new branch generation, including setting a unique seed for the child based on this seed.
	protected override void TryBranch() {
		if (WillBranch()) {
			ageSinceLastBranch = 0f;
			
			if(stopGrowingAfterBranching){
				doneGrowing = true;
			}
			
			int ind = DecideIndexForNumberOfBranches();
			int b = branchingOptions[ind];
			float a = branchingAngles[ind];
			float tempA = angleCentral - (b - 1) * a / 2f; //The -1 accounts for that there are b-1 angle gaps.
			
			for(int i = 0; i < b; i ++){
				TreeModel2 t = ((GameObject)Instantiate(this.gameObject, new Vector3(-10000f, -10000f, 0f), Quaternion.identity)).GetComponent<TreeModel2>();
				Utils.Assert(t != null, "Check branch creating successful");
				t.angleCentral = tempA;
				t.generation = generation + 1;
				t.proportion = 0.05f;
				t.relativeRoot = new Vector2( 0.5f, actualAge/maxAge );
				t.seed = Random.Range(0, 10000);
				t.doneGrowing = false;
				t.myAngle = t.angleCentral + (angleCentral - t.angleCentral)/Mathf.Abs (angleCentral - t.angleCentral) * Random.Range(-5f, 5f);
				t.maxAge = maxAge/(blooms + 1);
				t.myAnglePermanant = t.myAngle;
				t.Orient();
				//t.gameObject.transform.parent = this.gameObject.transform;
				
				for( int i2 = 0; i2 < branchingAngles.Length; i2++ ){
					t.branchingAngles[i2] = branchingAngles[i2]/2;
				}
				
				branches.Add (t);
				//Debug.Log( "Created New Branch." );
				
				tempA += a;
			}
		}
	}
	
}
