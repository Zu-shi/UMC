using UnityEngine;
using System.Collections;

public class CollisionTestScript : _Mono {

    float speed = 500f;

    void Update () {
            float step = speed * Time.deltaTime;
            Vector3 cameraPos = Camera.main.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f) , step);
            z = 0f;
    }
    
    void OnCollision2D(Collision2D other) {
        Debug.LogError("Trigger entered");
    }

}
