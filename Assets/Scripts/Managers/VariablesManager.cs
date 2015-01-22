using UnityEngine;
using System.Collections;

using System;
using System.IO;

public class VariablesManager : MonoBehaviour {

    //THESE ARE THE DEFAULT VALUES
    public static float RoundDuration = 2f;
    public static float RoundArrivalTime = 4f;
    public static float RoundSpeed = 1f;
    public static int RoundTotalBugs = 15;
    public static float RoundAngleRange = 85f;

	public static string configPath;

	public static void getConfigValues(){

		configPath = Environment.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + "Config.txt";

		System.IO.StreamReader file = new System.IO.StreamReader(configPath);

		string line = "";
		while ( (line = file.ReadLine) != null ){
			String[] pair = line.Split(' ');

			switch(pair[0]){
			case "RoundDuration":
				RoundDuration = float.Parse(pair[1]);
				break;

			case "RoundArrivalTime":
				RoundArrivalTime = float.Parse(pair[1]);
				break;

			case "RoundSpeed":
				RoundSpeed = float.Parse(pair[1]);
				break;

			case "RoundTotalBugs":
				RoundTotalBugs = int.Parse(pair[1]);
				break;

			case "RoundAngleRange":
				RoundAngleRange = float.Parse(pair[1]);
				break;
			
			default:
				Debug.Log("Config value " + pair[0] + " is unrecognized.");
				break;
			}
		}

	}

}
