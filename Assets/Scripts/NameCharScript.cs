using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NameCharScript : _Mono {
    public string letter;

    void Start (){ 
        DOTween.To(() => guiAlpha, x => guiAlpha = x, 1f, 0.7f); 
    }

    void Update(){
        GetComponent<GUIText>().text = letter;
    }
}
