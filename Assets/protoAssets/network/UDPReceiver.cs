using UnityEngine;
using System.Collections;
using System.Text;

public class UDPReceiver : MonoBehaviour {

	public delegate void ReceiveDataQueueHandler(string message);
	public delegate void ReceiveObjectQueueHandler(byte[] message, System.Net.IPEndPoint ipEp);
	
	public UDP udp;

	[Header("receive")]
	public int receivePort = 12345;

	public event ReceiveDataQueueHandler eventMessageReceivedQueue;
	public event ReceiveObjectQueueHandler eventReceivedObjectQueue;

	// Use this for initialization
	void Start () {
		
	}

	public void Setup () {

		if (udp == null) {
			udp = this.gameObject.AddComponent<UDP> ();
		}
		
		if(!udp.IsOpen()){
			udp.localPort = receivePort;
			udp.Create ();
		}

		udp.CreateReceiver ();
		udp.SetReceiveObjectQueueHandler(OnReceiveObjectQueue);

        SetupOption ();
	}

	protected virtual void SetupOption()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Queue 
	void OnReceiveObjectQueue(byte[] bytes, System.Net.IPEndPoint ipEp)
	{
//		Debug.Log("OnReceiveObjectQueue>> "+bytes + " / "+ipEp);
		if (eventReceivedObjectQueue != null) {
			eventReceivedObjectQueue (bytes, ipEp);
		}
		
		if(eventMessageReceivedQueue != null){
			Encoding sjisEnc = Encoding.GetEncoding ("utf-8");
			string message = sjisEnc.GetString (bytes);
			eventMessageReceivedQueue(message);
		}
	}
    //
    void OnReceivePacket(byte[] bytes, System.Net.IPEndPoint ipEp)
    { 
}


    void OnDisable()
    {
        Debug.Log("closing UDP socket in OnDisable");
		Close();
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
