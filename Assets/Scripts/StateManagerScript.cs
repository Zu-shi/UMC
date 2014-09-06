using UnityEngine;
using System.Collections;

public class StateManagerScript : MonoBehaviour {

	public int currentStage { get; set; }
	public const float secondsPerCutscene = 6f;

	private bool inCutscene = true;
	private int totalSeconds;
	private int secondsForFirtstPart = 3;
	private int secondsForSecondPart = 60;
	private float secondTracker; //Keeps track of amount of time passed since last second.
	private GUITimerManagerScript guiTimerManager;
	private TreeManagerScript treeManager;
	private CameraManagerScript cameraManager;
	//private ShieldScript shieldManager;
	private HazardManagerScript hazardManager;

	public void Start(){
        currentStage = Globals.STAGE_STARTING;
		guiTimerManager = Globals.guiTimerManager;
		treeManager = Globals.treeManager;
		cameraManager = Globals.cameraManager;
		//shieldManager = Globals.shieldManager;
		hazardManager = Globals.hazardManager;

		totalSeconds = secondsForFirtstPart;
		guiTimerManager.SetTotalSeconds (totalSeconds);

		StartGame ();
	}

	public void Update(){
		UpdateTime ();
	}

	private void UpdateTime(){
		secondTracker += Time.deltaTime;
		if(secondTracker >= 1.0f){
			if(totalSeconds > 0){
				totalSeconds -= 1;
			}
			secondTracker = 0.0f;
		}

		if (totalSeconds == 0 && !inCutscene) {
			StartCutscene();
		}

		guiTimerManager.SetTotalSeconds (totalSeconds);
	}

	private void StartGame() {
        float time = secondsPerCutscene / 2;
        cameraManager.StartGame (time);
        treeManager.StartGame (time);
        totalSeconds = (int)Mathf.Ceil(time);
        Invoke("EndCutscene", time);
	}

	private void StartCutscene(){
		if (inCutscene) {
			Debug.LogWarning( "Call to starCutscene when cutscene is still active." );
		}
		inCutscene = true;
        
        guiTimerManager.StartCutscene ();
        cameraManager.StartCutscene (secondsPerCutscene);
        hazardManager.StartCutscene ();
        treeManager.StartCutscene(secondsPerCutscene);
        Invoke("EndCutscene", secondsPerCutscene + 0.1f);
	}
	
	private void EndCutscene(){
		if (!inCutscene) {
			Debug.LogWarning( "Call to endCutscene when cutscene is still active." );
		}
		inCutscene = false;

		guiTimerManager.EndCutscene ();
		cameraManager.EndCutscene ();
		hazardManager.EndCutscene ();
        treeManager.EndCutscene();

		currentStage += 1;
		switch (currentStage) {
		case Globals.STAGE_ONE: {totalSeconds = secondsForFirtstPart; break;}
		case Globals.STAGE_TWO: {totalSeconds = secondsForSecondPart; break;}
		}

	}
}
