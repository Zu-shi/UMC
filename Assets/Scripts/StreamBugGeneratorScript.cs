using UnityEngine;
using System.Collections;

public class StreamBugGeneratorScript : StreamBugGeneratorScriptParent {

    public float angleRange;
    public float speed;
    public float totalLifetime;
    public float bugsPerSecond;
    public float sinOscillateAmpRange;
    public float sinOscillateSpeedPerSecond;
    public float totalAngleChangeRangeMin;
    public float totalAngleChangeRangeMax;
    public GameObject bugPrefab;

    protected float totalAngleChange;
    protected float sinOscillateAmp;
    protected float sinOscillateSpeed;
    protected float initialAngle;

    //Counters
    protected float lifetime;
    protected float bugTimer;
    protected float time = 2f;

	// Use this for initialization
    public virtual void Start () {
        //Invoke("Initialize", 0.05f);
        Vector3 cameraPos = Camera.main.transform.position;
        y = cameraPos.y - Camera.main.orthographicSize / 4;
        x = Globals.treeManager.treePos.x;

        initialAngle = Random.Range(-angleRange, angleRange);
        totalAngleChange = Random.Range(totalAngleChangeRangeMin, totalAngleChangeRangeMax);
        totalAngleChange = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? totalAngleChange : -totalAngleChange;
        sinOscillateAmp = Random.Range(0f, sinOscillateAmpRange);
        sinOscillateSpeed = Random.Range(0f, sinOscillateSpeedPerSecond) / 2f / Mathf.PI;

        x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + initialAngle)) * speed * Camera.main.orthographicSize * time);
        y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + initialAngle)) * speed * Camera.main.orthographicSize * time);
	}
	
	// Update is called once per frame
	public virtual void Update () {
        lifetime += Time.deltaTime;
        bugTimer += Time.deltaTime;
        float ang =  initialAngle + totalAngleChange * lifetime / totalLifetime + sinOscillateAmp * Mathf.Sin(sinOscillateSpeed * lifetime);
        
        if(bugTimer >= 0.9f/bugsPerSecond){
            Vector3 cameraPos = Camera.main.transform.position;
            y = cameraPos.y - Camera.main.orthographicSize / 4;
            x = Globals.treeManager.treePos.x;
            x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * time);
            y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * time);
            GameObject go = Instantiate(bugPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            go.GetComponent<StreamBugHazard>().speed = speed;

            Vector2 scale = go.GetComponent<_Mono>().xys;
            go.GetComponent<_Mono>().xys = Camera.main.orthographicSize / initialCameraSize * scale;

            bugTimer = 0f;
        }

        if(lifetime > totalLifetime){
           DestroyObject(gameObject);
        }
	}
}
