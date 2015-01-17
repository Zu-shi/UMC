using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptRound : StreamBugGeneratorScriptParent {

    protected float angleSeperation;
    protected float angleTracker;
    
    // Use this for initialization
    public virtual void Start () {
        angleTracker = initialAngle = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? angleRange : -angleRange;
        angleSeperation = -initialAngle * 2f / (totalBugs + 1);
    }
    
    // Update is called once per frame
    public override void Update () {
        base.Update();
        
        if(bugTimer >= totalDuration/totalBugs){
            aimedAngle = angleTracker;
            angleTracker += angleSeperation;
            MakeBug(Globals.HazardColors.BLUE);
        }
    }
      
}
