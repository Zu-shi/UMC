using UnityEngine;
using System.Collections;

public static class Utils {

	public static bool printAssertMessages = false;

	public static void Assert(bool statement, string context = ""){
		if(printAssertMessages && context != ""){
			Debug.Log(context);
		}

		if (!statement) {
			Debug.LogError("Assertion failed");
		}
	}	

	public static Vector2 GetPointFromAngleAndRadius(Vector2 xy, float angle, float rad){
		return new Vector2(xy.x + Mathf.Cos(Mathf.Deg2Rad * angle) * rad, xy.y + Mathf.Sin(Mathf.Deg2Rad * angle) * rad);
	}
}
