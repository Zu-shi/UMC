using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameOverGUIScript : _Mono{

    private float targetAlpha = 0.4f;
    private GUIText guiTextInstance;
    private _Mono guiTextMono;
    //private int score = 0;

	// Use this for initialization
	void Start () {
        guiAlpha = 0;
        guiTextInstance = transform.GetChild(0).GetComponent<GUIText>();
	}

    void ShowGuiText() {
    }

	// Update is called once per frame
    public void Show () {
        int h = Mathf.FloorToInt( Globals.treeManager.mainTree.totalHeight );
        Debug.Log(Globals.stateManager.currentStage);
        guiTextInstance.text = (Globals.stateManager.currentStage != Globals.STAGE_THREE) ?
        "Your tree grew to be " + h + " inches tall! \n Score: " + 
                h + "\n Press Enter to Restart." :
                "Your tree grew to be " + h + " inches tall! \n You grew " + Globals.treeManager.mainTree.numFruits + " fruits!\n Score: " + 
                (h + Globals.treeManager.mainTree.numFruits * 5) + "\n Press Enter to Restart." ;
        

        guiTextMono = guiTextInstance.gameObject.AddComponent<_Mono>();
        guiTextMono.guiTextAlpha = 0f;
        //Sequence sq = DOTween.Sequence();
        DOTween.To(()=> guiAlpha, x=> guiAlpha = x, targetAlpha, 1f);
        DOTween.To(()=> guiTextMono.guiTextAlpha, x=> guiTextMono.guiTextAlpha = x, 1f, 1f);
        //sq.AppendCallback(ShowGuiText);
	}
}
