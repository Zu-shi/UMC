using UnityEngine;
using System.Collections;

public class CursorScript : _Mono {

    public Sprite treeSceneCursor;
    public Sprite idleSceneCursor;
    public Sprite idleSceneTreeCursor;
    public Sprite idleSceneBadTreeCursor;
    public GameObject clickWave;


    _Mono saplingIcon;
    _Mono treeBound;
    float waitTimeBeforeClick = 1.5f;
    float clickTimer = 1.5f;
    float detectMoveDistance = 8f;
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

	// Use this for initialization
	void Start () {
        saplingIcon = GameObject.Find("SaplingIcon").GetComponent<_Mono>();
        treeBound = GameObject.Find("TreeArea").GetComponent<_Mono>();
        Cursor.visible = false;
        alpha = 0.7f;
        originalXs = xs;
        originalYs = ys;
	}
	
	// Update is called once per frame
    void LateUpdate () {
        _Mono cameraMono = Camera.main.GetComponent<_Mono>();
        Clickable scrollAreaTest = Scout(x,y);
        if(scrollAreaTest!=null){
            if(scrollAreaTest.name == "ScrollAreaLeft"){
                if(cameraMono.x > -cameraxBound){
                    cameraMono.x -= 15f;
                }
            }else if(scrollAreaTest.name == "ScrollAreaRight"){
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
                    if(validAreaTest!=null && validAreaTest.name=="ValidArea"){
                        saplingIcon.spriteRenderer.sprite = idleSceneTreeCursor;
                        saplingIcon.alpha = 0.9f;
                    }else{
                        saplingIcon.spriteRenderer.sprite = idleSceneBadTreeCursor;
                        saplingIcon.alpha = 0.25f;
                    }
                    saplingIcon.x = x;
                    saplingIcon.y = y + saplingIcon.spriteRenderer.sprite.rect.height/2;
                }else{
                    treeBound.GetComponent<Clickable>().LateUpdate();
                    saplingIcon.xy = treeBound.xy;
                }
                treeBound.alpha = 1f;
                AdministerClicks();
                break;
            }
            case(GlobalManagerScript.Scene.TREE):{
                spriteRenderer.sprite = treeSceneCursor;
                float sizeRatio = Utils.halfScreenHeight / Utils.resolutionWidth;
                xs = sizeRatio * originalXs;
                ys = sizeRatio * originalYs;
                saplingIcon.xy = new Vector2(-10000,-10000);
                treeBound.alpha = 0f;
                break;
            }
        }

        alphaAngle += Mathf.PI / flashesPerSecond * Time.deltaTime;
        alpha = 0.55f + 0.2f * Mathf.Sin(alphaAngle);

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
	}

    void AdministerClicks(){
        clickTimer -= Time.deltaTime;
        
        if( Utils.PointDistance(new Vector2(x, y), new Vector2(xprev, yprev)) > detectMoveDistance ){
            clickTimer = waitTimeBeforeClick;
            waitingForClick = true;
        }
        
        if(hasTree){
            Clickable clickObj = Scout(x, y);
            if(clickObj != null){
                if(clickObj.name == "ValidArea"){
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
                if(clickObj.name == "TreeArea"){
                    hasTree = !hasTree;
                }
                else if(clickObj.name == "ValidArea"){
                    Globals.globalManager.InitiateTreeScene(x, y, false);
                    hasTree = false;
                }
            }
        }
    }

    public Clickable Click(float xpos, float ypos){
        Debug.Log("Click");
        Instantiate(clickWave, xy, Quaternion.identity);
        Clickable clickObj = Scout(xpos, ypos);
        if(clickObj != null){
            Debug.Log("Clicked on "+ clickObj.name);
        }
        return clickObj;
    }

    public Clickable Scout(float xpos, float ypos){
        RaycastHit hitInfo;
        //Debug.Log(transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
        z = -10; //Changing position for Raycasting
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
