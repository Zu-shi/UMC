using UnityEngine;
using System.Collections;

public class TreeModelPreMorph : TreeModel {
	
	protected override void Awake () {
		base.Awake ();
		branchingOptions = new int[]{2, 3};
		branchingProbability = new float[]{0.5f, 0.5f};
		branchingAngles = new float[]{ 10f, 10f};
		foilagePosition = new Vector2 (0.5f, 0.2f);
		foilageXs = 0.1f;
		foilageYs = 0.1f;
	}

	// Takes care of new branch generation, including setting a unique seed for the child based on this seed.

	protected override void TryBranch() {
		if (WillBranch()) {
			ageSinceLastBranch = 0f;
			
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
				t.myAnglePermanant = t.myAngle;// + (90f - t.angleCentral)/Mathf.Abs (90f - t.angleCentral) * Random.Range(8f, 15f);
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

}
