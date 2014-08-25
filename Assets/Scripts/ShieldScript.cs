using UnityEngine;
using System.Collections;

public class ShieldScript : _Mono {

	public Camera camera;

	public GameObject shield;

	public float parabolaWidth;
	public float parabolaHeight;

	private float height;
	private float width;


	// Use this for initialization
	void Start () {
		height = camera.orthographicSize;
		width = height * camera.aspect;
	}
	
	// Update is called once per frame
	void Update () {
		height = camera.orthographicSize;
		width = height * camera.aspect;

		float xPos = inputX;
		float yPos;

		if(xPos < (-1 * parabolaWidth)){
			yPos = GetYCoord(-1 * parabolaWidth);
			xPos = -1 * parabolaWidth;
		}
		else if (xPos > parabolaWidth) {
			yPos = GetYCoord (parabolaWidth);
			xPos = parabolaWidth;
		}
		else {
			yPos = GetYCoord(xPos);	
		}
		shield.transform.position = new Vector3 (xPos, yPos, shield.transform.position.z);
	}

	float GetYCoord(float xCoord){
		return GetCoeff() * Mathf.Pow (xCoord, 2f) + parabolaHeight + camera.transform.position.y;
	}

	float GetCoeff(){
		return -((height + parabolaHeight) / (Mathf.Pow (parabolaWidth, 2f)));
	}
	
}
