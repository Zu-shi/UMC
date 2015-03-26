using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TreeManagerScript : _Mono {

    //Only these two variables should be exposed at all times.
    public float currentGrowthPercentage = 0f;
    public int seed;
    public TreeModel[] morphingStagesPrefab;
    public Vector2[] morphingRanges;
    public Vector2 treePos = Vector2.zero;
    public float currentEvaluation = 1f;
    
    public TreeModel mainTree { get; set; }
    public int secondsForFirtstPart{get; set;}
    public int secondsForSecondPart{get; set;}
    public int secondsForThirdPart{get; set;}
    public List<TreeModel> morphingStages { get; set; }

    private float stage1performance = 1f;
    private float stage2performance = 1f;
    private bool isGameOver = false;
    //private float targetGrowthPercentage = 1.4f/7f;
    private float START_PERCENTAGE = 1.4f/7f;
    private float targetGrowthPercentage = 1.4f/7f;
    private float secondsSurvivedInStage3 = 0f;
    //private float secondsSurvivedInStage2 = 0f;
    //private float secondsSurvivedInStage1 = 0f;
    private float secondsSurvived = 0f;
    private int stage2HazardCount;
    private int stage1HazardCount;
    private bool inCutscene = true;
	private Vector2 stage1Range { get; set; }
	private Vector2 stage2Range { get; set; }
	private float targetAge { get; set; }
	private float maxAge { get; set; }
    private float stage1Evaluation;
    private float stage2Evaluation;
    private Sequence seq;
    private bool monitorForFruits = false;
    private int fruitCachedNumbers = 0;  //Tracks changes in fruitTweenedNumebers
    private int fruitTweenedNumbers = 0;  //Is tweened to create cool effects in fruit intervals.

	// Use this for initialization
	protected virtual void Start () {
		inCutscene = false;
        currentGrowthPercentage = 0f;
        DOTween.To(() => currentGrowthPercentage, x => currentGrowthPercentage = x, targetGrowthPercentage, 1.5f);
//        Debug.LogError("test");

		if (!Globals.fixedHeightMode) {

			maxAge = 370f;
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
                //mainTree.growFoilage = false;

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
	
    public void gotHit(){
        
        switch (Globals.stateManager.currentStage) {
            case Globals.STAGE_ONE: {
                stage1performance -= 0.1f;
                break;
            }
            case Globals.STAGE_TWO: {
                stage2performance -= 0.1f;
                break;
            }
            //Stage three does not need a penalty for getting hit.
        }

    }

    public void GrowthSpurt () {
        //targetGrowthPercentage = Mathf.Min(Mathf.Min(secondsSurvived / 100f + 0.14f, currentGrowthPercentage + 0.05f));
        //targetGrowthPercentage = targetGrowthPercentage + 0.01f;
        targetGrowthPercentage = targetGrowthPercentage + 0.1f;
        Debug.Log("Growing to " + targetGrowthPercentage);
        Sequence s = DOTween.Sequence();
        Tween t = DOTween.To(() => currentGrowthPercentage, x => currentGrowthPercentage = x, currentGrowthPercentage, EssenceScript.rewardDelay);
        //rewardDelay
        Tween t2 = DOTween.To(() => currentGrowthPercentage, x => currentGrowthPercentage = x, targetGrowthPercentage, 0.7f);
        s.Append(t);
        s.Append(t2);
        t.SetEase(Ease.Linear);
        //currentGrowthPercentage = targetGrowthPercentage;
    }

    protected virtual void Update () {
        Globals.cameraManager.SetHeight(mainTree.totalHeight * 4 / 3);

        mainTree.targetAge = maxAge * currentGrowthPercentage;

        //FOR TESTING NEW GRAPHICS
        mainTree.targetAge = Mathf.Min(370, mainTree.targetAge);

        foreach(TreeModel tree in morphingStages){
            tree.targetAge = mainTree.targetAge;
        }

        if(monitorForFruits){
            if(fruitTweenedNumbers > fruitCachedNumbers) {
                if(fruitCachedNumbers < fruitTweenedNumbers - 1){Debug.Log("At least the " + fruitCachedNumbers+ "th fruit was skipped.");}
                fruitCachedNumbers = fruitTweenedNumbers;
                for(int i = 0; i < 1; i++){mainTree.GrowFruit();}
            }
        }
        
        if(!isGameOver && !inCutscene){
            secondsSurvived += Time.deltaTime;
        }
        /*
        if(!isGameOver && !inCutscene){
            if(!inCutscene && Globals.stateManager.currentStage == Globals.STAGE_TWO){
                secondsSurvivedInStage2 += Time.deltaTime;
            }else if(!inCutscene && Globals.stateManager.currentStage == Globals.STAGE_ONE){
                secondsSurvivedInStage1 += Time.deltaTime;
            }else if(!inCutscene && Globals.stateManager.currentStage == Globals.STAGE_THREE){
                secondsSurvivedInStage3 += Time.deltaTime;
            }
        }
        */
	}

	private bool DoneGrowing () {
		return (currentGrowthPercentage >= targetGrowthPercentage);
	}
	
    public float GetCutsceneTime(){
        if(Globals.stateManager.currentStage == Globals.STAGE_THREE){
            //Can implement fruit appears slower and slower 
            return Mathf.Floor(secondsSurvivedInStage3)/4 + 4;
        }else{
            return StateManagerScript.secondsPerCutscene;
        }
    }

    public void GameStart(float duration){
        Debug.Log ("TreeManager.StartGame");
        inCutscene = true;
        Tween t = DOTween.To(() => currentGrowthPercentage, x => currentGrowthPercentage = x, targetGrowthPercentage, duration / 4 * 3);
        t.Play();
    }

    public void CutsceneStart(float duration){

        Debug.Log ("TreeManager.StartCutscene");
        inCutscene = true;

        //This code is not necessary for part 3.
        seq = DOTween.Sequence();
        seq.Append(DOTween.To( ()=>currentGrowthPercentage, x=> currentGrowthPercentage = x, targetGrowthPercentage, duration/2));
        seq.AppendCallback(AppendCameraSequence);
	}

    public void StartGrowFruitsSequence(){
        monitorForFruits = true;

        seq = DOTween.Sequence();
        seq.AppendInterval(2f);
        seq.Append(DOTween.To( ()=>fruitTweenedNumbers, x=> fruitTweenedNumbers = x, 
                              Mathf.FloorToInt(secondsSurvivedInStage3), secondsSurvivedInStage3/4));
        seq.AppendInterval(2f);
    }

    public void GameOver(float duration){
        //mainTree.growFoilage = true;
        isGameOver = true;
        //targetGrowthPercentage = Mathf.Min(secondsSurvived / 100f + 0.14f, 1f);
        Debug.Log("TGP = " + targetGrowthPercentage);
        targetGrowthPercentage = Mathf.Clamp(targetGrowthPercentage, 0f, 1f);
        seq = DOTween.Sequence();
        seq.Append(DOTween.To( ()=>currentGrowthPercentage, x=> currentGrowthPercentage = x, targetGrowthPercentage, duration/2));
        seq.AppendCallback(AppendCameraSequence);
    }

    private void AppendCameraSequence() {
        Globals.cameraManager.AppendSequence(mainTree.totalHeight);
    }
	
    private void EvaluateStage2() {
        stage2Evaluation = currentEvaluation * ((stage2Range.y - stage2Range.x) / 3 * 2) + 
        stage2Range.x + 
        stage1Evaluation * ((stage2Range.y - stage2Range.x) / 3);
        stage2Evaluation *= stage2performance;
    }

	public void CutsceneEnd(){
		Debug.Log ("TreeManager.EndCutscene");
        inCutscene = false;
	}

    public void OnDestory() {
        Debug.Log("Treemanager.ondestory()");

        while(morphingStages.Count != 0){
            TreeModel t = morphingStages[0];
            morphingStages.Remove(t);
            t.DestroyRecursive();
        }
    }
}
