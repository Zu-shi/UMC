using UnityEngine;
using System.Collections;

using System;
using System.IO;

public class LoggingManager : MonoBehaviour {

	private static string logFilePath;
	private static int rewardNum = 0;

	public static void startLogging(){
		//Start writing to log file. Current Directory is used because of issues with permissions.
		logFilePath = Environment.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + "TreeGameLogFile.txt";
		
		String ts = "Game Start: " + TimeStamp ();
		
		if (!File.Exists (logFilePath)) {
			/*using (StreamWriter sw = File.CreateText(logFilePath)){
				sw.WriteLine(ts);
			}*/
		}
		else{
			/*using(StreamWriter sw = File.AppendText(logFilePath)){
				sw.WriteLine(ts);
			}*/
		}
	}

	public static void recordComboCount(){
        /*
		int comboNum = Globals.comboManager.comboTally;
		Debug.Log ("Combo count is " + comboNum);

		using(StreamWriter sw = File.AppendText(logFilePath)){
			sw.WriteLine("Combo Count: " + comboNum);
		}
  */      
	}

	public static void recordRewards(){
		/*using(StreamWriter sw = File.AppendText(logFilePath)){
			sw.WriteLine("Reward Count: " + rewardNum);
		}*/
	}

	public static void reward(){
		rewardNum++;
	}
	
	public static void endLogging(){
		//Write the end of this game's log entry
		String ts = "Game End: " + TimeStamp ();
		/*
		using(StreamWriter sw = File.AppendText(logFilePath)){
			
			sw.WriteLine(ts);
		}*/
	}
	
	private static string TimeStamp(){
		return DateTime.Now.ToString ("G");
	}
}
