using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameOverGUIScript : _Mono{

    private float targetAlpha = 0.4f;
    private GUIText guiTextInstance;
    private _Mono guiTextMono;
    private BoxCollider box;
    //private int score = 0;

	// Use this for initialization
	void Start () {
        guiAlpha = 0;
        guiTextInstance = transform.GetChild(0).GetComponent<GUIText>();
        box = gameObject.GetComponent<BoxCollider>();
	}

    void ShowGuiText() {
    }

	// Update is called once per frame
    public void Show () {
        int h = Mathf.FloorToInt( Globals.treeManager.mainTree.recordHeight );
        Debug.Log(Globals.stateManager.currentStage);
        guiTextInstance.text = (Globals.stateManager.currentStage != Globals.STAGE_THREE) ?
        "Your tree grew to be " + h + " inches tall!" + "\n Click here to continue." :
                "Your tree grew to be " + h + " inches tall! \n You grew " + Globals.treeManager.mainTree.numFruits + " fruits!\n Score: " + 
                (h + Globals.treeManager.mainTree.numFruits * 5) + "\n Click here to continue." ;
        

        guiTextMono = guiTextInstance.gameObject.AddComponent<_Mono>();
        guiTextMono.guiTextAlpha = 0f;
        Sequence sq = DOTween.Sequence();
        DOTween.To(()=> guiAlpha, x=> guiAlpha = x, targetAlpha, 1f);
        DOTween.To(()=> guiTextMono.guiTextAlpha, x=> guiTextMono.guiTextAlpha = x, 1f, 1f);
        sq.AppendInterval(1f);
        sq.AppendCallback(AllowClicks);

        Vector3 cameraPos = Globals.cameraManager.camera.transform.position;
        Camera cam = Globals.cameraManager.camera;
        box.size = new Vector3(cam.orthographicSize * cam.aspect * 2f, cam.orthographicSize / 1.5f, 20f);
        box.transform.position = new Vector3(cameraPos.x, cameraPos.y, box.transform.position.z);
        guiTextMono.x = 0.5f;
        guiTextMono.y = 0.5f;

        //box.bounds.size = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);
	}

    void AllowClicks(){
        Debug.Log("Cursor enabled");
        Globals.cursorManager.allowClicks = true;
    }
}
