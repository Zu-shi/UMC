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

	public static float HidingDuration = 2f;
	public static float HidingArrivalTime = 2.5f;
	public static float HidingSpeed = .8f;
	public static int HidingTotalBugs = 15;
	public static float HidingAngleRange = 50f;

	public static float WaveDuration = 2f;
	public static float WaveArrivalTime = 2f;
	public static float WaveSpeed = 1.6f;
	public static int WaveTotalBugs = 25;
	public static float WaveAngleRange = 55f;

	public static float DoubleFileDuration = 2.2f;
	public static float DoubleFileArrivalTime = 3f;
	public static float DoubleFileSpeed = 1.3f;
	public static int DoubleFileTotalBugs = 20;
	public static float DoubleFileAngleRange = 40f;


	public static string configPath;

	public static void getConfigValues(){
        /*
		configPath = Environment.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + "Config.txt";

		System.IO.StreamReader file = new System.IO.StreamReader(configPath);

		string line = "";
		while ( (line = file.ReadLine()) != null ){
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

			case "HidingDuration":
				HidingDuration = float.Parse(pair[1]);
				break;
				
			case "HidingArrivalTime":
				HidingArrivalTime = float.Parse(pair[1]);
				break;
				
			case "HidingSpeed":
				HidingSpeed = float.Parse(pair[1]);
				break;
				
			case "HidingTotalBugs":
				HidingTotalBugs = int.Parse(pair[1]);
				break;
				
			case "HidingAngleRange":
				HidingAngleRange = float.Parse(pair[1]);
				break;
			
			case "WaveDuration":
				WaveDuration = float.Parse(pair[1]);
				break;
				
			case "WaveArrivalTime":
				WaveArrivalTime = float.Parse(pair[1]);
				break;
				
			case "WaveSpeed":
				WaveSpeed = float.Parse(pair[1]);
				break;
				
			case "WaveTotalBugs":
				WaveTotalBugs = int.Parse(pair[1]);
				break;
				
			case "WaveAngleRange":
				WaveAngleRange = float.Parse(pair[1]);
				break;

			case "DoubleFileDuration":
				DoubleFileDuration = float.Parse(pair[1]);
				break;
				
			case "DoubleFileArrivalTime":
				DoubleFileArrivalTime = float.Parse(pair[1]);
				break;
				
			case "DoubleFileSpeed":
				DoubleFileSpeed = float.Parse(pair[1]);
				break;
				
			case "DoubleFileTotalBugs":
				DoubleFileTotalBugs = int.Parse(pair[1]);
				break;
				
			case "DoubleFileAngleRange":
				DoubleFileAngleRange = float.Parse(pair[1]);
				break;

			default:
				Debug.Log("Config value " + pair[0] + " is unrecognized.");
				break;
			}
		}
        */
	}

}
