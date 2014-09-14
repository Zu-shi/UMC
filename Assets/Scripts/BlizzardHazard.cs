using UnityEngine;
using System.Collections;

public class BlizzardHazard : BlueHazard {

	
	// Update is called once per frame
    public override void Update () {
        base.Update();
        angle = angle + 2f;
	}
}
