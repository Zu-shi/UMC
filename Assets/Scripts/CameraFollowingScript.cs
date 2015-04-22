using UnityEngine;
using System.Collections;

public class CameraFollowingScript : _Mono {

    public Camera followingCamera;
    private _Mono cameraMono;
    private Vector2 xyDiff;
    
    public void Start(){
        if(followingCamera != null){
            cameraMono = followingCamera.GetComponent<_Mono>();
            xyDiff = cameraMono.xy - xy;
        }
    }
    
    public void LateUpdate(){
        if(cameraMono != null){
            xy = cameraMono.xy - xyDiff;
        }
    }
}
