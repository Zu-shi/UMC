using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/* HazardManager version 2
 * A second attempt at creating a more versitile hazard manager.
 * This hazardManager manages the difficulty of the game by adjusting exactly
 * the amount of break the player has between hazards by looking lookForwardSeconds
 * seconds ahead of the current time, and scheduling a new event to attack the player
 * that many seconds from now.
 * 
 * This is done by utilizing arrivalTime and totalDuration variables in the
 * bug generators. totalDuration is the attacking period of the hazard, while
 * arrivalTime is the time it takes for the hazard to make contact. 
 * 
 * A new end mark is set lookForwardSeconds in the future, and each passing of the 
 * end mark triggers a schedule event.
 * 
 * --NOTE: NOT IMPLEMENTED YET--
 * Note: the first lookForwardSeconds of schedule is automatically set at the 
 * beginning to kick-start the cycle. arrivalTime is manupulated to shorten wait time.
 * -----------------------------
 * 
 * HELPFUL GRAPHICS:
 *         .-----------------------arrivalTime-------------------------.
 *         |                                                           |
 * ---...--|------------------------------|-------peaceDuration--------|----------totalDuration---------|
 *   3. new hazard                  1. old end mark              2. new attack                   4. new end mark
 *     scheduled                        passed                  time calculated                         set
 */
public class HazardManagerScript : MonoBehaviour {

    public StreamBugGeneratorScriptParent[] generators;

    private bool isGameOver = false;
	private bool inCutscene = true;
	private bool test = false;

	private float timeElapsed = 0f;
    private float peaceDuration;
    private Tween peaceDurationTween;
    private float endMark;
    private const float timePerNewHazard = 7f;
    public const int lookForwardSeconds = 10;
	private List< HazardEntry > hazardEntries = new List< HazardEntry >();
    private float damage;
    private Tween damageTween;

	// Use this for initialization
	void Start () {

        float initialPeaceDuration = 1.2f;
        float finalPeaceDuration = 0.1f;
        peaceDuration = initialPeaceDuration;
        float totalTimeToWeenPeaceDurtion = 80f; //In Game Seconds
        peaceDurationTween = DOTween.To(() => peaceDuration, x => peaceDuration = x, finalPeaceDuration, totalTimeToWeenPeaceDurtion);

        
        float initialDamage = 0.02f;
        float finalDamage = 0.08f;
        damage = initialDamage;
        float totalTimeToWeenDamage = 120f; //In Game Seconds
        peaceDurationTween = DOTween.To(() => damage, x => damage = x, finalDamage, initialDamage);
        //peaceDurationTween.Pause();

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
                hazardEntries.Remove(toCall);
                GameObject gen = Instantiate(generators[toCall.hazard], Vector3.zero, Quaternion.identity) as GameObject;
                gen.GetComponent<StreamBugGeneratorScriptParent>().damage = damage;
			}

            if (timeElapsed + lookForwardSeconds >= endMark) {
                //Takes care of generating different types of hazards
                int maxHazardLevel = (int)Mathf.Clamp(Mathf.Ceil(timeElapsed / timePerNewHazard), 0, generators.Length);
                int hazard = Utils.RandomToInt(maxHazardLevel);
                //steps 2, 3, 4
                AddHazard(hazard, endMark + peaceDuration - generators[hazard].arrivalTime);
                endMark += peaceDuration + generators[hazard].totalDuration;
            }
		}
	}

    public void GameOver(){
        isGameOver = true;
    }

	public void AddHazard (int h, float t) {
		hazardEntries.Add ( new HazardEntry(h, t) );
	}
	
	public void CutsceneStart(){
		Debug.Log ("HazardManager.StartCutscene");
        inCutscene = true;
        peaceDurationTween.Pause();
	}
	
	public void CutsceneEnd(){
		Debug.Log ("HazardManager.EndCutscene");
        inCutscene = false;
        peaceDurationTween.Play();
	}
}


class HazardEntry{
	public int hazard;
	public float time;
	
    public HazardEntry(int h, float t){
		time = t;
		hazard = h;
	}
}
