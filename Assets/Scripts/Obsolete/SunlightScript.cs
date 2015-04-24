using UnityEngine;
using System.Collections;

public class SunlightScript : _Mono {
    
    private float amplitude = 0.2f;
    private float period = 4f;
    private float location;
    private float counter;
    private float sprHeight;
    private float originalXs;
    private float originalYs;
    private float originalAlpha;
    public float normedX;
    
    // Use this for initialization
    void Start () {
        originalAlpha = 0.3f;
        counter = 0f;
        originalXs = xs;
        originalYs = ys;
        counter = Random.Range(0f, 2 * Mathf.PI);
        amplitude = Random.Range(0.2f, 0.25f);
    }
    
    // Update is called once per frame
    void Update () {
        xs = Camera.main.orthographicSize / 480f * originalXs;
        ys = Camera.main.orthographicSize / 480f * originalYs;
        xy = InputManagerScript.normToWorldPoint(normedX, 1);

        alpha = amplitude * Mathf.Sin (counter) + originalAlpha;
        counter += Time.deltaTime * Mathf.PI * 2 / period;


        GetComponent<BoxCollider2D>().offset = new Vector2(0, -spriteRenderer.sprite.rect.size.y/2);
        GetComponent<BoxCollider2D>().size =  
            new Vector2(spriteRenderer.sprite.rect.size.x,
                        spriteRenderer.sprite.rect.size.y);

        int mask = 1 << LayerMask.NameToLayer("ShieldLeaf");
        //Debug.Log(LayerMask.NameToLayer("ShieldLeaf"));
        RaycastHit2D leafHit = Physics2D.Raycast(xy, new Vector2(0f, -1000f), Mathf.Infinity, mask);
        //Debug.Log(xy);
        if(leafHit.collider != null){
            //float distance = ( leafHit.point - (Vector2) transform.position ).magnitude;
            //ys = distance / 100f;
            Debug.Log("Collided " + leafHit.transform.gameObject.name);
        }

        //Debug.DrawRay(new Vector3(xy.x, xy.y, 0), new Vector3(0f, -1000f, 0f));
    }

    
    void OnTriggerLeave(Collider other) {
        /*
        if (other.gameObject.tag == "Shield")
        {
            _Mono o = other.gameObject.GetComponent<_Mono>();
            ys = originalYs;
        }
        */
    }

}
