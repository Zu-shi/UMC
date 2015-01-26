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
    public ComboCounterScript ccsPrefab;
    public Globals.HazardColors color;
    private ComboCounterScript ccs;

    protected float bugTimer;
    protected int bugCounter = 1;
    protected float lifetime;
    protected float aimedAngle;
    protected float initialAngle;
    protected bool madeBlinker = false;
    
    public virtual void Start () {
        ccs = Utils.InstanceCreate(Utils.OUT_OF_SCREEN, ccsPrefab).GetComponent<ComboCounterScript>();
        Debug.Log(ccs);
        Debug.Log("bugcounter = " + bugCounter);
        Debug.Log("totalbugs = " + totalBugs);
        ccs.startedTracking = true;
        ccs.bugs = new Hazard[totalBugs];
        MakeBug(color);
    }
    
    // Update is called once per frame
    public virtual void Update () {
        lifetime += Time.deltaTime;
        bugTimer += Time.deltaTime;
        if(arrivalTime - lifetime < 1.2f && !madeBlinker){
            MakeBlinker();
            madeBlinker = true;
            Debug.Log("Blinker");
        }

        if(madeBlinker)
            DestroyObject(gameObject);
        else
            bugTimer -= totalDuration/totalBugs;
    }

    public void MakeBug (Globals.HazardColors c) {
        //Now all bugs are created in one fell swoop
        while(bugCounter <= totalBugs) {
            float ang = Mathf.Deg2Rad * (90f + aimedAngle);
            float dist = (Utils.distanceScale + speed * Camera.main.orthographicSize * (arrivalTime + totalDuration/totalBugs * (bugCounter - 1)) );
            x = Globals.target.x + Mathf.Cos(ang) * dist;
            y = Globals.target.y + Mathf.Sin(ang) * dist;
            GameObject go = Instantiate(bugPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            StreamBugHazardScript sbh = go.GetComponent<StreamBugHazardScript>();
            sbh.color = c;
            sbh.speed= speed;
            sbh.angle = aimedAngle - 180f;
            
            //Vector2 scale = go.GetComponent<_Mono>().xys;
            //go.GetComponent<_Mono>().xys = Camera.main.orthographicSize / initialCameraSize * scale;
            
            bugTimer -= totalDuration/totalBugs;
            sbh.streamMember = bugCounter;
            sbh.streamTotal = totalBugs;
            sbh.damage = damage;
            sbh.ccs = ccs;
            ccs.bugs[bugCounter - 1] = sbh;

            AngleChange();
        }
    }

    protected virtual void AngleChange(){
        //TO BE OVERRIDDEN
        Debug.Log("This shoudl not print");
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
