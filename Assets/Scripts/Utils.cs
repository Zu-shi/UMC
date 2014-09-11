using UnityEngine;
using System.Collections;
using System;

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

    public static T RandomFromValues<T>(params T[] t){
        /*
        int choice = UnityEngine.Random.Range(0, t.Length);
        object parsedValue = t[choice];
        Type type = typeof(T);
        
        try
        {
            parsedValue = Convert.ChangeType(parsedValue, type);
        }
        catch (ArgumentException e)
        {
            parsedValue = null;
        }
        
        return (T) parsedValue;*/
        return (T)( t[UnityEngine.Random.Range(0, t.Length)] );
    }

    public static T RandomFromArray<T>(T[] t){
        return (T)( t[UnityEngine.Random.Range(0, t.Length)] );
    }
}
