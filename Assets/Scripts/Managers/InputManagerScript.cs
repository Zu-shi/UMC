using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {
    
    public enum ControlMethod { CARTESIAN, POLAR }; 

    public float inputX{ get; set; }
    public float inputY{ get; set; }
    public float inputNormX{ get; set; }
    public float inputNormY{ get; set; }
	public bool mouseMode;
	public KinectManager kinectManagerPrefab;
    public CursorScript handCursor;
    private KinectManager kinectManager;
    public ControlMethod control = ControlMethod.POLAR;

	// Use this for initialization
	void Start () {
        handCursor = GameObject.Find("HandCursor").GetComponent<CursorScript>();
		inputX = 0;
		inputY = 0;
		if(!mouseMode){
			kinectManager = Object.Instantiate(kinectManagerPrefab, Vector3.zero, Quaternion.identity) as KinectManager;
            kinectManager.HandCursor1 = handCursor;
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
            inputNormX = Input.mousePosition.x/Screen.width;
            inputNormY = Input.mousePosition.y/Screen.height;
            handCursor.x = inputX;
            handCursor.y = inputY;
		} else {
			inputX = kinectManager.HandCursor1.x;
			inputY = kinectManager.HandCursor1.y;
            inputNormX = kinectManager.screenPosX;
            inputNormY = kinectManager.screenPosY;
			// Kinect controls to be implemented.
		}

        if (Input.GetKeyDown (KeyCode.Return)) {  
            //Application.LoadLevel (0);
            Globals.RestartTreeScene(false);
        }  
		//Debug.Log ("inputX = " + inputX + " inputY = " + inputY);
	}

    public Vector2 normToScreenPoint (Vector2 v) {
        //Debug.Log("width2:" +v.x * Screen.width + "height2:" +v.y * Screen.height);
        return Camera.main.ScreenToWorldPoint (new Vector2(v.x * Screen.width, v.y * Screen.height));
    }

    
    public Vector2 normToScreenPoint (float x, float y) {
        //Debug.Log("width2:" +v.x * Screen.width + "height2:" +v.y * Screen.height);
        return Camera.main.ScreenToWorldPoint (new Vector2(x * Screen.width, y * Screen.height));
    }

    public Vector2 screenToNormPoint (float x, float y) {
        //Debug.Log("width2:" +v.x * Screen.width + "height2:" +v.y * Screen.height);
        return new Vector2( (x - Camera.main.transform.position.x) / Utils.halfScreenWidth / 2,
                           (y - (Camera.main.transform.position.y - Utils.halfScreenHeight)) / Utils.halfScreenWidth / 2);
    }
}
