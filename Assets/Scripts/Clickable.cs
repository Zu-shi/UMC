using UnityEngine;
using System.Collections;

//This class is a container returned by raycast on clickable objects. Objects of this type has a collision box
//Without rotation, and has a name that is used for logic checking.
public class Clickable : _Mono {

    //Note: layer of this object must be "Clickable"
    public string name;
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
