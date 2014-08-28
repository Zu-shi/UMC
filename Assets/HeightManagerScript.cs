using UnityEngine;
using System.Collections;

public class HeightManagerScript : MonoBehaviour {

	public float maxHeight = 540f;
	public float height = 300f;
	public float minimumCameraSize = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(height < maxHeight){
			height += 5f;
		}

		float actualHeight = Mathf.Max (minimumCameraSize, height);
		Camera.main.orthographicSize = actualHeight;
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 
		                                             actualHeight,
		                                             Camera.main.transform.position.z);
	}
}
