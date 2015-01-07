using UnityEngine;
using System.Collections;

public class CenterLeafScript : _Mono {
	
    public _Mono goldenLeaf;

	// Update is called once per frame
	void Update () {
	
	}

    public void MakeSpecialEffect() {
        //Debug.LogWarning("creating golden leaf");
        _Mono fx = Object.Instantiate(goldenLeaf, gameObject.transform.position, Quaternion.Euler(transform.eulerAngles)) as _Mono;
        fx.xs = xs * 1.2f;
        fx.ys = ys * 1.2f;
    }

}
