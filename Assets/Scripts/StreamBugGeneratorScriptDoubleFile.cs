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
    public override void Start () {
		totalBugs = VariablesManager.DoubleFileTotalBugs;
		arrivalTime = VariablesManager.DoubleFileArrivalTime;
		speed = VariablesManager.DoubleFileSpeed;
		totalDuration = VariablesManager.DoubleFileDuration;
		initialAngle = Random.Range(-VariablesManager.DoubleFileAngleRange, VariablesManager.DoubleFileAngleRange);
        angleSeperation = Random.Range(angleSeperationRangeMin, angleSeperationRangeMax);
        angleSeperation = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? angleSeperation : -angleSeperation;
        centerAngle = initialAngle;
        initialAngle = initialAngle + angleSeperation / 2f;
        aimedAngle =  centerAngle + angleSeperation / 2f;
        bugSwitchCounter++;
        base.Start();

    }

    protected override void AngleChange() {
        bugCounter++;
        aimedAngle =  centerAngle + angleSeperation / 2f;
        bugSwitchCounter++;
        if(bugSwitchCounter >= bugsPerSwitch){
            bugSwitchCounter = 0;
            angleSeperation *= -1;
        }
    }
}
