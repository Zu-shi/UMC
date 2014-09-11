using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TreeManagerScript : _Mono {

	//Only these two variables should be exposed at all times.
	public TreeModel mainTree { get; set; }
    public float currentGrowthPercentage = 0f;
    private float targetGrowthPercentage = 1.5f/7f;
    public int seed;
    public TreeModel[] morphingStagesPrefab;
    public Vector2[] morphingRanges;
    public Vector2 treePos = Vector2.zero;

    private bool inCutscene = true;
	private Vector2 stage1Range { get; set; }
	private Vector2 stage2Range { get; set; }
	private float targetAge { get; set; }
	private float maxAge { get; set; }
	public List<TreeModel> morphingStages { get; set; }
	private int morphingIndex = 0;
    private float currentEvaluation = 0.3f;
    private float stage1Evaluation = 0f;
    private float stage2Evaluation = 0f;
    private float stage3Evaluation = 0f;
    private Sequence seq;

	// Use this for initialization
	protected virtual void Start () {
		inCutscene = false;

		if (!Globals.fixedHeightMode) {

			maxAge = 400f;
			stage1Range = new Vector2 (3/7f, 4/7f);
			stage2Range = new Vector2 (4/7f, 7/7f);
			stage1Evaluation = 0f;
			stage2Evaluation = 0f;

			morphingStages = new List<TreeModel> ();
			Utils.Assert (morphingStagesPrefab != null, "Check morphingStagesPrefab not null.");
			Utils.Assert (morphingStagesPrefab.Length != 0, "Check morphingStagesPrefab not empty.");
			if (!Globals.fixedHeightMode) {

                mainTree = Instantiate(morphingStagesPrefab[0], treePos, Quaternion.identity) as TreeModel;
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
        mainTree.targetAge = maxAge * currentGrowthPercentage;
        foreach(TreeModel tree in morphingStages){
            tree.targetAge = mainTree.targetAge;
        }
	}

    private void EndGame(){

    }

	private bool DoneGrowing () {
		return (currentGrowthPercentage >= targetGrowthPercentage);
	}
	
    public void StartGame(float duration){
        Debug.Log ("TreeManager.StartGame");
        inCutscene = true;
        //mainTree.targetAge
        Tween t = DOTween.To(() => currentGrowthPercentage, x => currentGrowthPercentage = x, targetGrowthPercentage, duration / 4 * 3);
        t.Play();
        //seq.Append(t);
        //seq.Play();
        //seq.AppendCallback(AppendCameraSequence);
    }

    public void StartCutscene(float duration){
        Debug.Log ("TreeManager.StartCutscene");
        inCutscene = true;

        switch (Globals.stateManager.currentStage) {
            case Globals.STAGE_ONE: {
                stage1Evaluation = currentEvaluation; 
                targetGrowthPercentage = stage1Evaluation * (stage1Range.y - stage1Range.x) + stage1Range.x;
                break;
            }
            case Globals.STAGE_TWO: {
                EvaluateStage2(); 
                targetGrowthPercentage = stage2Evaluation;
                break;
            }
        }

        seq = DOTween.Sequence();
        seq.Append(DOTween.To( ()=>currentGrowthPercentage, x=> currentGrowthPercentage = x, targetGrowthPercentage, duration/2));
        seq.AppendCallback(AppendCameraSequence);
	}

    private void AppendCameraSequence() {
        Globals.cameraManager.appendSequence(mainTree.totalHeight);
    }
	
    private void EvaluateStage2() {
        stage2Evaluation = currentEvaluation * ((stage2Range.y - stage2Range.x) / 3 * 2) + 
            stage2Range.x + 
            stage1Evaluation * ((stage2Range.y - stage2Range.x) / 3);
    }

	public void EndCutscene(){
		Debug.Log ("TreeManager.EndCutscene");
        inCutscene = false;
	}

    
    private void GrowTree(){
        /*
        if (currentGrowthPercentage < targetGrowthPercentage)
        {
            currentGrowthPercentage += growthRate;
        } else
        {
            if(!appendedSequence){
                Globals.cameraManager.appendSequence(mainTree.totalHeight);
            }
        }
        
        mainTree.targetAge = maxAge * targetGrowthPercentage;
        if (!Globals.fixedHeightMode) {
            foreach(TreeModel tree in morphingStages){
                tree.targetAge = targetAge;
            }
        }
        */
    }
}
