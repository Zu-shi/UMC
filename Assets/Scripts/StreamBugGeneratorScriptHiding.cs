using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptHiding : StreamBugGeneratorScriptParent {

    public float sinOscillateAmpRange;
    public float sinOscillateSpeedPerSecond;
    public float totalAngleChangeRangeMin;
    public float totalAngleChangeRangeMax;
    protected float totalAngleChange;
    protected float sinOscillateAmp;
    protected float sinOscillateSpeed;
    
    // Use this for initialization
    public virtual void Start () {
        initialAngle = Random.Range(-angleRange, angleRange);
        totalAngleChange = Random.Range(totalAngleChangeRangeMin, totalAngleChangeRangeMax);
        totalAngleChange = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? totalAngleChange : -totalAngleChange;
        sinOscillateAmp = Random.Range(0f, sinOscillateAmpRange);
        sinOscillateSpeed = Random.Range(0f, sinOscillateSpeedPerSecond) / 2f / Mathf.PI;
    }
    
    // Update is called once per frame
    public virtual void Update () {
        base.Update();
        if(bugTimer >= totalDuration/totalBugs){
            aimedAngle =  initialAngle + totalAngleChange * lifetime / totalDuration + sinOscillateAmp * Mathf.Sin(sinOscillateSpeed * lifetime);
            MakeBug(Globals.HazardColors.PURPLE1);
        }
    }
}
