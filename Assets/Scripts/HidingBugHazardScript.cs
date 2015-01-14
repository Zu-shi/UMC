using UnityEngine;
using System.Collections;

public class HidingBugHazardScript : StreamBugHazardScript {

    private float startHidingDistance = 225f;
    private float endHidingDistance = 150f;
    private float startingStartHidingDistance;
    private float startingEndHidingDistance;

    public override void Start () {
        base.Start();
        startingStartHidingDistance = startHidingDistance;
        startingEndHidingDistance = endHidingDistance;
        isBlockableByOuterShield = false;
    }

    public override void Update () {
        float sizeRatio = Utils.halfScreenHeight / Globals.INITIAL_HEIGHT;
        startHidingDistance = sizeRatio * startingStartHidingDistance;
        endHidingDistance = sizeRatio * startingEndHidingDistance;

        base.Update();

        float dist = Utils.PointDistance(xy, Globals.treeManager.treePos);
        float midDist = (startHidingDistance + endHidingDistance) / 2;
        if(dist < startHidingDistance && dist > endHidingDistance){
            if(dist > midDist){
                alpha = Mathf.Lerp(1, -1, (startHidingDistance - dist) / (startHidingDistance - midDist) );
            }else{
                alpha = Mathf.Lerp(1, 0, (dist - endHidingDistance) / (midDist - endHidingDistance) );
            }
        }
	}
}
