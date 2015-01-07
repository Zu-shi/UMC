using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SunlightManagerScript : MonoBehaviour {

    public GameObject rayPrefab;
    private int numRays = 8;
    private float range = 0.15f;
    private List<_Mono> rays;

	// Use this for initialization
    void Start () {
        rays = new List<_Mono>();
        for(int i = 0; i < numRays; i++){
            Vector2 point = Globals.inputManager.normToScreenPoint( 0.5f + i * range / (numRays - 1) - range/2, 1f);
            GameObject ray = Object.Instantiate(rayPrefab, new Vector3(point.x, point.y, 0f), Quaternion.identity) as GameObject;
            SunlightScript rayMono = ray.GetComponent<SunlightScript>();
            rayMono.normedX = 0.5f + i * range / (numRays - 1) - range/2;
            rays.Add(rayMono);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
