using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptRound : StreamBugGeneratorScriptParent {

    protected float angleSeperation;
    protected float angleTracker;
    
    // Use this for initialization
    public override void Start () {
        angleTracker = initialAngle = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? angleRange : -angleRange;
        angleSeperation = -initialAngle * 2f / (totalBugs + 1);
        aimedAngle = angleTracker;
        angleTracker += angleSeperation;
        base.Start();
    }

    protected override void AngleChange(){
        bugCounter++;
        aimedAngle = angleTracker;
        angleTracker += angleSeperation;
    }
}
