using UnityEngine;
using System.Collections;

public class GlobalManagerScript : MonoBehaviour {

    public GameObject treeScenePrefab;
    public bool startWithTreeScene = true;

	// Use this for initialization
	void Start () {
        Globals.SaveTree(treeScenePrefab);
        if(startWithTreeScene){
            Globals.RestartTreeScene();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
