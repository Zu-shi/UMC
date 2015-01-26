using UnityEngine;
using System.Collections;

public class ComboManagerScript : _Mono {

    int comboCount = 0;
    public int totalComboCount = 0;
    private GUIText guiTextInstance;
    private float flashyTextTimer;

    public Color flashyColor;
    public RewardScript rewardObj;
    public AudioClip notComboSound;
    public AudioClip comboSound;
    public AudioClip rewardAppearsSound;

	public int comboTally = 0;

    void Start () {
        guiTextInstance = gameObject.GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
    void Update () {
        if(totalComboCount!=0){
            guiText.text = "Combo: " + comboCount + "/" + totalComboCount;
        }

        if(flashyTextTimer >= 0f)
            flashyTextTimer -= Time.deltaTime;

        guiTextInstance.color = flashyTextTimer > 0 ? flashyColor : Color.white; 
        guiTextInstance.fontSize = flashyTextTimer > 0 ? (int)(30 + flashyTextTimer * 10 + (float)(comboCount) / totalComboCount * 10 ) : 30;
        guiTextInstance.fontStyle = flashyTextTimer > 0 ? FontStyle.Bold : FontStyle.Normal;
        
        guiTextInstance.text = "";
	}

    public bool ProcessCombo (int id, int total, Globals.HazardColors color, CenterLeafScript cls){
        //Debug.Log("id: " + id);

        bool result = false;
        if(cls != null){
            if( (id == comboCount + 1 && totalComboCount == total) || id == 1)
            { 
                comboCount = id; 
                result = true;
                cls.MakeSpecialEffect();
                flashyTextTimer = 0.3f;
            }
            else {comboCount = 0;}
            totalComboCount = total;

            if( comboCount == total ){
                CreateReward(color);
                comboCount = 0;
            }
        }

        if(result){Globals.stateManager.audioSource.PlayOneShot(comboSound); return true;}
        else{Globals.stateManager.audioSource.PlayOneShot(notComboSound); return false;}
    }

    void CreateReward(Globals.HazardColors color){
		comboTally++;
		Debug.Log ("comboTally is now " + comboTally);

        RewardScript rewardObjTemp;
        Globals.stateManager.audioSource.PlayOneShot(rewardAppearsSound);
        rewardObjTemp = Utils.InstanceCreate<RewardScript>(Vector2.zero, rewardObj);
        rewardObjTemp.color = color;
    }
}
