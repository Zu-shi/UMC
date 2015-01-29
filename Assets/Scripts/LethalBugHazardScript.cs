using UnityEngine;
using System.Collections;

public class LethalBugHazardScript : StreamBugHazardScript {

    public override void Update () {
        base.Update();
        damage = 1f;
	}
}
