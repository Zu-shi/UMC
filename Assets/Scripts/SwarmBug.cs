using UnityEngine;
using System.Collections;

public class SwarmBug : MonoBehaviour {

	public float radius;
	public float rotationSpeed;
	
	private Vector3 center;

	// Use this for initialization
	void Start () {
		center = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float x = center.x + Mathf.Sin (Time.time * rotationSpeed) * radius;
		float y = center.y + Mathf.Cos (Time.time * rotationSpeed) * radius;
		transform.position = new Vector3 (x, y, transform.position.z);
	}
}
