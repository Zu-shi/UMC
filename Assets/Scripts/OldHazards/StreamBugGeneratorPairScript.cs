using UnityEngine;
using System.Collections;

public class StreamBugGeneratorPairScript {
    /*
    public float angleSeperationMangnitude = 6f;
    protected float angleSeperation;
    
    public override void Start () {
        base.Start();
        angleSeperation = Mathf.FloorToInt(Random.Range(0f, 2f)) == 1 ? angleSeperationMangnitude : -angleSeperationMangnitude;
    }

    // Update is called once per frame
    public override void Update () {
        lifetime += Time.deltaTime;
        bugTimer += Time.deltaTime;
        float ang =  initialAngle + totalAngleChange * lifetime / totalLifetime + sinOscillateAmp * Mathf.Sin(sinOscillateSpeed * lifetime);
        
        if(bugTimer >= 1f/bugsPerSecond){
            Vector3 cameraPos = Camera.main.transform.position;
            y = cameraPos.y - Camera.main.orthographicSize / 4;
            x = Globals.treeManager.treePos.x;
            x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * arrivalTime);
            y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * arrivalTime);
            GameObject go = Instantiate(bugPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            go.GetComponent<StreamBugHazard>().speed = speed;
            
            Vector2 scale = go.GetComponent<_Mono>().xys;
            go.GetComponent<_Mono>().xys = Camera.main.orthographicSize / initialCameraSize * scale;
            
            bugTimer = 0f;

            ang += angleSeperationMangnitude;
            y = cameraPos.y - Camera.main.orthographicSize / 4;
            x = Globals.treeManager.treePos.x;
            x = ( x + Mathf.Cos( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * arrivalTime);
            y = ( y + Mathf.Sin( Mathf.Deg2Rad * (90f + ang)) * speed * Camera.main.orthographicSize * arrivalTime);
            GameObject go2 = Instantiate(bugPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            go2.GetComponent<StreamBugHazard>().speed = speed;
            go2.GetComponent<_Mono>().xys = Camera.main.orthographicSize / initialCameraSize * scale;
        }
        
        if(lifetime > totalLifetime){
            DestroyObject(gameObject);
        }
    }
    */
}
