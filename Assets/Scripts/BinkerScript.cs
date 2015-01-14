using UnityEngine;
using System.Collections;

public class BinkerScript : _Mono {

    private Vector2 startingXys;

	// Use this for initialization
    void Start () {
        startingXys = xys;
	}
	
	// Update is called once per frame
    void Update () {
        float sizeRatio = Utils.halfScreenHeight / Globals.INITIAL_HEIGHT;
        xys = sizeRatio * startingXys;
	}
}
