using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;
using System;


public class LogMsg {
	
	System.IO.StreamWriter logFile;
	DateTime start;
	DateTime end;
	public LogMsg (string name, string header) {
		
		start = System.DateTime.Now;
		string date = DateTime.Now.ToString("MM_dd_HH_mm");
		string folderPath = @"./logs/" + "/" + date +"/";
		
		if(Directory.Exists(folderPath) == false){
			DirectoryInfo di = Directory.CreateDirectory(folderPath);
			
		}
		string path = folderPath + name + ".csv";
		if(File.Exists(path)){
			path = folderPath + name + "(2).csv";	
		}
		logFile = new System.IO.StreamWriter(path, true);
		logFile.WriteLine("#Starting log sesion " + System.DateTime.Now.ToString("hh:mm:ss:fff"));	
		logFile.WriteLine("%Timestamp;" + header );	
	}
	
	public void LogMessage(string msg){
		string logEntry = "";
		logEntry = DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";" + msg;
//		Debug.Log("Log msg: " +msg);
		logFile.WriteLine(logEntry);
	}
	
	public void myClose(){
		end = System.DateTime.Now;
		TimeSpan duration = end - start;
		int ms = (int)duration.TotalMilliseconds;
		if(logFile != null)
			logFile.WriteLine("closing session;" +ms.ToString());
		logFile.Close();		
	}
	
	void OnApplicationQuit(){
		
		myClose();
	}
}






//public class LogMsg {
//	
//	System.IO.StreamWriter logFile = new StreamWriter();
//	DateTime start;
//	DateTime end;
//	public string path;
//	public LogMsg (string name, string header) {
//		
//		start = System.DateTime.Now;
//		string date = DateTime.Now.ToString("MM_dd_HH_mm");
//		string folderPath = @"./logs/" + "/" + date +"/";
//		
//		if(Directory.Exists(folderPath) == false){
//			DirectoryInfo di = Directory.CreateDirectory(folderPath);
//			
//		}
//		path = folderPath + name + ".csv";
//		if(File.Exists(path)){
//			path = folderPath + name + "(2).csv";	
//		}
//
//		using (System.IO.StreamWriter sw = File.CreateText(path)) {
//
//						sw.WriteLine ("#Starting log sesion " + System.DateTime.Now.ToString ("hh:mm:ss:fff"));	
//						sw.WriteLine ("%Timestamp;" + header);	
//				}
//	}

//	public void LogMessage(string msg){
//		string logEntry = msg + ";";
//		
//		
//		
//		System.DateTime UnixEpoch = new DateTime (1970, 1, 1);
//		TimeSpan now = DateTime.UtcNow - UnixEpoch;
//		
//		double myTime = (double)now.TotalMilliseconds;
//		logEntry += now.TotalMilliseconds.ToString () +";";
//		
////		Debug.Log (logEntry);
//		
//		using (System.IO.StreamWriter sw = File.AppendText(path)) {
//			sw.WriteLine(logEntry);
//			
//		}
//	}



//	public void LogMessage(string msg){
//		string logEntry = "";
//		//logEntry = DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";" + msg;
//		logEntry = DateTime.Now.Ticks.ToString();
//
//		System.DateTime UnixEpoch = new DateTime(1970, 1, 1);
//		System.DateTime now = DateTime.Now;
//		TimeSpan time = now - UnixEpoch;
//		int myTime = (int)time.Ticks;
//		Debug.Log (myTime);
//
//		using (System.IO.StreamWriter sw = File.AppendText(path)) {
//			sw.WriteLine(myTime.ToString());
//
//				}
//	}

//
//	public void LogMessage(string msg){
//		string logEntry = "";
//		logEntry = DateTime.Now.ToString("hh:mm:ss:fff",CultureInfo.InvariantCulture) + ";" + msg;
//		
//		logFile.WriteLine(logEntry);
//	}
//	
//	public void myClose(){
//		end = System.DateTime.Now;
//		TimeSpan duration = end - start;
//		int ms = (int)duration.TotalMilliseconds;
//		string logEntry = "";
//		logEntry = "closing session;" + System.DateTime.Now.ToString("hh:mm:ss:fff") + ";" + ms;
//		
//		using (System.IO.StreamWriter sw = File.AppendText(path)) {
//			sw.WriteLine(logEntry);
//		}
//	}
//	
//	void OnApplicationQuit(){
//		myClose();
//	}
//}

//
//public LogMsg (string name, string header) {
//	
//	start = System.DateTime.Now;
//	string date = DateTime.Now.ToString("MM_dd_HH_mm");
//	string folderPath = @"./logs/" + "/" + date +"/";
//	
//	if(Directory.Exists(folderPath) == false){
//		DirectoryInfo di = Directory.CreateDirectory(folderPath);
//		
//	}
//	path = folderPath + name + ".csv";
//	if(File.Exists(path)){
//		path = folderPath + name + "(2).csv";	
//	}
//	logFile = new System.IO.StreamWriter(path, true);
//	logFile.WriteLine("#Starting log sesion " + System.DateTime.Now.ToString("hh:mm:ss:fff"));	
//	logFile.WriteLine("%Timestamp;" + header );	
//}

