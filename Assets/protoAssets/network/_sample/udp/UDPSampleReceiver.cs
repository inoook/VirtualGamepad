using UnityEngine;
using System.Collections;

namespace nOSC {
    public class UDPSampleReceiver : MonoBehaviour {

        public UDPReceiver udpReciever;
        public bool autoConnect = true;

        // Use this for initialization
        void Start()
        {
            if (autoConnect)
            {
                Connect();
            }
        }

        void Connect()
        {
            if (udpReciever == null)
            {
                udpReciever = this.gameObject.GetComponent<UDPReceiver>();
            }
            udpReciever.eventMessageReceivedQueue += HandleEventMessageReceivedQueue;
            udpReciever.eventReceivedObjectQueue += ReceivedObjectQueue;

            udpReciever.Setup();
        }

        [SerializeField] int rpm = 0;
        [SerializeField] int speed = 0;

        void ReceivedObjectQueue(byte[] message, System.Net.IPEndPoint ipEp)
        {
            Debug.Log("ReceivedObjectQueue>> " + message + " / " + message.Length + " / " + ipEp);
            if (message.Length >= 7)
            {
                Debug.Log(message[0] + " / " + message[1] + " / " + (byte)(message[6] ^ 0xFF));
                rpm = System.BitConverter.ToInt16(new byte[] { (byte)(message[2] ^ 0xFF), (byte)(message[3] ^ 0xFF) }, 0);
                speed = System.BitConverter.ToInt16(new byte[] { (byte)(message[4] ^ 0xFF), (byte)(message[5] ^ 0xFF) }, 0);
                Debug.Log(rpm + " / " + speed);
            }
            else
            {
                Debug.LogWarning("xxxxx: " + message.Length);
            }
        }

        void HandleEventMessageReceivedQueue(string message)
        {
            Debug.Log("OnReceiveDataQueue>> " + message);
            if (message.IndexOf("\0") > -1)
            {
                Debug.Log("OK: " + message);
            }
        }

        void Disconnect()
        {
            udpReciever.eventMessageReceivedQueue -= HandleEventMessageReceivedQueue;
            udpReciever.eventReceivedObjectQueue -= ReceivedObjectQueue;

            udpReciever.Close();
        }

        bool IsOpen()
        {
            if (udpReciever == null)
            {
                return false;
            }
            else
            {
                return udpReciever.IsOpen();
            }
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            GUILayout.Label("UDP Receiver");
            if (GUILayout.Button("Connect: " + IsOpen()))
            {
                Connect();
            }
            if (GUILayout.Button("DisConnect"))
            {
                Disconnect();
            }
            GUILayout.Label((udpReciever.receivePort).ToString());

            GUIStyle style = GUI.skin.GetStyle("TextField");
            style.wordWrap = true;

            //		GUILayout.TextField(udpReciever.receiveMessage, style, GUILayout.Height(100));

            GUILayout.EndArea();
            /*
            if(GUILayout.Button(new Rect(10,10,100,100), "Send")){
                udp.SendData("sendStr");
            }
            */
        }
    }
}
