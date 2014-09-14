using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameOverGUIScript : _Mono{

    private float targetAlpha = 0.4f;
    private GUIText guiText;
    private _Mono guiTextMono;

	// Use this for initialization
	void Start () {
        guiAlpha = 0;
        guiText = transform.GetChild(0).GetComponent<GUIText>();
	}

    void ShowGuiText() {
    }

	// Update is called once per frame
    public void Show () {
        int h = Mathf.FloorToInt( Globals.treeManager.mainTree.totalHeight );
        guiText.text = "Your tree grew to be " + h + " inches tall! \n Score: " + 
            h + "\n Press Enter to Restart.";

        guiTextMono = guiText.gameObject.AddComponent<_Mono>();
        guiTextMono.guiTextAlpha = 0f;
        Sequence sq = DOTween.Sequence();
        DOTween.To(()=> guiAlpha, x=> guiAlpha = x, targetAlpha, 1f);
        DOTween.To(()=> guiTextMono.guiTextAlpha, x=> guiTextMono.guiTextAlpha = x, 1f, 1f);
        //sq.AppendCallback(ShowGuiText);
	}
}
