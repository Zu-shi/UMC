﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CursorScript : _Mono {

    public GameObject circlePrefab;
    public GUIText descriptionAreaPrefab;
    public Sprite treeSceneCursor;
    public Sprite idleSceneCursor;
    public Sprite idleSceneTreeCursor;
    public Sprite idleSceneBadTreeCursor;
    public GameObject clickWave;
    private bool _allowClicks;
    private _Mono circleHint;
    public bool allowClicks{
        get{return _allowClicks;} 
        set{
            _allowClicks = value;
            if(!value){
                DOTween.To(() => alpha, x => alpha = x, 0, 0.7f);
            }
        }
    }

    _Mono leftArrow;
    _Mono rightArrow;

    _Mono saplingIcon;
    _Mono treeBound;
    float waitTimeBeforeClick = 0.8f;
    float clickTimer = 0.9f;
    //float detectMoveDistance = 8f; //Old distance for Mouse
    float detectMoveDistance = 14f / 800f;
    bool waitingForClick = true;
    bool hasTree = false;
    bool inValidRegion = false;
    float cameraxBound = 1730f;

    float originalXs = 0f;
    float originalYs = 0f;
    float flashesPerSecond = 1f;
    float alphaAngle = 0f;
    float xprev;
    float yprev;
    //float yprev;
    int switchCounter = 0;
    GUIText descriptionGUIText;

	// Use this for initialization
	void Start () {
        circleHint = (GameObject.Instantiate(circlePrefab) as GameObject).AddComponent<_Mono>();
        circleHint.alpha = 0.3f;
        descriptionGUIText = GameObject.Instantiate(descriptionAreaPrefab);
        saplingIcon = GameObject.Find("SaplingIcon").GetComponent<_Mono>();
        treeBound = GameObject.Find("TreeArea").GetComponent<_Mono>();
        leftArrow = GameObject.Find("LeftArrow").GetComponent<_Mono>();
        rightArrow = GameObject.Find("RightArrow").GetComponent<_Mono>();
        Cursor.visible = false;
        alpha = 0.7f;
        originalXs = xs;
        originalYs = ys;
	}
	
    void Update (){
        _Mono cameraMono = Camera.main.GetComponent<_Mono>();
        if(Globals.globalManager.currentScene.Equals(GlobalManagerScript.Scene.IDLE)){
            leftArrow.alpha = Mathf.Min(1f, 1 + cameraMono.x / cameraxBound);
            rightArrow.alpha = Mathf.Min(1f, 1 - cameraMono.x / cameraxBound);
        }else{
            leftArrow.alpha = rightArrow.alpha = 0f;
        }
    }
	// Update is called once per frame
    void LateUpdate () {
        _Mono cameraMono = Camera.main.GetComponent<_Mono>();
        Clickable scrollAreaTest = Scout(x,y);
        if(scrollAreaTest!=null && Globals.globalManager.currentScene.Equals(GlobalManagerScript.Scene.IDLE)){
            if(scrollAreaTest.nickname == "ScrollAreaLeft"){
                if(cameraMono.x > -cameraxBound){
                    cameraMono.x -= 15f;
                }
            }else if(scrollAreaTest.nickname == "ScrollAreaRight"){
                if(cameraMono.x < cameraxBound){
                    cameraMono.x += 15f;
                }
            }
        }

        switch(Globals.globalManager.currentScene){
            case(GlobalManagerScript.Scene.IDLE):{
                treeBound.alpha = 1;
                spriteRenderer.sprite = idleSceneCursor;
                xys = new Vector2(30, 30);
                if(hasTree){
                    Clickable validAreaTest = Scout(x,y);
                    if(validAreaTest!=null && validAreaTest.nickname == "ValidArea"){
                        saplingIcon.spriteRenderer.sprite = idleSceneTreeCursor;
                        saplingIcon.alpha = 0.9f;
                    }else{
                        saplingIcon.spriteRenderer.sprite = idleSceneBadTreeCursor;
                        saplingIcon.alpha = 0.25f;
                    }
                    saplingIcon.x = x;
                    saplingIcon.y = y + saplingIcon.spriteRenderer.sprite.rect.height/2;
                }else{
                    treeBound.GetComponent<CameraFollowingScript>().LateUpdate();
                    saplingIcon.xy = treeBound.xy;
                }
                treeBound.alpha = 1f;
                break;
            }
            case(GlobalManagerScript.Scene.TREE):{
                spriteRenderer.sprite = idleSceneCursor;
                float sizeRatio = Utils.halfScreenHeight / Utils.resolutionWidth;
                xs = sizeRatio * originalXs;
                ys = sizeRatio * originalYs;
//                Debug.Log(sizeRatio);
//                Debug.Log(originalXs);
                saplingIcon.xy = new Vector2(-10000,-10000);
                treeBound.alpha = 0f;
                break;
            }
        }
        AdministerClicks();

        if (allowClicks){
            alphaAngle += Mathf.PI / flashesPerSecond * Time.deltaTime;
            alpha = 0.55f + 0.2f * Mathf.Sin(alphaAngle);
        }

        if(x - xprev > Camera.main.orthographicSize/300f){
            switchCounter++;
            if(switchCounter > 5){
                angle = -10f;
            }
        }else if(xprev - x > Camera.main.orthographicSize/300f){
            switchCounter++;
            if(switchCounter > 5){
                angle = 10f;
            }
        }else{
            switchCounter = 0;
            angle = 0f;
        }

        xprev = x;
        yprev = y;
        z = 0;

        if(clickTimer < 0.4f && clickTimer > 0f){
            circleHint.xy = xy;
            circleHint.xys = xys * clickTimer / 10f;
        }else{
            circleHint.xys = Vector2.zero;
        }
	}

    void AdministerClicks(){
        descriptionGUIText.text = "";
        if(allowClicks){
            
            Clickable descriptionTest = Scout(x,y);
            if(descriptionTest!=null && Globals.globalManager.currentScene.Equals(GlobalManagerScript.Scene.IDLE)){
                if(descriptionTest.nickname == "Description"){
                    TreeContainerScript tcs = descriptionTest.GetComponent<TreeContainerScript>();
                    if(tcs.creator.Trim() != ""){
                        descriptionGUIText.text = "Tree by " + tcs.creator + "\n" + tcs.height + " inches tall" + "\nPlanted on " + tcs.date;
                    }else{
                        descriptionGUIText.text = tcs.height + " inches tall" + "\nPlanted on " + tcs.date;}
                }
            }


            clickTimer -= Time.deltaTime;
            
            if( Utils.PointDistance(new Vector2(x, y), new Vector2(xprev, yprev)) / Camera.main.orthographicSize > detectMoveDistance ){
                clickTimer = waitTimeBeforeClick;
                waitingForClick = true;
            }
            
            if(hasTree){
                Clickable clickObj = Scout(x, y);
                if(clickObj != null){
                    if(clickObj.nickname == "ValidArea"){
                        saplingIcon.spriteRenderer.sprite = idleSceneTreeCursor;
                    }else{
                        saplingIcon.spriteRenderer.sprite = idleSceneBadTreeCursor;
                    }
                }else{
                    saplingIcon.spriteRenderer.sprite = idleSceneBadTreeCursor;
                }
            }else{
                saplingIcon.spriteRenderer.sprite = idleSceneTreeCursor;
            }
            
            if(clickTimer < 0f && waitingForClick){
                waitingForClick = false;
                Clickable clickObj = Click(x, y);
                if (clickObj != null){
                    if(clickObj.nickname == "TreeArea"){
                        hasTree = !hasTree;
                    }
                    else if(clickObj.nickname == "ValidArea" && hasTree){
                        Globals.globalManager.InitiateTreeScene(x, y, false);
                        hasTree = false;
                    }
                    else if(clickObj.nickname == "GameOver" && Globals.globalManager.currentScene.Equals(GlobalManagerScript.Scene.TREE)){
                        //Debug.LogWarning("Game Over Clicked");
                        Globals.RestartIdleScene();
                    }else if(clickObj.nickname == "VirtualKeyboard"){
                        clickObj.gameObject.GetComponent<KeyboardKeyScript>().AlertVirtualKeyboardManager();
                    }
                }
            }
        }
    }

    public Clickable Click(float xpos, float ypos){
        Debug.Log("Click at " + xy.x + ", " + xy.y);
        Instantiate(clickWave, xy, Quaternion.identity);
        Clickable clickObj = Scout(xpos, ypos);
        if(clickObj != null){
            Debug.Log("Clicked on "+ clickObj.nickname);
        }
        return clickObj;
    }

    public Clickable Scout(float xpos, float ypos){
        RaycastHit hitInfo;
        //Debug.Log(transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
        z = -100; //Changing position for Raycasting
        Physics.Raycast(transform.position, new Vector3(0,0,1), out hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Clickable"));
        z = 0;
        if(hitInfo.collider == null){
            return null;
        }else{
            if(hitInfo.collider.GetComponent<Clickable>() == null){
                Debug.LogError("Collider did not have a clickable object");
            }
            return hitInfo.collider.GetComponent<Clickable>();
        }
        //(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers);
    }
}
