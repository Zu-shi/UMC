using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Globals {

	public static bool fixedHeightMode = false;
	private static InputManagerScript _inputManager;
	public const int STAGE_STARTING = 0;
	public const int STAGE_ONE = 1;
    public const int STAGE_TWO = 2;
    public const int STAGE_THREE = 3;
    public const float INITIAL_HEIGHT = 140;
    public static GameObject treePrefab;
    public static List<TreeModel> mainTrees = new List<TreeModel>();

	public static InputManagerScript inputManager {
		get{
			if(_inputManager == null)
				_inputManager = GameObject.Find("InputManager").GetComponent<InputManagerScript>();
			return _inputManager;
		}
	}
	
    private static StateManagerScript _stateManager;
    public static StateManagerScript stateManager {
        get{
            if(_stateManager == null)
                _stateManager = GameObject.Find("StateManager").GetComponent<StateManagerScript>();
            return _stateManager;
        }
    }

	private static TreeManagerScript _treeManager;
	public static TreeManagerScript treeManager {
		get{
			if(_treeManager == null)
				_treeManager = GameObject.Find("TreeManager").GetComponent<TreeManagerScript>();
			return _treeManager;
		}
	}

	private static GUITimerManagerScript _guiTimerManager;
	public static GUITimerManagerScript guiTimerManager {
		get{
			if(_guiTimerManager == null)
				_guiTimerManager = GameObject.Find("GUITimerManager").GetComponent<GUITimerManagerScript>();
			return _guiTimerManager;
		}
	}

	private static CameraManagerScript _cameraManager;
	public static CameraManagerScript cameraManager {
		get{
			if(_cameraManager == null)
				_cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManagerScript>();
			return _cameraManager;
		}
	}

	private static ShieldScript _shieldManager;
	public static ShieldScript shieldManager {
		get{
			if(_shieldManager == null)
				_shieldManager = GameObject.Find("ShieldManager").GetComponent<ShieldScript>();
			return _shieldManager;
		}
	}

    private static GameOverGUIScript _gameOverGUIScript;
    public static GameOverGUIScript gameOverGUIScript {
        get{
            if(_gameOverGUIScript == null)
                _gameOverGUIScript = GameObject.Find("GameOverGUI").GetComponent<GameOverGUIScript>();
            return _gameOverGUIScript;
        }
    }

	private static HazardManagerScript _hazardManager;
	public static HazardManagerScript hazardManager {
		get{
			if(_hazardManager == null)
				_hazardManager = GameObject.Find("HazardManager").GetComponent<HazardManagerScript>();
			return _hazardManager;
		}
	}

    private static TreeGrowth _treeGrowthManager;
    public static TreeGrowth treeGrowthManager {
        get{
            if(_treeGrowthManager == null)
                _treeGrowthManager = GameObject.Find("TreeManager").GetComponent<TreeGrowth>();
            return _treeGrowthManager;
        }
    }

    public static void SaveTree (GameObject tree) {
        treePrefab = tree;
    }

    public static void RestartTreeScene(bool keepTrees = true){
        GameObject treeScene = GameObject.Find("TreeScene");
        if(treeScene != null){
            mainTrees.Add(treeManager.mainTree);
            foreach(TreeModel t in mainTrees){
                t.setAlphaRecursive(t.alpha * 0.3f);
            }
            
            Object.DestroyImmediate(treeScene, true);
            Debug.Log("treeScene destoryed ");
        }
        
        GameObject treeSceneNew = GameObject.Instantiate(treePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        Debug.Log("treeSceneNew made? " + treeSceneNew.name);
        treeSceneNew.name = "TreeScene";

        _hazardManager = GameObject.Find("HazardManager").GetComponent<HazardManagerScript>();
        _shieldManager = GameObject.Find("ShieldManager").GetComponent<ShieldScript>();
        _cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManagerScript>();
        _guiTimerManager = GameObject.Find("GUITimerManager").GetComponent<GUITimerManagerScript>();
        _stateManager = GameObject.Find("StateManager").GetComponent<StateManagerScript>();
        _treeManager = GameObject.Find("TreeManager").GetComponent<TreeManagerScript>();
        _gameOverGUIScript = GameObject.Find("GameOverGUI").GetComponent<GameOverGUIScript>();

        //_treeManager.mainTree.x = -100f + Random.Range(0f, 200f);
        _treeManager.treePos = new Vector2(-100f + Random.Range(0f, 200f), 0);
    }
}