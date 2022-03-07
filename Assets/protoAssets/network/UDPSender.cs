using UnityEngine;
using System.Collections;

public class UDPSender : MonoBehaviour {
	
	public UDP udp;

	// default port
	[Header("send to")]
	public string ip = "127.0.0.1";
	public int sendPort = 11999;

	int localPort = -1;

	void Start()
	{

	}

	// localPort -1: auto
	public void SetLocalPort(int port = -1)
	{
		localPort = port;
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
	
	// override address and port
	public void Send(string str, string ip, int sendPort)
	{
		if(udp != null){
			udp.SendString(str, ip, sendPort);
		}
	}
	// use default address and port
	public void Send(string str)
	{
		if(udp != null){
			udp.SendString(str, ip, sendPort);
		}
	}
	
	public void SendPacket(byte[] packet, int length, string ip, int sendPort)
	{
		if(udp != null){
			udp.SendPacket(packet, length, ip, sendPort);
		}
	}
	public void SendPacket(byte[] packet, int length)
	{
		if(udp != null){
			udp.SendPacket(packet, length, ip, sendPort);
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

}
