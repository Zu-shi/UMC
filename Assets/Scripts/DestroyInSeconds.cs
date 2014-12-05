using UnityEngine;
using System.Collections;

public class DestroyInSeconds : MonoBehaviour {

    public float duration;
    private float  timer = 0f;
	
	void Update () {
        timer += Time.deltaTime;
        if(timer >= duration){Destroy(this.gameObject);}
	}
}
