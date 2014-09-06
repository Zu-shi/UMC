using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraManagerScript : _Mono {
	
    private bool inCutscene = true;
	private DG.Tweening.Core.TweenCallback cb;
	private float maxHeight = 840f;
	private float _height;
	private float panTime = 2f;
	private Tween t;
	private Tweener pan;
	
	private float height;
	private bool _cutsceneMode = true;
    private float minimumCameraSize = 10f;
    private float startGameBeginHeight = 10f;
    private float startGameEndHeight = 140f;

	private float cutScene1Height = 480f;
	private float cutScene2Height = 960f;
	private float treeHeight;
	private float actualHeight;
    private Tweener ts;
    private Sequence cutscene;
    private float durationCache;

	void Start () {

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
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 
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
	
	public void StartGame(float duration){
		Debug.Log ("CameraManager.StartGame");
        inCutscene = true;
        height = startGameBeginHeight;
        durationCache = duration;

        cutscene = DOTween.Sequence();
        cutscene.Append( DOTween.To(()=> height, x=> height = x, startGameEndHeight, duration/2) );
        cutscene.Play();
	}

	public void StartCutscene(float duration){
		Debug.Log ("CameraManager.StartCutscene");
        inCutscene = true;

        durationCache = duration;
        cutscene = DOTween.Sequence();

        float toHeight = 0f;
        switch (Globals.stateManager.currentStage) {
            case Globals.STAGE_ONE: {toHeight = cutScene1Height; break;}
            case Globals.STAGE_TWO: {toHeight = cutScene2Height; break;}
            default: {toHeight = cutScene2Height; break;}
        }
        cutscene.Append( DOTween.To(()=> height, x=> height = x, toHeight, duration/6) );
        cutscene.AppendInterval(duration / 3 * 2);
        //cutscene.Append( DOTween.To(()=> height, x=> height = x, toHeight, duration/3 * 2) );
        cutscene.Play();
	}
	
	public void EndCutscene(){
		Debug.Log ("CameraManager.EndCutscene");
        inCutscene = false;

        if (cutscene.IsPlaying())
        {
            Debug.LogError("cutScene still playing when EndCutScene called");
        }
	}

    public void appendSequence(float targetHeight){
        Debug.Log("Appended target height" + targetHeight);
        cutscene.Append( DOTween.To(()=> height, x=> height = x, targetHeight, durationCache/6) );
    }
}
