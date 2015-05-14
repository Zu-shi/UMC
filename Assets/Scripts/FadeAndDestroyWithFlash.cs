using UnityEngine;
using System.Collections;

public class FadeAndDestroyWithFlash : _Mono {
    
    public float rate = 0.05f;
    public float duration = 0f;
    public float a = 1f;
    //If duration is nonzero, rate is ignored and duration is used instead.
    
    // Update is called once per frame
    public void Update () {
        if(duration == 0f){
            a -= rate;
            alpha = a;
        }else{
            a -= Time.deltaTime / duration;
            alpha = a;
        }
        
        if(alpha <= 0){
            GameObject.Destroy(gameObject);
        }
    }
}
