using UnityEngine;
using System.Collections;
using System.Linq;

public class TreeMorpher : _Mono {

	/*
	public TreeModel preMorph;
	public TreeModel postMorph;
	public TreeModel toMorph;
	private float startHeight = 250f;
	private float endHeight = 500f;

	// Use this for initialization
	void Start () {

		Utils.Assert (preMorph.seed == postMorph.seed, "Check random seed equality.");
		Utils.Assert (Enumerable.SequenceEqual(preMorph.branchingOptions, postMorph.branchingOptions), "check growth options equality.");

	}
	
	// Update is called once per frame
	void LateUpdate () {

		if(Globals.heightManager.height > startHeight && Globals.heightManager.height < endHeight){
			float process = (Globals.heightManager.height - startHeight) / (endHeight - startHeight);
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
				}
			}else{
				Debug.Log("Index unequal");
			}
		}
	}
	*/
}
