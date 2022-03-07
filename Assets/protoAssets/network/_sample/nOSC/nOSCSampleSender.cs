using UnityEngine;
using System.Collections;

using nOSC;

public class nOSCSampleSender: MonoBehaviour {

	public OscSender udpSender;
	public float sendRate = 24;

	public bool autoConnect = true;

	// Use this for initialization
	void Start () {
		int[] v = new int[]{ 1, 2, 3 };
		int[] v2 = new int[]{ 4, 5, 6, 7, 8, 9 };
		System.Array.Copy (v, 0, v2, 3, 3);
		for (int i = 0; i < v2.Length; i++) {
			Debug.Log (v2[i]);
		}

		if(autoConnect){
			Connect();
		}
	}

	float t = 0;
	void Update()
	{
		if (!isSend) { return; }

		t += Time.deltaTime;
		float d = 1.0f / sendRate;
		if (t > d) {
			//sendMsg ();
			sendOSCMsg();
			//t += d - t;
			t = t - d;
		}
	}

	void Connect()
	{
		if(udpSender == null){
			udpSender = this.gameObject.GetComponent<OscSender>();
		}
		udpSender.Setup();
	}
	void Disconnect()
	{
		udpSender.Close();
	}

	[SerializeField] bool isSend = false;

	public void sendMsg()
	{
		if(udpSender != null){
			OscMessage oscM = new OscMessage ();
			oscM.setAddress ("/blob");
			oscM.addIntArg (126);
			oscM.addBlobArg (new byte[]{1,2,3});
			oscM.addFloatArg (123.0f);
			oscM.addStringArg ("abc");
			oscM.addIntArg (300);

			udpSender.Send (oscM);
		}
	}

	[SerializeField] float v = 100;

	public void sendOSCMsg()
	{
		if(udpSender != null){

			OscMessage oscMF = new OscMessage();
			oscMF.setAddress("/curves");
			oscMF.addFloatArg(v);

			OscMessage oscMF2 = new OscMessage();
			oscMF2.setAddress("/curves2");
			oscMF2.addFloatArg(v);

			// bundle
			ArrayList oms = new ArrayList ();
			oms.Add (oscMF);
			oms.Add (oscMF2);
			udpSender.Send (oms);
		}
	}

	public string sendMessage = "send data";
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(350,10,300,300));
		GUILayout.Label("UDP Sender");
		GUILayout.Label("send to: "+udpSender.ip + " : "+udpSender.sendPort);

		if(GUILayout.Button("Connect: "+ udpSender.IsOpen())){
			Connect();
		}
		if(GUILayout.Button("DisConnect")){
			Disconnect();
		}
		if(GUILayout.Button("Send OSC")){
			if(udpSender != null){
				sendMsg ();
			}
		}
		if(GUILayout.Button("Send OSC Bundle")){
			sendOSCMsg ();
		}
		GUILayout.EndArea();
	}


}
