using UnityEngine;
using System.Collections;

public class VirtualKeyboardManager : MonoBehaviour {

    //Set all as child
    public GameObject plainGuitextPrefab;
    public GameObject plainGuitextChildPrefab;
    public Sprite buttonSprite;
    public int currentNameChar{get; set;}
    int[] rows = {10, 9, 8, 7};
    int maxNameChar = 3;
    KeyboardKeyScript[] keys;
    NameCharScript[] nameChars;
    string name = "";
    int maxNumber = 10;
    int numKeys = 30;
    float preferredWidth = 0.7f;
    float gapToButtonRatio = 0.2f;
    float buttonSize;
    float gap;
    float height;
    float heightEnd;
    float widthStart;
    float nameGap;
    float nameYValue;
    Camera cam;
    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ/. /";
    string BACKSPACE_MESSAGE = "Back";
    string ENTER_MESSAGE = "Done";
    bool showed = false;

	public void Show () {
        nameYValue = 0.7f;
        nameGap = 0.09f;
        cam = Camera.main;
        buttonSize = preferredWidth / (maxNumber + (maxNumber - 1) * gapToButtonRatio);
        gap = buttonSize * gapToButtonRatio;
        height = (buttonSize * rows.Length + (rows.Length - 1) * gap) * Camera.main.aspect;   
        widthStart = (1f - preferredWidth) / 2 + buttonSize / 2;
        heightEnd = 1f - ((1f - height) / 2) + buttonSize / 2 - 0.1f;
        keys = new KeyboardKeyScript[numKeys];
        nameChars = new NameCharScript[maxNameChar];
        for(int i = 0; i < numKeys; i++){ 
            GameObject item = GameObject.Instantiate(plainGuitextChildPrefab) as GameObject;
            KeyboardKeyScript kks = item.AddComponent<KeyboardKeyScript>();
            keys[i] = kks;
            kks.gameObject.transform.SetParent(this.gameObject.transform);
            kks.gameObject.AddComponent<SpriteRenderer>().sprite = buttonSprite;
            kks.spriteRenderer.sortingLayerName = "Foreground";
            kks.alpha = 0f;
        }
        for(int i = 0; i < maxNameChar; i++){
            GameObject item = GameObject.Instantiate(plainGuitextPrefab) as GameObject;
            NameCharScript ncs = item.AddComponent<NameCharScript>();
            ncs.xy = new Vector2(
                0.5f - nameGap + nameGap * i, nameYValue
            );
            nameChars[i] = ncs;
            ncs.gameObject.transform.SetParent(this.gameObject.transform);
            ncs.letter = "_";
        }

        PositionKeys();
        RenderKeys();
        SetLogicToKeys();
        Debug.Log("Cursor enabled");
        Globals.cursorManager.allowClicks = true;
        Globals.inputManager.smallScreenMode = false;
	}

    public void ButtonPressed(string message) {
        if(!message.Equals(BACKSPACE_MESSAGE) && !message.Equals(ENTER_MESSAGE)){
            if(name.Length < 3){
                name = name + message;
            }
        }else if(message.Equals(BACKSPACE_MESSAGE)){
            name = name.Substring(0, name.Length - 1);
        }else if(message.Equals(ENTER_MESSAGE)){
            Globals.treeManager.mainTree.transform.parent.name = name;
            Globals.treeManager.mainTree.transform.parent.GetComponent<TreeContainerScript>().creator = name;
            Globals.RestartIdleScene();
        }

        int index = 0;
        for(index = 0; index < name.Length; index++){
            nameChars[index].letter = name.Substring(index, 1);
        }
        for(; index < maxNameChar; index++){
            nameChars[index].letter = "_";
        }
    }

    void Update() {
        if(!showed){
            showed = true;
            //Show();
        }
    }

    void PositionKeys () {
        int position = 0;
        for(int i = 0; i < rows.Length; i++){
            for(int j = 0; j < rows[i]; j++){
                //Special case for the final row.
                if(i == rows.Length - 1 && j > 0){
                    if(j == 1){
                        //Space bar
                        keys[position + j].keyLocation = new Vector2(widthStart + (3) * (buttonSize + gap) + i * 0.5f * buttonSize, heightEnd - i * (buttonSize + gap) * Camera.main.aspect);
                        keys[position + j].xy = InputManagerScript.normToWorldPoint(
                            keys[position + j].keyLocation
                        );
                        keys[position + j].ys = (InputManagerScript.normToWorldScale(buttonSize, buttonSize) / buttonSprite.bounds.size.x).x;
                        keys[position + j].xs = (InputManagerScript.normToWorldScale(buttonSize * 5 + gap * 4, 1) / buttonSprite.bounds.size.x).x;
                    }else if (j == 2){
                        //Enter key
                        keys[position + j].keyLocation = new Vector2(widthStart + (6) * (buttonSize + gap) + i * 0.5f * buttonSize, heightEnd - i * (buttonSize + gap) * Camera.main.aspect);
                        keys[position + j].xy = InputManagerScript.normToWorldPoint(
                            keys[position + j].keyLocation
                        );
                        keys[position + j].xys = InputManagerScript.normToWorldWidthScale(buttonSize) / buttonSprite.bounds.size.x * Vector2.one;
                    }
                }else{
                    keys[position + j].keyLocation = new Vector2(widthStart + j * (buttonSize + gap) + i * 0.5f * buttonSize, heightEnd - i * (buttonSize + gap) * Camera.main.aspect);
                    keys[position + j].xy = InputManagerScript.normToWorldPoint(
                            keys[position + j].keyLocation
                    );
//                    Debug.Log(keys[position + j].keyLocation.x);
                    /*
                    Debug.Log("Linear difference " + (buttonSize + gap) * Camera.main.orthographicSize * Camera.main.aspect * 2);
                    Debug.Log("Calculated difference " + (InputManagerScript.normToWorldPoint(1 + buttonSize + gap, 1).x - InputManagerScript.normToWorldPoint(1, 1).x));
                    Debug.Log("Linear difference 2" + (buttonSize + gap) * Camera.main.orthographicSize * Camera.main.aspect * 2);
                    Debug.Log("Calculated difference 2" + (InputManagerScript.normToWorldPoint(1 + buttonSize + gap, 1).x - InputManagerScript.normToWorldPoint(1, 1).x));

                    Debug.Log("bboundsizex" + buttonSprite.bounds.size.x);
                    Debug.Log("bsize" + buttonSize);
                    Debug.Log("gap" + gap);
                    Debug.Log("Camera width " + Camera.main.orthographicSize * Camera.main.aspect * 2);
                    Debug.Log("Screen width " + Screen.width);
                    Debug.Log("Pixel width " + Camera.main.pixelWidth);
                    */
                    keys[position + j].xys = InputManagerScript.normToWorldWidthScale(buttonSize) / buttonSprite.bounds.size.x * Vector2.one;
                }
            }
            position += rows[i];
        }
    }

    void RenderKeys () {
        /*
        int index = 0;
        foreach(char c in letters){
            keys[index].key = c.ToString();
            index ++;
        }
        */
        //Need to render sprites on top of the obejcts
    }

    void SetLogicToKeys () {
        int index = 0;
        string[] otherMessages = {BACKSPACE_MESSAGE, ENTER_MESSAGE};
        int otherIndex = 0;
        foreach(char c in letters){
            if(c != '/'){
                keys[index].SetKey(c.ToString());
            }else{
                keys[index].SetKey(otherMessages[otherIndex]);
                otherIndex++;
            }
            index ++;
        }

        foreach(KeyboardKeyScript key in keys){
            BoxCollider box = key.gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(key.spriteRenderer.sprite.bounds.size.x, key.spriteRenderer.sprite.bounds.size.y, 30f); //Extremely hackish z value to get detected first
            Clickable click = key.gameObject.AddComponent<Clickable>();
            click.nickname = "VirtualKeyboard";
            //box.center = 
        }
    }

}
