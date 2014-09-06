using UnityEngine;
using System.Collections;

public static class Globals {

	public static bool fixedHeightMode = false;
	private static InputManagerScript _inputManager;
	public const int STAGE_STARTING = 0;
	public const int STAGE_ONE = 1;
	public const int STAGE_TWO = 2;

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

	private static HazardManagerScript _hazardManager;
	public static HazardManagerScript hazardManager {
		get{
			if(_hazardManager == null)
				_hazardManager = GameObject.Find("HazardManager").GetComponent<HazardManagerScript>();
			return _hazardManager;
		}
	}
}