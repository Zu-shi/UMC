using UnityEngine;
using System.Collections;

public class StateManagerScript : MonoBehaviour {

	public int currentStage { get; set; }

	private int totalSeconds;
	private int secondsForFirtstPart = 20;
	private int secondsForSecondPart = 40;
	private float secondTracker; //Keeps track of amount of time passed since last second.
	private GUITimerManagerScript guiTimerManager;
	private TreeManagerScript treeManager;
	private CameraManagerScript cameraManager;
	private ShieldScript shieldManager;
	private HazardManagerScript hazardManager;

	public void Start(){
		guiTimerManager = Globals.guiTimerManager;
		treeManager = Globals.treeManager;
		cameraManager = Globals.cameraManager;
		shieldManager = Globals.shieldManager;
		hazardManager = Globals.hazardManager;

		totalSeconds = secondsForFirtstPart;
		guiTimerManager.SetTotalSeconds (totalSeconds);
	}

	public void Update(){
		UpdateTime ();
	}

	private void UpdateTime(){
		secondTracker += Time.deltaTime;
		if(secondTracker >= 1.0f){
			if(totalSeconds > 1){
				totalSeconds -= 1;
			}
			secondTracker = 0.0f;
		}
		
		guiTimerManager.SetTotalSeconds (totalSeconds);
	}

	private void StartCutscene(){

	}
	
	private void EndCutscene(){
		
	}
}
