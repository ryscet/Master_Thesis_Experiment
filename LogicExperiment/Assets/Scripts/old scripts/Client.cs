//
//using UnityEngine;
//using System.Collections;
//using System;
//using System.IO;
//using System.Net.Sockets;
//
//public class Client : MonoBehaviour {
//	internal Boolean socketReady = false;
//	
//	TcpClient mySocket;
//	NetworkStream theStream;
//	StreamWriter theWriter;
//	StreamReader theReader;
//	String Host = "localhost";
//	Int32 Port = 13000;
//	int i = 0;
//	
//	void Start () {
//		setupSocket();
//	}
//	void Update () {
//	}
//	
//	void OnGUI()
//	{
//		if (GUI.Button(new Rect(10, 10, 100, 100), "Click")){
//			string m = "Hello World: " + DateTime.Now.ToString("hh:mm:ss:fff");
//			writeSocket(m);
//			i++;
//			Debug.Log(m);
//		}
//	}
//	// **********************************************
//	public void setupSocket() {
//		try {
//			mySocket = new TcpClient(Host, Port);
//			theStream = mySocket.GetStream();
//			theWriter = new StreamWriter(theStream);
//			theReader = new StreamReader(theStream);
//			socketReady = true;
//			
//			Byte[] data = System.Text.Encoding.ASCII.GetBytes("Psy");  
//			// Get a client stream for reading and writing. 
//			
//			NetworkStream stream = mySocket.GetStream();
//			stream.Write(data, 0, data.Length);
//			stream.Flush();
//		}
//		catch (Exception e) {
//			Debug.Log("Socket error: " + e);
//		}
//	}
//	public void writeSocket(string theLine) {
//		if (!socketReady)
//			return;
//		//String foo = theLine + "\r\n";
//		// Translate the passed message into ASCII and store it as a Byte array.
//		Byte[] data = System.Text.Encoding.ASCII.GetBytes(theLine);  
//		// Get a client stream for reading and writing. 
//		
//		NetworkStream stream = mySocket.GetStream();
//		stream.Write(data, 0, data.Length);
//		stream.Flush();
//		//theWriter.Write(data);
//		//theWriter.Flush();
//		
//		
//		
//		
//		// Send the message to the connected TcpServer. 
//		stream.Write(data, 0, data.Length);
//		
//	}
//	public String readSocket() {
//		if (!socketReady)
//			return "";
//		if (theStream.DataAvailable)
//			return theReader.ReadLine();
//		return "";
//	}
//	public void closeSocket() {
//		if (!socketReady)
//			return;
//		theWriter.Close();
//		theReader.Close();
//		mySocket.Close();
//		socketReady = false;
//	}
//} // end class s_TCP
//
