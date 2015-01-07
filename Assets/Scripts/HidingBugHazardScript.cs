using UnityEngine;
using System.Collections;

public class HidingBugHazardScript : StreamBugHazardScript {

    private float startHidingDistance = 170f;
    private float endHidingDistance = 90f;

    public override void Update () {
        base.Update();

        float dist = Utils.PointDistance(xy, Globals.treeManager.treePos);
        float midDist = (startHidingDistance + endHidingDistance) / 2;
        if(dist < startHidingDistance && dist > endHidingDistance){
            if(dist > midDist){
                alpha = Mathf.Lerp(1, -1, (startHidingDistance - dist) / (startHidingDistance - midDist) );
            }else{
                alpha = Mathf.Lerp(1, 0, (dist - endHidingDistance) / (midDist - endHidingDistance) );
            }
            isBlockable = false;
        }else{
            isBlockable = true;
        }
	}
}
