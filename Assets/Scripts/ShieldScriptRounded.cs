using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldScriptRounded : _Mono {

    public GameObject shieldLeafPrefab;
    public _Mono goldenShieldLeafPrefab;
    private int numLeaves = 7;
    private int angleSpread = 30;
    
    private float parabolaWidth = 0.6f;
    private float parabolaHeight = 0.6f;
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
            }
        }
	}

	// Update is called once per frame
	void Update () {
        swayAngle += swayFreq * Mathf.PI / (1f / Time.deltaTime);

        float centerAngle = (1 - Globals.inputManager.inputNormX) * 180f;

        int i = 0;

        foreach(_Mono l in leaves) {
            float angle = centerAngle - angleSpread/2f + i * angleSpread/(numLeaves - 1);

            //script for bouncing leaves near the end.
            if(angle > 180f){angle = 180f - (angle - 180f);}
            else if(angle < 0f){angle = -angle;}

            l.xy = Globals.inputManager.normToScreenPoint(getNormedXY(angle));

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
            width = Mathf.Cos(Mathf.Deg2Rad * angle) * ( parabolaWidth/2f + (Globals.inputManager.inputNormY - 0.5f) * 2 * parabolaWidthRange) + 0.5f;
            height = Mathf.Sin(Mathf.Deg2Rad * angle) * ( parabolaHeight  + (Globals.inputManager.inputNormY - 0.5f) * 2 * parabolaHeightRange);
        }
        //Debug.Log("width" + width + "height" + height);
        return new Vector2(width, height);
    }

    void OnDestroy(){
        //foreach(_Mono l in leaves) {
        //    Destroy(l.gameObject);
        //}
    }
}
