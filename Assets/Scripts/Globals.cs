using UnityEngine;
using System.Collections;

public class Globals {

	private static InputManagerScript _inputManager;

	public static InputManagerScript inputManager {
		get{
			if(_inputManager == null)
				_inputManager = GameObject.Find("InputManager").GetComponent<InputManagerScript>();
			return _inputManager;
		}
	}
	
	private static HeightManagerScript _heightManager;
	
	public static HeightManagerScript heightManager {
		get{
			if(_heightManager == null)
				_heightManager = GameObject.Find("HeightManager").GetComponent<HeightManagerScript>();
			return _heightManager;
		}
	}
}