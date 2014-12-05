using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptBlue : StreamBugGeneratorScriptParent {

    public float angleRange;
    public float speed;
    public float totalLifetime;
    public float bugsPerSecond;
    public GameObject bugPrefab;
    
    protected float angleSeperation;
    protected float initialAngle;
    protected float angleTracker;
    
    //Counters
    protected float lifetime;
    protected float bugTimer;
    protected int bugCounter;
    protected float time = 2f;
    
    // Use this for initialization
    public virtual void Start () {
        //Invoke("Initialize", 0.05f);
        Vector3 cameraPos = Camera.main.transform.position;
        y = cameraPos.y - Camera.main.orthographicSize / 4;
        x = Globals.treeManager.treePos.x;
        
        angleTracker = initialAngle = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? angleRange : -angleRange;
        angleSeperation = -initialAngle * 2f / (Mathf.Floor(bugsPerSecond * totalLifetime) + 1);
        
        x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + initialAngle)) * speed * Camera.main.orthographicSize * time);
        y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + initialAngle)) * speed * Camera.main.orthographicSize * time);
    }
    
    // Update is called once per frame
    public virtual void Update () {
        lifetime += Time.deltaTime;
        bugTimer += Time.deltaTime;
        
        if(bugTimer >= 0.9f/bugsPerSecond){
            float ang = angleTracker;

            Vector3 cameraPos = Camera.main.transform.position;
            y = cameraPos.y - Camera.main.orthographicSize / 4f;
            x = Globals.treeManager.treePos.x;
            x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * time);
            y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * time);
            GameObject go = Instantiate(bugPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            go.GetComponent<StreamBugHazard>().speed = speed;
            
            Vector2 scale = go.GetComponent<_Mono>().xys;
            go.GetComponent<_Mono>().xys = Camera.main.orthographicSize / initialCameraSize * scale;
            
            bugTimer = 0f;
            angleTracker += angleSeperation;
        }
        
        if(lifetime > totalLifetime){
            DestroyObject(gameObject);
        }
    }
}
