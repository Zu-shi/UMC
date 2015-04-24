 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.;

public class InputManagerScript : MonoBehaviour {
    
    public enum ControlMethod { CARTESIAN, POLAR }; 
    
    public float inputX{ get; set; }
    public float inputY{ get; set; }
    public float inputNormX{ get; set; }
    public float inputNormY{ get; set; }
    [Tooltip("Whether to restrict controls to a smaller box in the center.")]
    public bool smallScreenMode;
    public bool mouseMode;
    
    #if UNITY_IPHONE
    #elif UNITY_ANDROID
    #else
    public KinectManager kinectManagerPrefab;
    private KinectManager kinectManager;
    #endif
    
    private CursorScript handCursor;
    public ControlMethod control = ControlMethod.POLAR;
    
    // Use this for initialization
    void Start () {
        handCursor = GameObject.Find("CursorManager").GetComponent<CursorScript>();
        inputX = 0;
        inputY = 0;
        #if UNITY_IPHONE
        #elif UNITY_ANDROID
        #else
        if(!mouseMode){
            kinectManager = Object.Instantiate(kinectManagerPrefab, Vector3.zero, Quaternion.identity) as KinectManager;
            kinectManager.HandCursor1 = handCursor;
        }
        #endif
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
            #if UNITY_IPHONE
            #elif UNITY_ANDROID
            #else
            inputX = kinectManager.HandCursor1.x;
            inputY = kinectManager.HandCursor1.y;
            inputNormX = kinectManager.screenPosX;
            inputNormY = kinectManager.screenPosY;
            #endif
            // Kinect controls to be implemented.
        }
        
        inputNormX = (smallScreenMode) ? (inputNormX - 0.5f) / 2f + 0.5f : inputNormX;
        inputNormY = (smallScreenMode) ? (inputNormY - 0.5f) / 2f + 0.5f : inputNormY;
        
        if (Input.GetKeyDown (KeyCode.Return)) {  
            //Application.LoadLevel (0);
            //Globals.RestartTreeScene(0, -100f + UnityEngine.Random.Range(0f, 200f), false);
            Globals.RestartIdleScene();
        }  
        //Debug.Log ("inputX = " + inputX + " inputY = " + inputY);
    }
    
    public static  Vector2 normToWorldPoint (Vector2 v) {
        //Debug.Log("width2:" +v.x * Screen.width + "height2:" +v.y * Screen.height);
        //return Camera.main.ScreenToWorldPoint (new Vector2(v.x * Screen.width, v.y * Screen.height));
        return Camera.main.ViewportToWorldPoint(v);
    }
    
    public static  Vector2 normToWorldPoint (float x, float y) {
        //Debug.Log("width2:" +v.x * Screen.width + "height2:" +v.y * Screen.height);
        //return Camera.main.ScreenToWorldPoint (new Vector2(x * Screen.width, y * Screen.height));
        return Camera.main.ViewportToWorldPoint(new Vector2(x, y));
    }

    public static  Vector2 normToWorldScale (Vector2 v) {
        return new Vector2(v.x * Camera.main.orthographicSize * Camera.main.aspect, v.y * Camera.main.orthographicSize) * 2;
    }

    public static Vector2 normToWorldScale (float x, float y) {
        //Debug.Log("width2:" +v.x * Screen.width + "height2:" +v.y * Screen.height);
        return new Vector2(x * Camera.main.orthographicSize * Camera.main.aspect, y * Camera.main.orthographicSize) * 2;
    }
    
    public static float normToWorldWidthScale (float x) {
        return x * Camera.main.orthographicSize * Camera.main.aspect * 2;
    }

    public Vector2 worldToNormPoint (float x, float y) {
        //Debug.Log("width2:" +v.x * Screen.width + "height2:" +v.y * Screen.height);
        return new Vector2( (x - Camera.main.transform.position.x) / Utils.halfScreenWidth / 2,
                           (y - (Camera.main.transform.position.y - Utils.halfScreenHeight)) / Utils.halfScreenWidth / 2);
    }
}
