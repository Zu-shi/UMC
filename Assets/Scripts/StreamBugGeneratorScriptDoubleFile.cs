using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptDoubleFile : StreamBugGeneratorScriptParent {
    
    private int bugsPerSwitch = 5;
    public float angleSeperationRangeMin;
    public float angleSeperationRangeMax;
    protected float angleSeperation;
    //Counters
    protected int bugSwitchCounter;
    protected float centerAngle;

    // Use this for initialization
    public virtual void Start () {
        initialAngle = Random.Range(-angleRange, angleRange);
        angleSeperation = Random.Range(angleSeperationRangeMin, angleSeperationRangeMax);
        angleSeperation = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? angleSeperation : -angleSeperation;
        centerAngle = initialAngle;
        initialAngle = initialAngle + angleSeperation / 2f;
    }
    
    // Update is called once per frame
    public override void Update () {
        base.Update();
        if(bugTimer >= totalDuration/totalBugs){
            aimedAngle =  centerAngle + angleSeperation / 2f;
            MakeBug(Globals.HazardColors.YELLOW);
            bugSwitchCounter++;
            if(bugSwitchCounter >= bugsPerSwitch){
                bugSwitchCounter = 0;
                angleSeperation *= -1;
            }
        }
    }
}
