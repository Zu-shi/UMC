using UnityEngine;
using System.Collections;

public class GUIHeightManager : MonoBehaviour {

	private GUIText guiTextInstance;

	// Use this for initialization
	void Start () {
		guiTextInstance = gameObject.GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
        guiTextInstance.text = "Score: " + Mathf.Round(Globals.stateManager.gameDuration * 10);

        guiTextInstance.text = "";
    }
}
