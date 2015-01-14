using UnityEngine;
using System.Collections;

public class CursorScript : _Mono {

    float originalXs = 0f;
    float originalYs = 0f;
    float flashesPerSecond = 1f;
    float alphaAngle = 0f;
    float xprev = 0f;
    float yprev = 0f;
    int switchCounter = 0;

	// Use this for initialization
	void Start () {
        Screen.showCursor = false;
        alpha = 0.7f;
        originalXs = xs;
        originalYs = ys;
	}
	
	// Update is called once per frame
	void Update () {

        alphaAngle += Mathf.PI / flashesPerSecond * Time.deltaTime;
        alpha = 0.55f + 0.2f * Mathf.Sin(alphaAngle);

        float sizeRatio = Utils.halfScreenHeight / Utils.resolutionWidth;
        xs = sizeRatio * originalXs;
        ys = sizeRatio * originalYs;

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
	}
}
