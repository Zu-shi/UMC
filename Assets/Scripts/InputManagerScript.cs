using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {

	public float inputX{ get; set;}
	public float inputY{ get; set;}
	public Vector2 inputPos {
		set {
			inputX = value.x;
			inputY = value.y;
		}
		get {
			return new Vector2(inputX, inputY);
		}
	}
	public bool mouseMode;

	// Use this for initialization
	void Start () {
		inputX = 0;
		inputY = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseMode) {
			inputX = Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
			inputY = Camera.main.ScreenToWorldPoint (Input.mousePosition).y;
		} else {
			
		}

	}
}
