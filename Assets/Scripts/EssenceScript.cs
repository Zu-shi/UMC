using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EssenceScript : _Mono {

    public Color red;
    public Color blue;
    public Color yellow;
    public Color purple;
    public Color white;
    public Globals.HazardColors color;
    public static float rewardDelay = 1.5f;

	// Use this for initialization
    void Start () {
        Tween t, t2;
        t = transform.DOMove(Globals.mainTreePos + new Vector2(0, Camera.main.orthographicSize / 4f), rewardDelay);
        t.SetEase(Ease.InOutQuad);
        t2 = DOTween.To(()=> alpha, x=> alpha = x, 0, rewardDelay);
        t2.SetEase(Ease.InQuad);

        switch (color){
            case(Globals.HazardColors.RED):{spriteRenderer.color = red; break;}
            case(Globals.HazardColors.BLUE):{spriteRenderer.color = blue; break;}
            case(Globals.HazardColors.YELLOW):{spriteRenderer.color = yellow; break;}
            case(Globals.HazardColors.PURPLE1):{spriteRenderer.color = purple; break;}
            case(Globals.HazardColors.PURPLE2):{spriteRenderer.color = purple; break;}
            case(Globals.HazardColors.WHITE):{spriteRenderer.color = white; break;}
        }
	}
}
