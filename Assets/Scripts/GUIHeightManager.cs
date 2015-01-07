using UnityEngine;
using System.Collections;

public class GUIHeightManager : MonoBehaviour {

	public int totalSeconds;
	public int secondsToFlash;
	public float flashesPerSecond;
	public Color nonFlashingColor = Color.white;
	public Color flashingColor = Color.red;
	private bool flashing = false;
	private GUIText guiText;

	// Use this for initialization
	void Start () {
		guiText = gameObject.GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
        guiText.text = "Score: " + Mathf.Round(Globals.stateManager.gameDuration * 10);
    }
}
