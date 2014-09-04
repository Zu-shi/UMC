using UnityEngine;
using System.Collections;
using System.Linq;

public class AgeBasedTreeMorpher : _Mono {
	
	public TreeModel preMorph;
	public TreeModel postMorph;
	public TreeModel toMorph;
	public float startAge;
	public float endAge;
	private bool disabled = false;
	private TreeManagerScript tsc = null;

	// Use this for initialization
	void Start () {
		
		Utils.Assert (preMorph.seed == postMorph.seed, "Check random seed equality.");
		Utils.Assert (Enumerable.SequenceEqual(preMorph.branchingOptions, postMorph.branchingOptions), "check growth options equality.");
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if(!disabled){
			if(toMorph.age > startAge){
				float processPreClamp = (toMorph.age - startAge) / (endAge - startAge);
				float process = Mathf.Clamp(processPreClamp, 0f, 1f);

				//Utils.Assert (preMorph.branches.Count == postMorph.branches.Count, "Check branch number equality.");
				if(preMorph.allBranches.Count == postMorph.allBranches.Count){
					for(int i = 0; i < preMorph.allBranches.Count; i++){
						TreeModel preB = preMorph.allBranches[i];
						TreeModel postB = postMorph.allBranches[i];
						TreeModel toB = toMorph.allBranches[i];
						
						toB.relativeRoot = Vector2.Lerp(preB.relativeRoot, postB.relativeRoot, process);
						toB.foilagePosition = Vector2.Lerp(preB.foilagePosition, postB.foilagePosition, process);
						toB.foilageXs = Mathf.Lerp(preB.foilageXs, postB.foilageXs, process);
						toB.foilageYs = Mathf.Lerp(preB.foilageYs, postB.foilageYs, process);
						toB.myAnglePermanant = Mathf.Lerp(preB.myAnglePermanant, postB.myAnglePermanant, process);
						toB.angleCentral = Mathf.Lerp(preB.angleCentral, postB.angleCentral, process);
					}
				}else{
					Debug.Log("Index unequal");
				}

				if(processPreClamp > 1f){
					toMorph.branchingAngles = postMorph.branchingAngles;
					toMorph.foilageXs = postMorph.foilageXs;
					toMorph.foilageYs = postMorph.foilageYs;
					toMorph.foilagePosition = postMorph.foilagePosition;

					disabled = true;
				}
			}
		}
	}
}
