using UnityEngine;
using System.Collections;

public class InvariantScaleScript : _Mono {
    
    private float originalXs, originalYs;
    
    public virtual void Start(){
        originalXs = xs;
        originalYs = ys;
    }
    
    public virtual void Update(){        
        float sizeRatio = Utils.halfScreenHeight / Globals.INITIAL_HEIGHT;
        xs = sizeRatio * originalXs;
        ys = sizeRatio * originalYs;
	}
}
