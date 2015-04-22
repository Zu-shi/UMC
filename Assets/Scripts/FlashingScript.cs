using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FlashingScript : _Mono {

    public float alphaMultiplierMax = 1f;
    public float alphaMultiplierMin = 0.6f;
    public float alphaMultiplierPeriod = 1f;
    bool paused = false;
    Sequence s;
    float a;

	// Use this for initialization
	void Start () {
        a = alphaMultiplierMin;
        s = DOTween.Sequence();
        Tween t1 = DOTween.To(() => a, x => a = x, alphaMultiplierMax, alphaMultiplierPeriod/2);
        t1.SetEase(Ease.InOutSine);
        //Tween t2 = DOTween.To(() => a, x => a = x, alphaMultiplierMin, alphaMultiplierPeriod/2);
        //t2.SetEase(Ease.InOutSine);
        s.Append(t1);
        //s.Append(t2);
        s.SetLoops(10000000, LoopType.Yoyo);
        s.Play();
	}
	
	// Update is called once per frame
    void LateUpdate () {
        alpha = alpha * a;
	}

    public void Pause () {
        s.TogglePause();
        //t.TogglePause();
    }
}
