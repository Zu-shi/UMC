#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraManagerScript : _Mono {
	
   // private bool isGameOver = false;
    private bool inCutscene = true;
	private DG.Tweening.Core.TweenCallback cb;
	private float maxHeight = 840f;
	private float _height;
	private float panTime = 2f;
	private Tween t;
	private Tweener pan;
	
	private float height;
    private float nextCutsceneHeight;
	private bool _cutsceneMode = true;
    private float minimumCameraSize = 10f;
    private float startGameBeginHeight = 10f;
    private float startGameEndHeight = 140f;

	private float cutScene1Height = 480f;
	private float cutScene2Height = 800f;
	private float treeHeight;
	private float actualHeight;
    private Tweener ts;
    private Sequence cutscene;
    private float durationCache;

    public float cameraRatio{
        get {return Camera.main.orthographicSize / startGameEndHeight;}
    }

	void Start () {
//        startGameEndHeight = Globals.INITIAL_HEIGHT;
	}

	void Update () {
		if (Globals.fixedHeightMode) {
			if (height < maxHeight) {
				height += 2f;
			}
		}
		//treeHeight = Globals.treeManager.mainTree.totalHeight;
		
		float actualHeight = Mathf.Max (minimumCameraSize, height);
		Camera.main.orthographicSize = actualHeight;
		Camera.main.transform.position = new Vector3(Globals.treeManager.treePos.x, 
		                                             actualHeight,
		                                             Camera.main.transform.position.z);
	}
	
    public void SetHeight (float h) {
        height = h;
        float actualHeight = Mathf.Max (minimumCameraSize, height);
        Camera.main.orthographicSize = actualHeight;
        Camera.main.transform.position = new Vector3(Globals.treeManager.treePos.x, 
                                                     actualHeight,
                                                     Camera.main.transform.position.z);

    }
    /*
	void SetCutScene(bool v, DG.Tweening.Core.TweenCallback cb) {
		if(pan.IsPlaying()){
			if(v){pan = DOTween.To(()=> height, x=> height = x, cutScene1Height, panTime).OnComplete(cb); 
				_cutsceneMode = v;}
			else{pan = DOTween.To(()=> height, x=> height = x, treeHeight, panTime).OnComplete(cb);
				_cutsceneMode = v;}
		}else{
			Debug.Log("Tweener already in effect, SetCutScene Ignoed.");
		}
	}*/
	
	public void GameStart(float duration){
		Debug.Log ("CameraManager.StartGame");
        inCutscene = true;
        height = startGameBeginHeight;
        durationCache = duration;

        cutscene = DOTween.Sequence();
        cutscene.Append( DOTween.To(()=> height, x=> height = x, startGameEndHeight, duration/2) );
        cutscene.Play();
	}

	public void CutsceneStart(float duration){
		Debug.Log ("CameraManager.StartCutscene");
        inCutscene = true;

        durationCache = duration;
        cutscene = DOTween.Sequence();
        cutscene.Append( DOTween.To(()=> height, x=> height = x, nextCutsceneHeight, duration/6) );
        cutscene.AppendInterval(duration / 3 * 2);
        cutscene.Play();
	}
	
    public void GameOver(float duration){
        //isGameOver = true;

        if(Globals.stateManager.currentStage != Globals.STAGE_THREE){
            durationCache = duration;
            cutscene = DOTween.Sequence();
            cutscene.Append( DOTween.To(()=> height, x=> height = x, nextCutsceneHeight, duration/6) );
            cutscene.AppendInterval(duration / 3 * 2);
            cutscene.Play();
        }else{
            float currentHeightTemp = height;
            cutscene.Append( DOTween.To(()=> height, x=> height = x, Globals.treeManager.mainTree.totalHeight/1.5f, duration/6) );
            cutscene.AppendInterval(duration / 3 * 2);
            cutscene.Append( DOTween.To(()=> height, x=> height = x, currentHeightTemp, duration/6) );
            cutscene.Play();
        }
    }

	public void CutsceneEnd(){
		Debug.Log ("CameraManager.EndCutscene");
        inCutscene = false;
        
        switch (Globals.stateManager.currentStage) {
            case Globals.STAGE_STARTING: {nextCutsceneHeight = cutScene1Height; break;}
            case Globals.STAGE_ONE: {nextCutsceneHeight = cutScene2Height; break;}
        }

        if (cutscene.IsPlaying())
        {
            Debug.LogError("cutScene still playing when EndCutScene called");
        }
	}

    public void AppendSequence(float targetHeight){
        Debug.Log("Appended target height" + targetHeight);
        cutscene.Append( DOTween.To(()=> height, x=> height = x, targetHeight, durationCache/6) );
    }
}
