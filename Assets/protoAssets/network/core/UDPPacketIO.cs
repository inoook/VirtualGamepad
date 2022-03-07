using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;

//using System.Threading;
using UnityEngine;
using System.Text;

public delegate void ReceiveBufferHandler (byte[] buffer, IPEndPoint ipEp);

// http://www.sundh.com/blog/wp-content/uploads/2012/06/UDPPacketIO.cs
// http://social.msdn.microsoft.com/Forums/en/netfxnetcom/thread/baa3a5bb-2154-445f-965d-8a139dbe932a

/// <summary>
/// UdpPacket provides packetIO over UDP
/// </summary>
public class UDPPacketIO : MonoBehaviour
{
	private UdpClient udpClient;
	private bool socketsOpen;
	
	private int localPort = -1;

	private Queue receiveObjectQueue;
	
	private class ReceiveObject{
		public byte[] packets;
		public IPEndPoint ipEp;
	}

	void Start ()
	{
		//do nothing. init must be called  	
	}

	public void Init (int port)
	{
		localPort = port;
		socketsOpen = false;
	}

	public void CreateReceiveQueue ()
	{
		receiveObjectQueue = Queue.Synchronized (new Queue ());
	}
	
	~UDPPacketIO ()
	{
		// latest time for this socket to be closed
		if (IsOpen ())
			Close ();
	}

	/// <summary>
	/// Open a UDP socket and create a UDP sender.
	/// 
	/// </summary>
	/// <returns>True on success, false on failure.</returns>
	public bool Open ()
	{
		Debug.Log ("### UDPPacketIO / Open ###");
		
		try {
			Debug.Log ("localPort: " + localPort);

			if(localPort <= 0){
				// autoCreate localPort
				udpClient = new UdpClient ();
			}else{
				IPEndPoint remoteIp = new IPEndPoint (IPAddress.Any, localPort);// localPort: send時に自分が使用するport
				udpClient = new UdpClient (remoteIp);
			}

			((Socket)(udpClient.Client)).SendBufferSize = 65507;//max
			//((Socket)(Sender.Client)).SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 65507);
			//Sender.ExclusiveAddressUse = true;
			//Sender.EnableBroadcast = true;
			//Sender.MulticastLoopback = false;
			//Sender.Ttl = 1;
			//Debug.Log( sender.DontFragment );

			socketsOpen = true;
			return true;
		} catch (Exception e) {
			Debug.LogWarning ("cannot open udp client interface at port " + localPort);
			Debug.LogWarning (e);
		}

		return false;
	}

	/// <summary>
	/// Close the socket currently listening, and destroy the UDP sender device.
	/// </summary>
	public void Close ()
	{
		if (!socketsOpen) {
			return;
		}
		if (udpClient != null)
			udpClient.Close ();
    
		// receive
		if (receiveObjectQueue != null) {
			receiveObjectQueue = null;
		}

		Debug.Log ("UDP closed: "+this.gameObject.name);

		socketsOpen = false;
	}

	public void OnDisable ()
	{
		Close ();
	}

	/// <summary>
	/// Query the open state of the UDP socket.
	/// </summary>
	/// <returns>True if open, false if closed.</returns>
	public bool IsOpen ()
	{
		return socketsOpen;
	}
	
//	public void SendString (string str, string host, int port)
//	{
//		Encoding sjisEnc = Encoding.GetEncoding ("utf-8");
//		byte[] packet = sjisEnc.GetBytes (str);
//		SendPacket (packet, packet.Length, host, port);
//	}

	// add
	/// <summary>
	/// Send a packet of bytes out via UDP.
	/// </summary>
	/// <param name="packet">The packet of bytes to be sent.</param>
	/// <param name="length">The length of the packet of bytes to be sent.</param>
	public void SendPacket (byte[] packet, int length, string host, int port)
	{
		if (!IsOpen ())
			Open ();
		if (!IsOpen ())
			return;
		
		IPEndPoint ipEP = new IPEndPoint (IPAddress.Parse (host), port);
		udpClient.Send (packet, length, ipEP);

		// broadcast
		//		IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
		//		sender.Send(packet, length, groupEP);
	}


    public ReceiveBufferHandler eventReceivePackets;

    /// <summary>
    /// Receive a packet of bytes over UDP.
    /// </summary>
    /// <param name="buffer">The buffer to be read into.</param>
    /// <returns>The number of bytes read, or 0 on failure.</returns>
    public int ReceivePacket (byte[] buffer)
	{
		if (!IsOpen ())
			Open ();
		if (!IsOpen ())
			return 0;
		
//		IPEndPoint iep = new IPEndPoint (IPAddress.Any, localPort);
		IPEndPoint iep = null;
		byte[] incoming = udpClient.Receive (ref iep);

		int count = Math.Min (buffer.Length, incoming.Length);

		System.Array.Copy (incoming, buffer, count);
//		Debug.Log (incoming.Length + " / "+buffer.Length + " / "+count);
		//
		if (count > 0) {
			// queueに追加
			if (receiveObjectQueue != null) {
				ReceiveObject receive = new ReceiveObject();
//					receive.packets = buffer;
				receive.packets = incoming;// osc 動く
				receive.ipEp = iep;

				receiveObjectQueue.Enqueue (receive);
			}
		}

		return count;
	}

    //System.DateTime t ;
    //	public byte[] ReceivePacket ()
    public int ReceivePacket ()
	{
		if (!IsOpen ())
			Open ();
		if (!IsOpen ())
			return -1;

		while (udpClient.Available > 0) {
			//Debug.Log (udpClient.Available);
			//		IPEndPoint iep = new IPEndPoint (IPAddress.Any, localPort);
			IPEndPoint iep = null;
			byte[] incoming = udpClient.Receive (ref iep);

			int count = incoming.Length;

			if (count > 0) {
                if (eventReceivePackets != null)
                {
                    //System.DateTime cTime = DateTime.Now;
                    //Debug.Log("act: "+(cTime - t).TotalMilliseconds + " / "+incoming.Length);
                    //t = cTime;
                    eventReceivePackets(incoming, iep);
                }
                // queueに追加
                if (receiveObjectQueue != null) {
					ReceiveObject receive = new ReceiveObject ();
					//					receive.packets = buffer;
					receive.packets = incoming;// osc 動く
					receive.ipEp = iep;

					receiveObjectQueue.Enqueue (receive);
				}
			}
			return incoming.Length;
		}
		return -1;
	}

	//
	// Queue
	public ReceiveBufferHandler eventReceiveBuffer;

	void Update ()
	{
		if (this.eventReceiveBuffer != null) {
			if (receiveObjectQueue != null) {
				lock (receiveObjectQueue.SyncRoot) {
					while (receiveObjectQueue.Count > 0) {
						ReceiveObject rObj = (ReceiveObject)(receiveObjectQueue.Dequeue ());
						this.eventReceiveBuffer (rObj.packets, rObj.ipEp);
					}
				}
			}
		}
	}

	public int GetLocalPort()
	{
        if(udpClient == null || udpClient.Client == null)
        {
            return -1;
        }
		return ((IPEndPoint)(udpClient.Client.LocalEndPoint)).Port;
	}
//
}
