using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeManagerScript : _Mono {

	//Only these two variables should be exposed at all times.
	public TreeModel mainTree { get; set; }
	public float currentGrowthPercentage { get; set; }
	public float targetGrowthPercentage { get; set; }

	private float growthRate = 0.002f;
	private bool inCutscene { get; set; }
	private Vector2 stage1Range { get; set; }
	private Vector2 stage2Range { get; set; }
	private TreeModel[] morphingStagesPrefab;
	private Vector2[] morphingRanges;
	private int seed;
	private float stage1Evaluation { get; set; }
	private float stage2Evaluation { get; set; }
	private float targetAge { get; set; }
	private float maxAge { get; set; }
	private List<TreeModel> morphingStages { get; set; }
	private int morphingIndex = 0;

	// Use this for initialization
	protected virtual void Start () {
		inCutscene = false;

		if (!Globals.fixedHeightMode) {

			maxAge = 523f;
			stage1Range = new Vector2 (0.222f, 0.444f);
			stage2Range = new Vector2 (0.444f, 0.777f);
			targetAge = maxAge * 0.222f;
			stage1Evaluation = 0f;
			stage2Evaluation = 0f;
			currentGrowthPercentage = 0f;
			targetGrowthPercentage = 0.111f;


			morphingStages = new List<TreeModel> ();
			Utils.Assert (morphingStagesPrefab != null, "Check morphingStagesPrefab not null.");
			Utils.Assert (morphingStagesPrefab.Length != 0, "Check morphingStagesPrefab not empty.");
			if (!Globals.fixedHeightMode) {

				mainTree = Instantiate(morphingStagesPrefab[0], Vector3.zero, Quaternion.identity) as TreeModel;
				mainTree.seed = seed;

				TreeModel treeInstance = null;
				TreeModel previousTreeInstance = null;
				for(int i = 0; i < morphingStagesPrefab.Length; i++){
					//These trees are instantiated for transitioning use.
					TreeModel prefab = morphingStagesPrefab[i];
					treeInstance = Instantiate(prefab, new Vector3(-10000f, -10000f, 0f), Quaternion.identity) as TreeModel;
					Utils.Assert (treeInstance != null, "Check treeInstance not null.");
					morphingStages.Add( treeInstance );
					treeInstance.seed = mainTree.seed;

					if(previousTreeInstance != null){
						Utils.Assert (i > 0, "Check index i greater than 0.");
						AgeBasedTreeMorpher tm = this.gameObject.AddComponent<AgeBasedTreeMorpher>();
						tm.startAge = maxAge * morphingRanges[i - 1].x;
						tm.endAge = maxAge * morphingRanges[i - 1].y;
						tm.toMorph = mainTree;
						tm.preMorph = previousTreeInstance;
						tm.postMorph = treeInstance;
					}

					previousTreeInstance = treeInstance;
				}

			}			
		}
	}
	
	protected virtual void Update () {
		if (inCutscene) {
			if (currentGrowthPercentage < targetGrowthPercentage) {
				currentGrowthPercentage += growthRate;
			}
		}

		mainTree.targetAge = maxAge * targetGrowthPercentage;

		if (!Globals.fixedHeightMode) {
			foreach(TreeModel tree in morphingStages){
				tree.targetAge = targetAge;
			}
		}

	}

	private bool doneGrowing () {
		return (currentGrowthPercentage >= targetGrowthPercentage);
	}
	
	public void StartCutscene(){
		Debug.Log ("TreeManager.StartCutscene");
	}
	
	public void EndCutscene(){
		Debug.Log ("TreeManager.EndCutscene");
	}
}
