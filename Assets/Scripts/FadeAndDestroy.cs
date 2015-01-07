using UnityEngine;
using System.Collections;

public class FadeAndDestroy : _Mono {

    public float rate = 0.05f;
    public float duration = 0f;
    //If duration is nonzero, rate is ignored and duration is used instead.

	// Update is called once per frame
	public void Update () {
        if(duration == 0f){
            alpha -= rate;
        }else{
            alpha -= Time.deltaTime / duration;
        }

        if(alpha <= 0){
            GameObject.Destroy(gameObject);
        }
	}
}
