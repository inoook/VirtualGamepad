using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPAdvance : MonoBehaviour {

	public UDP udp;
	
	public int localPort = -1;

	public event UDPReceiver.ReceiveObjectQueueHandler eventReceivedObjectQueue;

	void Start()
	{

	}

	// Use this for initialization
	public void Setup () {

		if(udp == null){
			udp = this.gameObject.AddComponent<UDP>();
		}

		if(!udp.IsOpen()){
			udp.localPort = localPort;
			udp.Create();
		}
	}
	public void SetupReceiver()
	{
		udp.CreateReceiver ();
		udp.SetReceiveObjectQueueHandler(OnReceivePacket);
	}

	#region send
	// override address and port
	public void Send(string str, string ip, int sendPort)
	{
		if(udp != null){
			udp.SendString(str, ip, sendPort);
		}
	}
	public void SendBytes(byte[] byteData, string ip, int sendPort)
	{
		if(udp != null){
			udp.SendPacket(byteData, ip, sendPort);
		}
	}

	#endregion

	void OnReceivePacket(byte[] buffer, System.Net.IPEndPoint fromIpEndPoint)
	{
//		string str = UDP.BytesToString (buffer);
//		Debug.Log (fromIpEndPoint.Address + " / "+fromIpEndPoint.Port + " / "+str);

		if (eventReceivedObjectQueue != null) {
			eventReceivedObjectQueue (buffer, fromIpEndPoint);
		}
	}

	public void Close()
	{
		if(udp != null){
			udp.Cancel();
			udp = null;
		}
	}

	public bool IsOpen()
	{
		if(udp == null){
			return false;
		}
		return udp.IsOpen();
	}

	void OnDisable()
	{
		Debug.Log("closing UDP socket in OnDisable");
		Close();
	}
}
