using UnityEngine;
using System.Collections;

public class UDPSampleSender: MonoBehaviour {

	public UDPSender udpSender;
	public float sendRate = 24;

	public bool autoConnect = true;

	// Use this for initialization
	void Start () {
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
			SendMsg ();
			// t += d - t;
			t = t - d;
		}
	}

	void Connect()
	{
		if(udpSender == null){
			udpSender = this.gameObject.GetComponent<UDPSender>();
		}
//		udpSender.SetLocalPort (12345);// localPortを指定するとき
		udpSender.Setup();
	}
	void Disconnect()
	{
		udpSender.Close();
	}

	[SerializeField] bool isSend = false;

	public void SendMsg()
	{
		if (!isSend) { return; }

		if(udpSender != null){
			udpSender.Send(sendMessage);
		}
	}

	public string sendMessage = "send data";
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(350,10,300,300));
		GUILayout.Label("UDP Sender");
		GUILayout.Label("send to: "+udpSender.ip + " : "+udpSender.sendPort);
		//GUILayout.Label ("localPort: "+udpSender.udp.GetLocalPort().ToString());
		if(GUILayout.Button("Connect: "+ udpSender.IsOpen())){
			Connect();
		}
		if(GUILayout.Button("DisConnect")){
			Disconnect();
		}
		sendMessage = GUILayout.TextField(sendMessage);
		if(GUILayout.Button("Send")){
			udpSender.Send(sendMessage + "\0");
		}
		GUILayout.EndArea();
	}
}
