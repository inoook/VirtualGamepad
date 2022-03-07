using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;

/// <summary>
/// OSCでの入力を仮想Gamepad入力へ反映
/// touchOSCでテスト
/// </summary>
public class ViGEmOSCBinder : MonoBehaviour
{
    [SerializeField] ViGEmGamePad gamePad = null;

	[SerializeField] uOscServer oscReciever = null;

	[SerializeField] bool autoConnect = true;

	// Use this for initialization
	void Awake()
	{
		Connect();
	}

	void Connect()
	{
		if (oscReciever == null)
		{
			oscReciever = this.gameObject.GetComponent<uOscServer>();
		}
		oscReciever.onDataReceived.AddListener(OnDataReceived);
	}

	void OnDataReceived(uOSC.Message message)
	{
		string address = message.address;
		if (address == "/leftThumb/1")
		{
			gamePad.LeftThumb = new Vector2((float)message.values[0], (float)message.values[1]);
		}
		else if (address == "/rightThumb/1")
		{
			gamePad.RightThumb = new Vector2((float)message.values[0], (float)message.values[1]);
		}
		else if (address == "/leftTrigger")
		{
			gamePad.LeftTrigger = (float)message.values[0];
		}
		else if (address == "/rightTrigger")
		{
			gamePad.RightTrigger = (float)message.values[0];
		}
		else if (address == "/btnA")
		{
			gamePad.BtnA = (float)message.values[0] == 1;
		}
		else if (address == "/btnB")
		{
			gamePad.BtnB = (float)message.values[0] == 1;
		}
	}


}
