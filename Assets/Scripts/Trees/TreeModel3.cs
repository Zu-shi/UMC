using UnityEngine;
using System.Collections.Generic;

//NOT USED
public class TreeModel3 : TreeModel {
	
	// Takes care of new branch generation, including setting a unique seed for the child based on this seed.
	protected override void TryBranch() {
		foilGen = 1;
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
				TreeModel t = ((GameObject)Instantiate(this.gameObject, new Vector3(-10000f, -10000f, 0f), Quaternion.identity)).GetComponent<TreeModel>();
				Utils.Assert(t != null, "Check branch creating successful");
				//t.Start();
				t.angleCentral = tempA;
				if(t.angleCentral != 90f)
					t.angleCentral += (90f - t.angleCentral)/Mathf.Abs (90f - t.angleCentral) * Random.Range(0f, -10f);
				t.generation = generation + 1;
				t.proportion = proportion / 1.1f;
				t.relativeRoot = new Vector2( 0.5f, 0.8f );
				t.seed = Random.Range(0, 10000);
				t.doneGrowing = false;
				t.myAngle = t.angleCentral;
				t.myAnglePermanant = t.myAngle;
				//t.gameObject.transform.parent = this.gameObject.transform;
				
				for( int i2 = 0; i2 < branchingAngles.Length; i2++ ){
					//Debug.Log( t.branchingAngles );
					//Debug.Log( t.branchingAngles[0] );
					t.branchingAngles[i2] = branchingAngles[i2]/2;
				}
				
				branches.Add (t);
				//Debug.Log( "Created New Branch." );
				
				tempA += a;
			}
		}
	}

}
