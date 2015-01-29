#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldScriptRounded : _Mono {

    public GameObject shieldLeafPrefab;
    public _Mono goldenShieldLeafPrefab;
    public Sprite centerLeafSprite;

    private int numLeaves = 3;
    private int angleSpread = 15;
    
    private float parabolaWidth = 0.64f;
    private float parabolaHeight = 0.64f;
    private float parabolaWidthRange = 0.1f;
    private float parabolaHeightRange = 0.3f;
    //Determine whether the leaves' positions are determined by y position of the player
    private bool parabolaRangeOn = true;

    private float swayRange = 21f;
    private float swayAngle = 0f;
    private float swayFreq = 3f;

    private float bigLeafSize = 0.45f;
    private float smallLeafSize = 0.2f;

    private List<_Mono> leaves;
    //private float cameraHeight;
    private float w;
    private Camera mainCamera;
    private float originalXs;
    private float originalYs;

    //The following variables are used for experimental mouse controls with 2 orbits
    private bool aboveTransitionThreshold = false;          //whether the mouse position is high enough to move to an outer orbit
    private float orbitalLerp;
    private float innerParabolaWidth = 0.5f;
    private float innerParabolaHeight = 0.5f;

    private bool freeMode = true;

    public bool outerShield{get{return orbitalLerp > 0.7f;}}
    public bool innerShield{get{return orbitalLerp < 0.3f;}}
	// Use this for initialization
    void Start () {
        //cameraHeight = Camera.main.orthographicSize;

        leaves = new List<_Mono>();
        for(int i = 0; i < numLeaves; i++){
            GameObject leaf = Object.Instantiate(shieldLeafPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            _Mono leafMono = leaf.GetComponent<_Mono>();
            leaves.Add(leafMono);

            if(isBigLeaf(i)){
                leafMono.gameObject.AddComponent<CenterLeafScript>().goldenLeaf = goldenShieldLeafPrefab;
                leafMono.spriteRenderer.sprite = centerLeafSprite;
            }

        }
	}

	// Update is called once per frame
	void Update () {
        swayAngle += swayFreq * Mathf.PI / (1f / Time.deltaTime);

        float centerAngle = (Globals.inputManager.control == InputManagerScript.ControlMethod.CARTESIAN) ?
            (1 - Globals.inputManager.inputNormX) * 180f : Utils.PointAngle(Globals.mainTreePos, Globals.inputManager.inputPos);

        int i = 0;

        foreach(_Mono l in leaves) {
            float angle = centerAngle - angleSpread/2f + i * angleSpread / (numLeaves - 1);

            //script for bouncing leaves near the end.
            if(angle > 180f){angle = 180f - (angle - 180f);}
            else if(angle < 0f){angle = -angle;}

            if(!freeMode){
                l.xy = Globals.inputManager.normToScreenPoint(getNormedXY(angle));
            }else{
                l.xy = Globals.inputManager.normToScreenPoint(getNormedXY(angle - centerAngle));
            }

            l.angle = angle;

            l.xs = isBigLeaf(i) ? bigLeafSize * Camera.main.orthographicSize / 480f : smallLeafSize * Camera.main.orthographicSize / 480f;
            l.ys = isBigLeaf(i) ? bigLeafSize * Camera.main.orthographicSize / 480f : smallLeafSize * Camera.main.orthographicSize / 480f;
            l.angle = angle - 90 + swayRange * Mathf.Cos(swayAngle + (float)i/numLeaves * Mathf.PI / 2f);

            i++;
        }
	}

    bool isBigLeaf(int i){
        return i == Mathf.FloorToInt(numLeaves/2f);
    }

    //Angle in degrees.
    Vector2 getNormedXY (float angle) {
        float width = 0f;
        float height = 0f;
        if(!parabolaRangeOn){
            width = Mathf.Cos(Mathf.Deg2Rad * angle) * parabolaWidth/2f + 0.5f;
            height = Mathf.Sin(Mathf.Deg2Rad * angle) * parabolaHeight;
        }else{
            if(Globals.inputManager.control == InputManagerScript.ControlMethod.CARTESIAN){
                //Older control scheme that maps x to angle, y to radius.
                float yval = Globals.inputManager.inputNormY - 0.5f;
                yval = Mathf.Clamp(yval, -0.5f, 0.5f);
                aboveTransitionThreshold = (yval > 0) ? true : false;
                orbitalLerp += aboveTransitionThreshold ? 0.05f : -0.05f;
                orbitalLerp = Mathf.Clamp(orbitalLerp, 0f, 1f);
                float interpolatedParabolaWidth = Mathf.Lerp( innerParabolaWidth, parabolaWidth, orbitalLerp);
                float interpolatedParabolaHeight = Mathf.Lerp( innerParabolaHeight, parabolaHeight, orbitalLerp);
                width = Mathf.Cos(Mathf.Deg2Rad * angle) * interpolatedParabolaWidth/2f + 0.5f;
                height = Mathf.Sin(Mathf.Deg2Rad * angle) * interpolatedParabolaHeight;
            }else{
                //Newer control scheme that takes angle directly into account.
                if(!freeMode){
                    float dist = Vector2.Distance(
                        Globals.inputManager.inputPos,
                        Globals.mainTreePos);
                    aboveTransitionThreshold = (dist > Utils.distanceScale * 0.8f) ? true : false;
                    orbitalLerp += aboveTransitionThreshold ? 0.05f : -0.05f;
                    orbitalLerp = Mathf.Clamp(orbitalLerp, 0f, 1f);
                    float interpolatedParabolaWidth = Mathf.Lerp( innerParabolaWidth, parabolaWidth, orbitalLerp);
                    float interpolatedParabolaHeight = Mathf.Lerp( innerParabolaHeight, parabolaHeight, orbitalLerp);
                    width = Mathf.Cos(Mathf.Deg2Rad * angle) * interpolatedParabolaWidth/2f + 0.5f;
                    height = Mathf.Sin(Mathf.Deg2Rad * angle) * interpolatedParabolaHeight;
                }else{
                    /*
                    float sizeRatio = Utils.halfScreenHeight / Globals.INITIAL_HEIGHT;
                    float dist = Vector2.Distance(
                        Globals.inputManager.inputPos,
                        Globals.mainTreePos);
                    */
                    /*
                    width = Mathf.Cos(Mathf.Deg2Rad * angle) * dist/Utils.halfScreenHeight /2f + 0.5f;
                    height = Mathf.Sin(Mathf.Deg2Rad * angle) * dist;*/
                    
                    float dist = Vector2.Distance(
                        Globals.inputManager.inputPos,
                        Globals.mainTreePos);
                    Vector2 dims = Globals.inputManager.inputPos - Globals.mainTreePos;

                    width = Globals.inputManager.inputNormX;
                    height = Globals.inputManager.inputNormY;

                    if(angle > 3f){
                        width = width + 0.04f;
                        height = height - 0.05f;
                    }
                    else if(angle < -3f){
                        width = width - 0.04f;
                        height = height - 0.05f;
                    }
                    /*
                    float tempAng = Utils.PointAngle(Globals.mainTreePos, Globals.inputManager.inputPos);
                    width = Mathf.Cos(Mathf.Deg2Rad * (angle + tempAng)) * Mathf.Abs(dims.x / Utils.halfScreenWidth / 2) + 0.5f;
                    height = Mathf.Sin(Mathf.Deg2Rad * (angle + tempAng)) * dims.y / Utils.halfScreenHeight / 2;
                    */
                }
            }
        }
        //Debug.Log("width" + width + "height" + height);
        return new Vector2(width, height);
    }

    void OnDestroy(){
        foreach(_Mono l in leaves) {
            if(l != null)
                Destroy(l.gameObject);
        }
    }
}
