using UnityEngine;
using System.Collections;

//Red Hazards do extra damage
public class StreamBugHazardScript : Hazard {
    
    public float damage;
    public Sprite redSprite;
    public Sprite blueSprite;
    public Sprite yellowSprite;
    public Sprite purple1Sprite;
    public Sprite purple2Sprite;

    public Sprite sprite{
        get{return spriteRenderer.sprite;}
        set{spriteRenderer.sprite = value;}
    }
    private Vector3 center;

    // Use this for initialization
    public override void  Start () {
        hasStarted = true;
        isStopped = false;
        hasFinished = false;
        isHarmful = true;
        isBlockable = true;
        center = transform.position;
        switch (color){
            case(Globals.HazardColors.RED):{sprite = redSprite; break;}
            case(Globals.HazardColors.BLUE):{sprite = blueSprite; break;}
            case(Globals.HazardColors.YELLOW):{sprite = yellowSprite; break;}
            case(Globals.HazardColors.PURPLE1):{sprite = purple1Sprite; break;}
            case(Globals.HazardColors.PURPLE2):{sprite = purple2Sprite; break;}
        }
        //spriteRenderer.color = new Color(0.5f, 0.8f, 0.5f, 1f);
    }
  
}