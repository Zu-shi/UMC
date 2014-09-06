using UnityEngine;
using System.Collections;

public class ShieldScript : _Mono {

	public _Mono shield;

	public float parabolaWidth;
	public float parabolaHeight;

	private float height;
    private float width;
    private Camera camera;


	// Use this for initialization
	void Start () {
        camera = Camera.main;
		height = camera.orthographicSize;
		width = height * camera.aspect;
	}
	
	// Update is called once per frame
	void Update () {
		height = camera.orthographicSize;
		width = height * camera.aspect;
        shield.xs = height / 240f;
        shield.ys = height / 240f;

		float xPos = inputX;
		float yPos;

		if(xPos < (-1 * parabolaWidth * width)){
			yPos = GetYCoord(-1 * parabolaWidth * width);
			xPos = -1 * parabolaWidth * width;
		}
		else if (xPos > parabolaWidth * width) {
			yPos = GetYCoord (parabolaWidth * width);
			xPos = parabolaWidth * width;
		}
		else {
			yPos = GetYCoord(xPos);	
		}
		shield.transform.position = new Vector3 (xPos, yPos, shield.transform.position.z);

        //Debug.Log("x = " + xPos + " y = " + yPos);
	}

	float GetYCoord(float xCoord){
		return GetCoeff() * Mathf.Pow (xCoord, 2f) + (parabolaHeight * height) + camera.transform.position.y;
	}

	float GetCoeff(){
        return -((height + parabolaHeight * height) / (Mathf.Pow (parabolaWidth * width, 2f)));
	}
	
}
