using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nOSC;

/// <summary>
/// OSCでの入力を仮想Gamepad入力へ反映
/// touchOSCでテスト
/// </summary>
public class ViGEmOSCBinder : MonoBehaviour
{
    [SerializeField] ViGEmGamePad gamePad = null;

	[SerializeField] OscReceiver udpReciever;

	[SerializeField] bool autoConnect = true;

	// Use this for initialization
	void Awake()
	{
		Connect();
	}

	void Connect()
	{
		if (udpReciever == null)
		{
			udpReciever = this.gameObject.GetComponent<OscReceiver>();
		}
		udpReciever.Setup();
		udpReciever.SetAddressHandler("/leftThumb/1", OnLeftThumbHandler);
		udpReciever.SetAddressHandler("/rightThumb/1", OnRightThumbHandler);
		udpReciever.SetAddressHandler("/leftTrigger", OnLeftTriggerHandler);
		udpReciever.SetAddressHandler("/rightTrigger", OnRightTriggerHandler);

        udpReciever.SetAddressHandler("/btnA", OnBtnAHandler);
        udpReciever.SetAddressHandler("/btnB", OnBtnBHandler);

        udpReciever.SetAllMessageHandler(OnAllOscFunc);
	}
	void OnLeftThumbHandler(OscMessage oscM)
	{
		gamePad.LeftThumb = new Vector2(oscM.getArgAsFloat(0), oscM.getArgAsFloat(1));
	}
	void OnRightThumbHandler(OscMessage oscM)
	{
		gamePad.RightThumb = new Vector2(oscM.getArgAsFloat(0), oscM.getArgAsFloat(1));
	}
	void OnLeftTriggerHandler(OscMessage oscM)
	{
		gamePad.LeftTrigger = oscM.getArgAsFloat(0);
	}
	void OnRightTriggerHandler(OscMessage oscM)
	{
		gamePad.RightTrigger = oscM.getArgAsFloat(0);
	}
	void OnBtnAHandler(OscMessage oscM)
	{
		gamePad.BtnA = (oscM.getArgAsFloat(0) == 1);
	}
	void OnBtnBHandler(OscMessage oscM)
	{
		gamePad.BtnB = (oscM.getArgAsFloat(0) == 1);
	}

	void OnAllOscFunc(OscMessage oscM)
    {
        //Debug.Log($"oscM: { oscM.Address}" );
    }

    void Disconnect()
	{
		udpReciever?.Close();
	}

    //bool IsOpen()
    //{
    //	if (udpReciever == null)
    //	{
    //		return false;
    //	}
    //	else
    //	{
    //		return udpReciever.IsOpen();
    //	}
    //}

    private void OnDestroy()
    {
		Disconnect();

	}

}
