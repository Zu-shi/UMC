using UnityEngine;
using System.Collections;

public class FadeAndDestroy : _Mono {

    public float rate = 0.05f;
	
	// Update is called once per frame
	public void Update () {
        alpha -= rate;
        if(alpha <= 0){
            GameObject.Destroy(this.gameObject);
        }
	}
}
