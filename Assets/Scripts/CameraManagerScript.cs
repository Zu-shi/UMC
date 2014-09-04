using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraManagerScript : _Mono {
	
	private DG.Tweening.Core.TweenCallback cb;
	private float maxHeight = 840f;
	private float _height;
	private float panTime = 2f;
	private Tween t;
	private Tweener pan;
	
	private float height;
	private bool _cutsceneMode = true;
	private float minimumCameraSize = 250f;
	private float cutScene1Height = 100f;
	private float cutScene2Height;
	private float treeHeight;
	private float actualHeight;

	void Start () {

	}

	void Update () {
		
		if (Globals.fixedHeightMode) {
			if (height < maxHeight) {
				height += 2f;
			}
		}
		treeHeight = Globals.treeManager.mainTree.totalHeight;
		
		float actualHeight = Mathf.Max (minimumCameraSize, height);
		Camera.main.orthographicSize = actualHeight;
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 
		                                             actualHeight,
		                                             Camera.main.transform.position.z);
	}
	
	void SetCutScene(bool v, DG.Tweening.Core.TweenCallback cb) {
		if(pan.IsPlaying()){
			if(v){pan = DOTween.To(()=> height, x=> height = x, cutScene1Height, panTime).OnComplete(cb); 
				_cutsceneMode = v;}
			else{pan = DOTween.To(()=> height, x=> height = x, treeHeight, panTime).OnComplete(cb);
				_cutsceneMode = v;}
		}else{
			Debug.Log("Tweener already in effect, SetCutScene Ignoed.");
		}
	}

	public void StartCutscene(){
		Debug.Log ("CameraManager.StartCutscene");
	}
	
	public void EndCutscene(){
		Debug.Log ("CameraManager.EndCutscene");
	}
}
