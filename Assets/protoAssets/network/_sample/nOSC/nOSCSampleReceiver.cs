using UnityEngine;
using System.Collections;

using nOSC;

public class nOSCSampleReceiver : MonoBehaviour {
	
	public OscReceiver udpReciever;

	public bool autoConnect = true;
	
	// Use this for initialization
	void Start () {
		if(autoConnect){
			Connect();
		}
	}

	void Connect()
	{
		if(udpReciever == null){
			udpReciever = this.gameObject.GetComponent<OscReceiver>();
		}
		udpReciever.Setup();
		udpReciever.SetAddressHandler ("/test", OnTestHandler);
		udpReciever.SetAddressHandler ("/curves", OnTestHandler);
		udpReciever.SetAddressHandler ("/curves2", OnTestHandler);
		udpReciever.SetAddressHandler ("/blob", OnBlobHandler);
		udpReciever.SetAllMessageHandler(OnAllOscFunc);
	}
	
	void OnTestHandler(OscMessage oscM){
		string str = Osc.OscMessageValueToString(oscM);
		Debug.Log ("oscM>>> "+oscM.Address + " / "+str);
	}
	void OnAllOscFunc(OscMessage oscM){
		Debug.LogWarning("oscM: "+ oscM.Address);
		//string str = Osc.OscMessageValueToString(oscM);
		//string[] strs = str.Split(","[0]);
//		Debug.Log("OnAllOscFunc >>> " + oscM.getAddress() + " // " + oscM.getArgAsFloat(0) +", " + oscM.getArgAsFloat(1)  +", " + oscM.getArgAsFloat(2));
		//Debug.Log("OnAllOscFunc >>> " + oscM.getAddress() + " / " + oscM.timeStamp.ToLocalTime() + " / "+strs);

	}
	void OnBlobHandler(OscMessage oscM)
	{
		Debug.Log ("oscM>>> "+oscM.Address + " / "+oscM.getArgAsInt(0) + " / "+oscM.getArgAsFloat(2) + " / "+oscM.getArgAsString(3) + " / "+oscM.getArgAsInt(4));
		byte[] bytes = oscM.getArgAsBlob (1);
		string str = "";
		for (int i = 0; i < bytes.Length; i++) {
			str += bytes[i].ToString()+ ", ";
		}
		Debug.Log (str);
	}
	
	void Disconnect()
	{
		udpReciever.Close();
	}

	bool IsOpen()
	{
		if(udpReciever == null){
			return false;
		}else{
			return udpReciever.IsOpen();
		}
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10,10,300,300));
		GUILayout.Label("UDP Receiver");
		if(GUILayout.Button("Connect: "+ IsOpen() )){
			Connect();
		}
		if(GUILayout.Button("DisConnect")){
			Disconnect();
		}
		GUILayout.Label((udpReciever.receivePort).ToString());

		GUIStyle style = GUI.skin.GetStyle("TextField");
		style.wordWrap = true;

		GUILayout.EndArea();
	}
}
