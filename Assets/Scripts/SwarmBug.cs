using UnityEngine;
using System.Collections;

public class SwarmBug : _Mono {

	public float radius;
	public float rotationSpeed;
	
	private Vector3 center;

	// Use this for initialization
	void Start () {
		center = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float x = transform.parent.position.x + Mathf.Sin (Time.time * rotationSpeed) * radius;
        float y = transform.parent.position.y + Mathf.Cos (Time.time * rotationSpeed) * radius;
        alpha = transform.parent.gameObject.GetComponent<_Mono>().alpha;
	}
}
