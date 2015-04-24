using UnityEngine;
using System.Collections;
using DG.Tweening;

public class KeyboardKeyScript : _Mono {

    //public string key;
    public string message;
    public Vector2 keyLocation;
    _Mono guiText;

    void Start (){ 
        guiText = transform.GetChild(0).gameObject.GetComponent<_Mono>();
        DOTween.To(() => alpha, x => alpha = x, 1f, 0.7f); 
        DOTween.To(() => guiText.guiAlpha, x => guiText.guiAlpha = x, 1f, 0.7f); 
    }

	// Update is called once per frame
    public void SetKey (string m) {
        guiText = transform.GetChild(0).gameObject.GetComponent<_Mono>();
        message = m;
        guiText.GetComponent<GUIText>().text = message;
        if(message.Length > 1){
            guiText.GetComponent<GUIText>().fontSize = Mathf.RoundToInt(guiText.GetComponent<GUIText>().fontSize / 1.3f);
        }
        guiText.xy = new Vector2(keyLocation.x, keyLocation.y);
	}

    public void AlertVirtualKeyboardManager () {
        Globals.virtualKeyboardManager.ButtonPressed(message);
    }
}
