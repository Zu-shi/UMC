using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptLethal : StreamBugGeneratorScriptParent {
    
    public float sinOscillateAmpRange;
    public float sinOscillateSpeedPerSecond;
    public float totalAngleChangeRangeMin;
    public float totalAngleChangeRangeMax;
    protected float totalAngleChange;
    protected float sinOscillateAmp;
    protected float sinOscillateSpeed;
    
    public override void Start () {
        initialAngle = Random.Range(-angleRange, angleRange);
        totalAngleChange = Random.Range(totalAngleChangeRangeMin, totalAngleChangeRangeMax);
        totalAngleChange = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? totalAngleChange : -totalAngleChange;
        sinOscillateAmp = Random.Range(0f, sinOscillateAmpRange);
        sinOscillateSpeed = Random.Range(0f, sinOscillateSpeedPerSecond) / 2f / Mathf.PI;
        float lt = totalDuration/totalBugs * (bugCounter - 1);
        aimedAngle =  initialAngle + totalAngleChange * lt / totalDuration + sinOscillateAmp * Mathf.Sin(sinOscillateSpeed * lt);
        base.Start();
    }
    
    protected override void AngleChange() {
        bugCounter++;
        float lt = totalDuration/totalBugs * (bugCounter - 1);
        aimedAngle =  initialAngle + totalAngleChange * lt / totalDuration + sinOscillateAmp * Mathf.Sin(sinOscillateSpeed * lt);
    }

    public override void Update() {
        damage = 1;
    }
}
