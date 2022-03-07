using UnityEngine;
using System.Collections;
using System.Text;

namespace nOSC{
	public class OscReceiver : UDPReceiver {

		private OscMessageHandler allMessageHandler;
		Hashtable addressTable;

		protected override void SetupOption()
		{
			// Create the hashtable for the address lookup mechanism
			addressTable = new Hashtable ();
			base.eventReceivedObjectQueue += ReceivedObjectQueue;
		}

		public void SetAddressHandler (string key, OscMessageHandler ah)
		{
			Hashtable.Synchronized (addressTable).Add (key, ah);
		}
		public void SetAllMessageHandler(OscMessageHandler amh)
		{
			allMessageHandler = amh;
		}

		void ReceivedObjectQueue (byte[] buffer, System.Net.IPEndPoint ipEp)
		{
			try
			{
				ArrayList messages = Osc.PacketToOscMessages(buffer, buffer.Length);
				foreach (OscMessage om in messages)
				{
					if (allMessageHandler != null)
						allMessageHandler(om);
					OscMessageHandler h = (OscMessageHandler)Hashtable.Synchronized(addressTable)[om.Address];
					if (h != null)
						h(om);
				}
            }
            catch
            {
				Debug.LogWarning("osc error");
				Debug.LogWarning(string.Join(",", buffer));
			}
		}
	}
}
