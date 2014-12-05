using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScriptYellow : StreamBugGeneratorScriptParent {
    
    public int bugsPerSwitch;
    public float angleRange;
    public float speed;
    public float totalLifetime;
    public float bugsPerSecond;
    public float angleSeperationRangeMin;
    public float angleSeperationRangeMax;
    public GameObject bugPrefab;
    
    protected float angleSeperation;
    protected float initialAngle;
    
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
        
        initialAngle = Random.Range(-angleRange, angleRange);
        angleSeperation = Random.Range(angleSeperationRangeMin, angleSeperationRangeMax);
        angleSeperation = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? angleSeperation : -angleSeperation;
        
        x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + initialAngle)) * speed * Camera.main.orthographicSize * time);
        y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + initialAngle)) * speed * Camera.main.orthographicSize * time);
    }
    
    // Update is called once per frame
    public virtual void Update () {
        lifetime += Time.deltaTime;
        bugTimer += Time.deltaTime;
        
        if(bugTimer >= 0.9f/bugsPerSecond){
            float ang =  initialAngle + angleSeperation / 2f;
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

            bugCounter++;
            if(bugCounter >= bugsPerSwitch){
                bugCounter = 0;
                angleSeperation *= -1;
            }
        }
        
        if(lifetime > totalLifetime){
            DestroyObject(gameObject);
        }
    }
}
