using UnityEngine;
using System.Collections;

public class HeavyWindHazardScript : BlueHazard {

    _Mono chair;

    public override void Start () {
        base.Start();
        chair = transform.GetChild(0).gameObject.AddComponent<_Mono>();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        chair.angle = chair.angle + 2f;
        chair.alpha = alpha;
    }

}