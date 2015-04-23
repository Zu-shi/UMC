using UnityEngine;
using System.Collections;

public class HealLeafScript : _Mono {
	
	// Update is called once per frame
	void Update () {
        xy = Globals.inputManager.normToScreenPoint(Globals.stateManager.leafLifex, Globals.stateManager.leafLifey);
	}
}
