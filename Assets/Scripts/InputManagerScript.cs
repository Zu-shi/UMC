using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {

	public float inputX{ get; set;}
	public float inputY{ get; set;}
	public bool mouseMode;
	public KinectManager kinectManagerPrefab;
	private KinectManager kinectManager;
	
	// Use this for initialization
	void Start () {
		inputX = 0;
		inputY = 0;
		if(!mouseMode){
			kinectManager = Object.Instantiate(kinectManagerPrefab, Vector3.zero, Quaternion.identity) as KinectManager;
		}
	}

	public Vector2 inputPos {
		set {
			inputX = value.x;
			inputY = value.y;
		}
		get {
			return new Vector2(inputX, inputY);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseMode) {
			inputX = Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
			inputY = Camera.main.ScreenToWorldPoint (Input.mousePosition).y;
		} else {
			inputX = kinectManager.HandCursor1.x;
			inputY = kinectManager.HandCursor1.y;
			// Kinect controls to be implemented.
		}

        if (Input.GetKeyDown (KeyCode.Return)) {  
            //Application.LoadLevel (0);
            Globals.RestartTreeScene(false);
        }  
		//Debug.Log ("inputX = " + inputX + " inputY = " + inputY);
	}
}
