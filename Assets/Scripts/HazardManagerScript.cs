using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HazardManagerScript : MonoBehaviour {
    
    private bool isGameOver = false;
	private bool inCutscene = true;
	private bool test = false;
	private float timeElapsed = 0f;
    private float timeSinceLastHazard = 0f;
    private float secondsPerHazard;
    private Tween secondsPerHazardTween;

	public Dictionary<HazardEnum, GameObject> hazardDictionary = new Dictionary<HazardEnum, GameObject>();
	private List< HazardEntry > hazardEntries = new List< HazardEntry >();

    public GameObject bugPrefab;
    public GameObject healPrefab;

    public GameObject redBugGenPrefab;
    public GameObject yellowBugGenPrefab;
    public GameObject blueBugGenPrefab;

    private GameObject[] firstStageHazardsPrefab = new GameObject[1];
    private GameObject[] secondStageHazardsPrefab = new GameObject[2];
    private GameObject[] thirdStageHazardsPrefab = new GameObject[3];

	// Use this for initialization
	void Start () {
        firstStageHazardsPrefab[0] = redBugGenPrefab;
        secondStageHazardsPrefab[0] = redBugGenPrefab;
        thirdStageHazardsPrefab[0] = redBugGenPrefab;
        
        secondStageHazardsPrefab[1] = yellowBugGenPrefab;
        thirdStageHazardsPrefab[1] = yellowBugGenPrefab;
        
        thirdStageHazardsPrefab[2] = blueBugGenPrefab;

        float initialSecondsPerHazard = 2.3f;
        float finalSecondsPerHazard = 1.8f;
        secondsPerHazard = initialSecondsPerHazard;
        float totalTimeToWeen = 120f; //In Game Seconds
        secondsPerHazardTween = DOTween.To(() => secondsPerHazard, x => secondsPerHazard = x, finalSecondsPerHazard, totalTimeToWeen);

        AddHazard(HazardEnum.BUG, timeElapsed + 0.1f);
        secondsPerHazardTween.Pause();
	}
	
	// Update is called once per frame
	void Update () {
		if (!inCutscene && !isGameOver){
			HazardEntry toCall = null;
			timeElapsed += Time.deltaTime;
			foreach(HazardEntry he in hazardEntries){
				if(he.time < timeElapsed){
					toCall = he;
					break;
				}
			}

			if (toCall != null) {

                if( Mathf.Floor(Random.Range(0f, 8f)) == 0 ){
                    GameObject gen = Instantiate(healPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                }

				hazardEntries.Remove(toCall);
                switch(Globals.stateManager.currentStage){
                    case(Globals.STAGE_ONE):{
                        AddHazard(HazardEnum.BUG, timeElapsed + secondsPerHazard);
                        GameObject gen = Instantiate(Utils.RandomFromArray<GameObject>(firstStageHazardsPrefab), Vector3.zero, Quaternion.identity) as GameObject;
                        break;
                    }
                    case(Globals.STAGE_TWO):{
                        AddHazard(HazardEnum.BUG, timeElapsed + secondsPerHazard);
                        GameObject gen = Instantiate(Utils.RandomFromArray<GameObject>(secondStageHazardsPrefab), Vector3.zero, Quaternion.identity) as GameObject;
                        break;
                    }
                    case(Globals.STAGE_THREE):{
                        AddHazard(HazardEnum.BUG, timeElapsed + secondsPerHazard);
                        GameObject gen = Instantiate(Utils.RandomFromArray<GameObject>(thirdStageHazardsPrefab), Vector3.zero, Quaternion.identity) as GameObject;
                        break;
                    }
                }
			}

		}
	}

    public void GameOver(){
        isGameOver = true;
    }

	public void AddHazard (HazardEnum h, float t) {
		hazardEntries.Add ( new HazardEntry(h, t) );
	}
	
	public void CutsceneStart(){
		Debug.Log ("HazardManager.StartCutscene");
        inCutscene = true;
        secondsPerHazardTween.Pause();
	}
	
	public void CutsceneEnd(){
		Debug.Log ("HazardManager.EndCutscene");
        inCutscene = false;
        secondsPerHazardTween.Play();
	}
}


class HazardEntry{
	public HazardEnum Hazard;
	public float time;
	
	public HazardEntry(HazardEnum h, float t){
		time = t;
		Hazard = h;
	}
}
