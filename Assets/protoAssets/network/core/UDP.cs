using UnityEngine;
using System.Collections;
using System.Threading;
using System;

// http://www.sundh.com/blog/wp-content/uploads/2012/06/Osc.cs
// TODO: ocsとまとめる。
[RequireComponent (typeof(UDPPacketIO))]
public class UDP : MonoBehaviour
{
// broadcast 255.255.255.255 // 送り先
	public int localPort = -1;//11998
	
	public UDPPacketIO udpPacketIO;
	
	Thread readThread;
	bool readerRunning = false;
	
	public void Create ()
	{
		udpPacketIO = this.gameObject.GetComponent<UDPPacketIO> ();
		if (udpPacketIO == null) {
			udpPacketIO = this.gameObject.AddComponent<UDPPacketIO> ();
		}

		udpPacketIO.Init (localPort);
		udpPacketIO.Open ();
	}
	
	public void CreateReceiver()
	{
		udpPacketIO.CreateReceiveQueue ();

		// thread による read
		readThread = new Thread (Read);
		readerRunning = true;
		readThread.IsBackground = true;      
		readThread.Start ();
	}
	
	public void Cancel ()
	{
		if (udpPacketIO != null && udpPacketIO.IsOpen ()) {
			udpPacketIO.Close ();
			udpPacketIO = null;
		}

		if (!readerRunning) return;

		readerRunning = false;

		if (readThread.IsAlive) {
			int timeoutMilliseconds = 3000;
			readThread.Join (timeoutMilliseconds);
			if (readThread.IsAlive) {
				readThread.Abort ();
			}
		}
	}

	public bool IsOpen ()
	{
		if (udpPacketIO != null && udpPacketIO.IsOpen ()) {
			return true;
		} else {
			return false;
		}
	}

	#region receive
	private void Read ()
	{
		try {
			while (readerRunning) {
//				byte[] buffer = new byte[100000];
//				int length = udpPacketIO.ReceivePacket (buffer);
				int length = udpPacketIO.ReceivePacket ();
				if (length <= 0) {
					Thread.Sleep (20);
				}
			}
		} catch (ThreadAbortException e) {
			Debug.Log ("ThreadAbortException" + e);
		}
	}

	public void SetReceiveObjectQueueHandler (ReceiveBufferHandler handler)
	{
		udpPacketIO.eventReceiveBuffer = handler;
	}
    public void SetReceivePacketHandler(ReceiveBufferHandler handler)
    {
        udpPacketIO.eventReceivePackets = handler;
    }
    #endregion

    #region send
    public void SendString (string str, string host, int port)
	{
		System.Text.Encoding sjisEnc = System.Text.Encoding.GetEncoding ("utf-8");
		byte[] packet = sjisEnc.GetBytes (str);
		SendPacket (packet, host, port);
	}
	
	public void SendPacket (byte[] packet, string host, int port)
	{
		if (udpPacketIO != null) {
			udpPacketIO.SendPacket (packet, packet.Length, host, port);
			//int leng = (packet.Length > 9216) ? 9216 : packet.Length;
			//udpPacketIO.SendPacket(packet, leng);
		} else {
			Debug.LogWarning ("udpPacketIO is null");
		}
	}
	public void SendPacket (byte[] packet, int length, string host, int port)
	{
		if (udpPacketIO != null) {
			udpPacketIO.SendPacket (packet, length, host, port);
		} else {
			Debug.LogWarning ("udpPacketIO is null");
		}
	}
	#endregion

	public int GetLocalPort()
	{
		if (udpPacketIO == null) {
			return -1;
		}
		return udpPacketIO.GetLocalPort ();
	}


	#region utils
	public static string BytesToString(byte[] bytes)
	{
		System.Text.Encoding sjisEnc = System.Text.Encoding.GetEncoding ("utf-8");
		return sjisEnc.GetString (bytes);
	}
	public static byte[] StringToBytes(string str)
	{
		System.Text.Encoding sjisEnc = System.Text.Encoding.GetEncoding ("utf-8");
		return sjisEnc.GetBytes (str);
	}
	#endregion
}
