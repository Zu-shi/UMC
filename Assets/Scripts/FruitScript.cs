using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FruitScript : _Mono {

    public Vector2 position;
    private float duration = 0.5f;

	// Use this for initialization
	void Start () {
        position = new Vector2( Random.Range(-6f, 6f), Random.Range(0.4f, 1f) );   

        float size = Random.Range(0.3f, 0.5f);

        DOTween.defaultEaseType = Ease.InOutBack;
        DOTween.To( ()=>xs, x=> xs = x, size, duration);
        DOTween.To( ()=>xs, x=> ys = x, size, duration);
        DOTween.defaultEaseType = Ease.OutQuad;

        //xs = size;
        //ys = size;

	}
	
	// Update is called once per frame
	void Update () {}
}
