using UnityEngine;
using System.Collections;
using System;

public static class Utils {

	public static bool printAssertMessages = false;
    public const int angleScale = 120;
    public const float leafRadius = 0.6f;
    public static float distanceScale { get{return leafRadius * Camera.main.orthographicSize * Camera.main.aspect;} }
    public static float warningBufferSize = 20f;
    public static float resolutionWidth = 480f;
    public static Vector2 OUT_OF_SCREEN = new Vector2(-100000f, -100000f);

    public static float halfScreenWidth 
    {
        get{ return Camera.main.orthographicSize * Camera.main.aspect; }
    }

    public static float halfScreenHeight
    {
        get{ return Camera.main.orthographicSize; }
    }

    public static Vector2 cameraPos
    {
        get{ return new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y); }
    }

    public static int RandomSign(){
        return Mathf.FloorToInt(UnityEngine.Random.Range(0f, 2f)) == 1 ? 1 : -1;
    }

	public static void Assert(bool statement, string context = ""){
		if(printAssertMessages && context != ""){
			Debug.Log(context);
		}

		if (!statement) {
			Debug.LogError("Assertion failed");
		}
	}	
    
    public static _Mono InstanceCreate(Vector2 xy, _Mono prefab){
        return InstanceCreate(xy.x, xy.y, prefab);
    }

    public static _Mono InstanceCreate(float x, float y, _Mono prefab){
        return InstanceCreate(x, y, 0, prefab);
    }

    public static _Mono InstanceCreate(float x, float y, float z, _Mono prefab){
        GameObject go = GameObject.Instantiate(prefab.gameObject, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        return go.AddComponent<_Mono>();
    }

    public static T InstanceCreate<T>(Vector2 xy, T prefab) where T : UnityEngine.Component{
        T go = GameObject.Instantiate(prefab, new Vector3(xy.x, xy.y, 0), Quaternion.identity) as T;
        return go.gameObject.AddComponent<_Mono>().GetComponent<T>();
    }

    public static _Mono InstanceCreate<T>(float x, float y, float z, T prefab) where T : UnityEngine.Component{
        T go = GameObject.Instantiate(prefab.gameObject, new Vector3(x, y, z), Quaternion.identity) as T;
        return go.gameObject.AddComponent<_Mono>();
    }

	public static Vector2 GetPointFromAngleAndRadius(Vector2 xy, float angle, float rad){
		return new Vector2(xy.x + Mathf.Cos(Mathf.Deg2Rad * angle) * rad, xy.y + Mathf.Sin(Mathf.Deg2Rad * angle) * rad);
	}

    public static T RandomFromValues<T>(params T[] t){
        return (T)( t[UnityEngine.Random.Range(0, t.Length)] );
    }

    public static T RandomFromArray<T>(T[] t){
        return (T)( t[UnityEngine.Random.Range(0, t.Length)] );
    }

    public static int RandomToInt(int max){
//        Debug.Log("Max = " + max);
        UnityEngine.Random.seed = (int)Time.realtimeSinceStartup;
        return Mathf.FloorToInt(UnityEngine.Random.Range(0f, max));
    }

    public static Vector2 RandomVectorInRadius(float radius){
        float r = UnityEngine.Random.Range(0f, radius);
        float theta = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
        return new Vector2(r * Mathf.Sin(theta), r * Mathf.Cos(theta));
    }

    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }
    /**
     * Given a coordinate system in polar coordinates, convert it to cartesian coordinates
     * The angles from [90 - angleScale, 90 + angleScale] is mapped to [-1, 1] for angle
     * In short, the angle is centered at 90f, pointing north.
     * The distance to target from [0, shield] is mapped to [0, 1] for scale
     * Returns the cartesian coordinate value
     * */
    public static Vector2 polarToCart(float theta, float dist){
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 targetPos = cameraPos - new Vector3(0f, Camera.main.orthographicSize / 4, 0f);
        float realTheta = Mathf.Deg2Rad * (theta + 90f);
        float realDist = dist * distanceScale;
        float x = targetPos.x + Mathf.Cos(realTheta) * realDist;
        float y = targetPos.y + Mathf.Sin(realTheta) * realDist;
        return new Vector2(x, y);
    }

    public static float PointDistance(Vector2 v1, Vector2 v2){
        return Vector2.Distance(v1, v2);
    }

    public static float PointAngle(Vector2 v1, Vector2 v2){
        return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * Mathf.Rad2Deg;
    }

    public static Vector2 AngleToBlinkerLocation(float theta){
        /*
        Vector2 joinLocation1 = new Vector2(cameraPos.x - halfScreenWidth + warningBufferSize, 
                                            cameraPos.y + halfScreenHeight - warningBufferSize);
        Vector2 joinLocation2 = new Vector2(cameraPos.x + halfScreenWidth - warningBufferSize, 
                                            cameraPos.y + halfScreenHeight - warningBufferSize);
        float joinSeperationAngle1 = Mathf.Atan2(joinLocation1.y - Globals.target.y, joinLocation1.x - Globals.target.x);
        float joinSeperationAngle2 = Mathf.Atan2(joinLocation2.y - Globals.target.y, joinLocation2.x - Globals.target.x);
        float realTheta = theta + 90f;
        if(joinSeperationAngle2 * Mathf.Rad2Deg > realTheta){
            return 
        }else if(joinSeperationAngle2 * Mathf.Rad2Deg > realTheta){

        }else{

        }*/
        return polarToCart(theta, 1.5f);
    }

    public static bool IsDestroyed(_Mono o){
        return (o == null || o.Equals(null));
    }

    
    public static bool IsBugDestroyed(Hazard o){
        return (o.isFading);
    }
}
