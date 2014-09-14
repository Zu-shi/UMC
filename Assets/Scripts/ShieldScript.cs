using UnityEngine;
using System.Collections;

public class ShieldScript : _Mono {

	public _Mono shield;

	public float parabolaWidth;
	public float parabolaHeight;

	private float height;
    private float width;
    private Camera mainCamera;
    private float originalXs;
    private float originalYs;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
		height = mainCamera.orthographicSize;
        width = height * mainCamera.aspect;
        //originalXs = shield.xs;
        //originalYs = shield.ys;
	}
	
	// Update is called once per frame
	void Update () {

        if(originalXs == 0f){
            originalXs = shield.xs;
            originalYs = shield.ys;
        }

        height = mainCamera.orthographicSize;
        width = height * mainCamera.aspect;
        shield.xs = originalXs * height / 240f;
        shield.ys = originalYs * height / 240f;
        //Debug.Log(originalXs + " " + height);

        float xPos = inputX;
		float yPos;
        float xOff = Globals.treeManager.treePos.x;

		if(xPos < (-1 * parabolaWidth * width + xOff)){
            yPos = GetYCoord(-1 * parabolaWidth * width);
            xPos = -1 * parabolaWidth * width + xOff;
		}
        else if (xPos > parabolaWidth * width + xOff) {
            yPos = GetYCoord (parabolaWidth * width);
            xPos = parabolaWidth * width + xOff;
		}
		else {
            yPos = GetYCoord(xPos - xOff);
		}
		shield.transform.position = new Vector3 (xPos, yPos, shield.transform.position.z);

		UpdateRotation (xPos - xOff);
		
	}

	void UpdateRotation(float xCoord){
		shield.angle = 60 * Mathf.Atan(getDerivative (xCoord));
	}

	float getDerivative(float xCoord){
		return (2 * GetCoeff ()) * xCoord;
	}

	float GetYCoord(float xCoord){
        return GetCoeff() * Mathf.Pow (xCoord, 2f) + (parabolaHeight * height) + mainCamera.transform.position.y;
	}

	float GetCoeff(){
        return -((height + parabolaHeight * height) / (Mathf.Pow (parabolaWidth * width, 2f)));
	}
	
}
