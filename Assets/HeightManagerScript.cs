using UnityEngine;
using System.Collections;

public class HeightManagerScript : MonoBehaviour {

	private float maxHeight = 480f;
	public float height { get; set;}
	private float minimumCameraSize = 250;

	// Use this for initialization
	void Start () {
		height = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(height < maxHeight){
			height += 0.5f;
		}

		float actualHeight = Mathf.Max (minimumCameraSize, height);
		Camera.main.orthographicSize = actualHeight;
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 
		                                             actualHeight,
		                                             Camera.main.transform.position.z);
	}
}
