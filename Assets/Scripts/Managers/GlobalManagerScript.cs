using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GlobalManagerScript : MonoBehaviour {

    public enum Scene{
        IDLE, TREE
    }

    public GameObject idleScenePrefab;
    public GameObject treeScenePrefab;
    public bool startWithIdleScene = true;
    public Scene currentScene{get; set;}
    float transitionTime = 1f;
    float IDLE_SCREEN_HALF_HEIGHT = 600f;
    float TREE_SCREEN_HALF_HEIGHT = 35f;

	// Use this for initialization
	void Start () {
        if(startWithIdleScene){
            currentScene = Scene.IDLE;
            InitiateIdleScene();
        }else{
            currentScene = Scene.TREE;
            InitiateTreeScene(0f, 0f, false);
        }
	}
	
    public void InitiateIdleScene(){
        currentScene = Scene.IDLE;
        _Mono cmono = Camera.main.GetComponent<_Mono>();
        
        //cmono.y = 600f;
        //Camera.main.orthographicSize = 600f;
        DOTween.To(()=>cmono.x, x=> cmono.x =x, 0, transitionTime);
        DOTween.To(()=>cmono.y, x=> cmono.y =x, IDLE_SCREEN_HALF_HEIGHT, transitionTime);
        DOTween.To(()=>Camera.main.orthographicSize, x=> Camera.main.orthographicSize =x, IDLE_SCREEN_HALF_HEIGHT, transitionTime);

        GameObject[] tcss = GameObject.FindGameObjectsWithTag("Container");
        foreach(GameObject go in tcss){
            TreeContainerScript tcs = go.GetComponent<TreeContainerScript>();
            tcs.setAlpha(1f);
        }
        /*
        foreach(TreeModel t2 in Globals.mainTrees){
            t2.setAlphaRecursive(1f);
        }*/
    }

    public void InitiateTreeScene(float posx, float posy, bool keepTrees){

        currentScene = Scene.TREE;
        _Mono cmono = Camera.main.GetComponent<_Mono>();
        DOTween.To(()=>cmono.y, x=> cmono.y =x, posy + TREE_SCREEN_HALF_HEIGHT, transitionTime);
        DOTween.To(()=>cmono.x, x=> cmono.x =x, posx, transitionTime);

        Sequence s = DOTween.Sequence();
        Tweener t = DOTween.To(()=>Camera.main.orthographicSize, x=> Camera.main.orthographicSize =x, TREE_SCREEN_HALF_HEIGHT, transitionTime);
        s.Append(t);
        s.AppendCallback(()=>InitiateTreeSceneCallBack(posx, posy, keepTrees));
        s.Play();

        GameObject[] tcss = GameObject.FindGameObjectsWithTag("Container");
        foreach(GameObject go in tcss){
            TreeContainerScript tcs = go.GetComponent<TreeContainerScript>();
            tcs.setAlpha(0f);
        }
        /*
        foreach(TreeModel t2 in Globals.mainTrees){
            t2.setAlphaRecursive(0.1f);
        }*/
    }

    void InitiateTreeSceneCallBack(float posx, float posy, bool keepTrees){
        Globals.SaveTree(treeScenePrefab);
        Globals.RestartTreeScene(posx, posy, keepTrees);
    }

	// Update is called once per frame
	void Update () {
//        Debug.Log("x: " + Camera.main.transform.position.x + ", y: " + Camera.main.transform.position.y + ", size: " + Camera.main.orthographicSize);

	}
}
