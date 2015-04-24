using UnityEngine;
using System.Collections;

public class HealLeafScript : _Mono {
	
	// Update is called once per frame
	void Update () {
        xy = InputManagerScript.normToWorldPoint(Globals.stateManager.leafLifex, Globals.stateManager.leafLifey);
	}
}
