using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HazardManagerScript : MonoBehaviour {

	private bool test = true;
	private float timeElapsed = 0f;
	public Dictionary<HazardEnum, GameObject> hazardDictionary = new Dictionary<HazardEnum, GameObject>();
	private List< HazardEntry > hazardEntries = new List< HazardEntry >();

	// Use this for initialization
	void Start () {
		if (test) {
			AddHazard(HazardEnum.BLIZZARD, 3f);
			AddHazard(HazardEnum.BLIZZARD, 4f);
			AddHazard(HazardEnum.BLIZZARD, 5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
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
			Debug.Log ("Hazard called");
		}
	}

	public void AddHazard (HazardEnum h, float t) {
		hazardEntries.Add ( new HazardEntry(h, t) );
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
