using UnityEngine;
using System.Collections;

public class ComboCounterScript : _Mono {
    
    public AudioClip notComboSound;
    public AudioClip comboSound;
    public AudioClip rewardAppearsSound;
    
    public Hazard[] bugs{get; set;}
    public RewardScript rewardObj;
    public bool startedTracking = false;
    private int currentIndex = 0;
    private int comboCount = 0; // -1 means combo has been stopped;
    
    // Update is called once per frame
    void Update () {
        /*
        bool destroyed = false;
        if(startedTracking){
            Debug.Log(bugs.Length);
            while( (bugs[currentIndex - 1] == null || bugs[currentIndex - 1].Equals(null))
                  && currentIndex != bugs.Length + 1){
                if(currentIndex < bugs.Length){currentIndex += 1;}
                else{
                    Destroy(gameObject); 
                    currentIndex = bugs.Length + 1;
                    Debug.Log("CCS destroyed");
                }
            }
            
            if(!destroyed){
        */
        if(startedTracking){
            _Mono bug = bugs[currentIndex];
            xy = bug.xy;
            xys = bug.xys * (comboCount == -1 ? 0f : 2.4f);
            angle = bug.angle;
            alpha = bug.alpha * 0.5f;
        }
        /*    }
        }*/
    }

    /*
    public void SetComboCount(int value){
        comboCount = value;
    }*/

    public void StopCombo(){
        comboCount = -1;
    }

    public void SeekNext(){
        currentIndex = 0;
        while(currentIndex < bugs.Length){
            if(!Utils.IsBugDestroyed(bugs[currentIndex])){
                break;
            }
            currentIndex++;
        }

        Debug.Log("Seeknext set currentIndex to " + currentIndex);
        if(currentIndex == bugs.Length) Destroy();
    }

    private void Destroy(){
        Destroy(gameObject);
        Debug.Log("CCS destroyed");
    }

    public bool ProcessCombo (int id, Globals.HazardColors color, CenterLeafScript cls){
        //Debug.Log("id: " + id);
        
        bool result = false;
        if(cls != null){
            if (id == comboCount + 1 )
            { 
                comboCount = id; 
                result = true;
                cls.MakeSpecialEffect();
            }
            else {comboCount = -1;}
            
            if( comboCount == bugs.Length ){
                CreateReward(color);
                comboCount = -1;
            }
        }
        
        if(result){Globals.stateManager.audioSource.PlayOneShot(comboSound); return true;}
        else{Globals.stateManager.audioSource.PlayOneShot(notComboSound); return false;}
    }
    
    void CreateReward(Globals.HazardColors color){
        RewardScript rewardObjTemp;
        Globals.stateManager.audioSource.PlayOneShot(rewardAppearsSound);
        rewardObjTemp = Utils.InstanceCreate<RewardScript>(Utils.OUT_OF_SCREEN, rewardObj);
        rewardObjTemp.color = color;
    }
}