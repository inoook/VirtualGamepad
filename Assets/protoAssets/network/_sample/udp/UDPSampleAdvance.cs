using UnityEngine;
using System.Collections;

public class UDPSampleAdvance: MonoBehaviour {
	
	public UDPAdvance m_udp;

	public string sendIp = "127.0.0.1";
	public int sendPort = 11999;
	
	[SerializeField] bool autoConnect = true;

	// Use this for initialization
	void Start () {
		if (autoConnect) {
			Connect ();
		}
	}

	void Connect()
	{
		if(m_udp == null){
			m_udp = this.gameObject.GetComponent<UDPAdvance>();
		}
		m_udp.Setup();

		m_udp.eventReceivedObjectQueue += OnReceivePacket;
		m_udp.SetupReceiver ();
	}

	void Disconnect()
	{
		m_udp.Close();
	}

	public void SendMsg()
	{
		if(m_udp != null){
			m_udp.Send(sendMessage, sendIp, sendPort);
		}
	}

	void OnReceivePacket(byte[] buffer, System.Net.IPEndPoint fromIpEndPoint)
	{
		string str = UDP.BytesToString (buffer);
		Debug.Log ("OnReceivePacket: "+fromIpEndPoint.Address + " / "+fromIpEndPoint.Port + " / "+str);
	}
	
	public string sendMessage = "send data";
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(350,10,300,300));
		GUILayout.Label("UDP Sender");
		GUILayout.Label("send to: "+sendIp + " : "+sendPort);

		if(GUILayout.Button("Connect: "+ m_udp.IsOpen())){
			Connect();
		}
		if(GUILayout.Button("DisConnect")){
			Disconnect();
		}
		sendMessage = GUILayout.TextField(sendMessage);
		if(GUILayout.Button("Send")){
			SendMsg();
		}
		GUILayout.EndArea();
	}
}
