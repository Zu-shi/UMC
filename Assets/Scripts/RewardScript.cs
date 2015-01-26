using UnityEngine;
using System.Collections;

public class RewardScript : _Mono {

    public float damage;
    public bool harmful = true;
    public Sprite redSprite;
    public Sprite blueSprite;
    public Sprite yellowSprite;
    public Sprite purple1Sprite;
    public Sprite purple2Sprite;
    private float lifeTime = 0f;
    //private float totalLifeTime = 3f;
    public AudioClip rewardSound;

    private float bigRadius = 0.85f;
    private float smallRadius = 18f;
    //private Vector2 startingXys;
    //private float startingBigRadius;
    private float startingSmallRadius;
    private float bigAngle;
    private float smallAngle = 0f;
    private float direction;

    //private bool fadeinout = true;
    private float fadeinEndTime = 0.5f;
    private float fadeoutStartTime = 6f;
    private float fadeoutEndTime = 7f;
    private bool claimed = false;

    public Globals.HazardColors color;
    //Set color of particles (White for now?)

    public Sprite sprite{
        get{return spriteRenderer.sprite;}
        set{spriteRenderer.sprite = value;}
    }

    //private Vector3 center;
    
    // Use this for initialization
    public void  Start () {
        //startingBigRadius = bigRadius;
        startingSmallRadius = smallRadius;

        //center = transform.position;
        direction = Utils.RandomSign();
        switch (color){
            case(Globals.HazardColors.RED):{sprite = redSprite; break;}
            case(Globals.HazardColors.BLUE):{sprite = blueSprite; break;}
            case(Globals.HazardColors.YELLOW):{sprite = yellowSprite; break;}
            case(Globals.HazardColors.PURPLE1):{sprite = purple1Sprite; break;}
            case(Globals.HazardColors.PURPLE2):{sprite = purple2Sprite; break;}
        }

        bigAngle = Random.Range(-60f, 60f);
        //spriteRenderer.color = new Color(0.5f, 0.8f, 0.5f, 1f);
        xys = Camera.main.orthographicSize / Globals.INITIAL_HEIGHT * xys * 1.5f;
    }

    public void Update () {
        float sizeRatio = Utils.halfScreenHeight / Globals.INITIAL_HEIGHT;
        //bigRadius = startingBigRadius * sizeRatio;
        smallRadius = startingSmallRadius * sizeRatio;
        //xys = startingXys * sizeRatio;

        fadein();
        fadeout();
        smallAngle += 0.05f * direction;
        bigAngle += 0.06f * direction;
        lifeTime += Time.deltaTime;
        Vector2 bigCircleXY = Utils.polarToCart(bigAngle, bigRadius);
        xy = bigCircleXY + new Vector2(Mathf.Cos(smallAngle) * smallRadius, Mathf.Sin(smallAngle) * smallRadius);
        angle = smallAngle * Mathf.Rad2Deg;
        if (direction == -1){angle = angle - 180f;}
    }

    void fadein () {
        if(lifeTime <= fadeinEndTime){
            alpha = Mathf.Lerp(0, 1, lifeTime/fadeinEndTime);
        }
    }

    void fadeout () {
        if(lifeTime >= fadeoutStartTime){
            alpha = Mathf.Lerp(1, 0, (lifeTime - fadeoutStartTime)/(fadeoutEndTime - fadeoutStartTime));
        }

        if(lifeTime >= fadeoutEndTime){
            Object.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Tree" || other.gameObject.tag == "Shield")
        {
            if(!claimed){
                Globals.stateManager.audioSource.PlayOneShot(rewardSound);
                claimed = true;
                lifeTime = fadeoutStartTime;
                Globals.treeGrowthManager.ClaimReward();
                Globals.treeManager.mainTree.AddToColorPool(color);

				LoggingManager.reward();
            }
            
        }
    }
}
