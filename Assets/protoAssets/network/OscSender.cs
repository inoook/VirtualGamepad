using UnityEngine;
using System.Collections;

namespace nOSC{
	public class OscSender : UDPSender {
		
		public void Send(OscMessage oscMessage)
		{
			Send(oscMessage, ip, sendPort);
		}
		/// <summary>
		/// Send an individual OSC message.  Internally takes the OscMessage object and 
		/// serializes it into a byte[] suitable for sending to the PacketIO.
		/// </summary>
		/// <param name="oscMessage">The OSC Message to send.</param>   
		public void Send (OscMessage oscMessage, string ip, int port)
		{
			byte[] packet = new byte[1000];
			int length = Osc.OscMessageToPacket (oscMessage, packet, 1000);
			base.SendPacket (packet, length, ip, port);
		}


		public void Send(ArrayList oms)
		{
//			byte[] packet = new byte[1000];
//			int length = Osc.OscMessagesToPacket(oms, packet, 1000);
//			base.SendPacket(packet, length);
			Send(oms, ip, sendPort);
		}

		/// <summary>
		/// Sends a list of OSC Messages.  Internally takes the OscMessage objects and 
		/// serializes them into a byte[] suitable for sending to the PacketExchange.
		/// </summary>
		/// <param name="oms">The OSC Message to send.</param>   
		public void Send(ArrayList oms, string ip, int port)
		{
			byte[] packet = new byte[1000];
			int length = Osc.OscMessagesToPacket(oms, packet, 1000);
//			base.sendBytes(packet, ip, port);
			base.SendPacket (packet, length, ip, port);
		}
	}
}
