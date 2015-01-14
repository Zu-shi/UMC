using UnityEngine;
using System.Collections;

public class ParticleScalingScript : MonoBehaviour {

    float startingSize;
    Vector3 startingVelocity;
    ParticleSystem ps;
    Particle p;

	// Use this for initialization
	void Start () {
        ps = gameObject.particleSystem;

        float sizeRatio = Utils.halfScreenHeight / Globals.INITIAL_HEIGHT;
        ps.startSpeed = sizeRatio * ps.startSpeed;
        ps.startSize = sizeRatio * ps.startSize;
	}
	
}
