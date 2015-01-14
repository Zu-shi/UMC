using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptParent : _Mono {
    
    public int totalBugs;
    public float angleRange;
    public float speed;
    protected float initialCameraSize = 140f;
    public float arrivalTime;
    public float totalDuration;
    public GameObject bugPrefab;
    public _Mono blinkerPrefab;
    public Sprite blinkerSprite;
    public float damage{get; set;}

    protected float bugTimer;
    protected int bugCounter = 1;
    protected float lifetime;
    protected float aimedAngle;
    protected float initialAngle;
    protected bool madeBlinker = false;

    // Update is called once per frame
    public virtual void Update () {
        lifetime += Time.deltaTime;
        bugTimer += Time.deltaTime;
        if(arrivalTime - lifetime < 1.2f && !madeBlinker){
            MakeBlinker();
            madeBlinker = true;
            Debug.Log("Blinker");
        }
    }

    public void MakeBug (Globals.HazardColors color) {
        if(bugCounter > totalBugs){
            if(madeBlinker)
                DestroyObject(gameObject);
            else
                bugTimer -= totalDuration/totalBugs;
            return;
        }

        x = ( Globals.target.x + Mathf.Cos( Mathf.Deg2Rad * (90f + aimedAngle)) * (Utils.distanceScale + speed * Camera.main.orthographicSize * arrivalTime));
        y = ( Globals.target.y + Mathf.Sin( Mathf.Deg2Rad * (90f + aimedAngle)) * (Utils.distanceScale + speed * Camera.main.orthographicSize * arrivalTime));
        GameObject go = Instantiate(bugPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        StreamBugHazardScript sbh = go.GetComponent<StreamBugHazardScript>();
        sbh.color = color;
        sbh.speed= speed;
        sbh.angle = aimedAngle - 180f;
        
        Vector2 scale = go.GetComponent<_Mono>().xys;
        go.GetComponent<_Mono>().xys = Camera.main.orthographicSize / initialCameraSize * scale;
        
        bugTimer -= totalDuration/totalBugs;
        sbh.streamMember = bugCounter;
        sbh.streamTotal = totalBugs;
        sbh.damage = damage;
        bugCounter++;
    }

    public void MakeBlinker(){
        _Mono blinker = Utils.InstanceCreate(AngleToBlinkerLocation(initialAngle), blinkerPrefab);
        blinker.spriteRenderer.sprite = blinkerSprite;
        blinker.angle = initialAngle + 180f;
    }

    public Vector2 AngleToBlinkerLocation(float theta){
        return Utils.polarToCart(theta, 1.07f);
    }
}
