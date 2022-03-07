using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nOSC;

/// <summary>
/// OSCでの入力を仮想Gamepad入力へ反映
/// </summary>
public class ViGEmOSCM5AtomBinder : MonoBehaviour
{
    [SerializeField] ViGEmGamePad gamePad = null;

	[SerializeField] OscReceiver udpReciever;

	[SerializeField] bool autoConnect = true;

	// Use this for initialization
	void Start()
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
		udpReciever.SetAddressHandler("/btnA", OnBtnAHandler);
		udpReciever.SetAddressHandler("/btnB", OnBtnBHandler);

		udpReciever.SetAddressHandler("/posture", OnPostureHandler);
		//udpReciever.SetAddressHandler("/acc", OnAccHandler);
		//udpReciever.SetAddressHandler("/gyro", OnGyroHandler);

		udpReciever.SetAllMessageHandler(OnAllOscFunc);
	}

	void OnBtnAHandler(OscMessage oscM)
	{
		try
		{
			gamePad.BtnA = (oscM.getArgAsInt(0) == 1);
		}
		catch{ }
	}
	void OnBtnBHandler(OscMessage oscM)
	{
		try
		{
			gamePad.BtnB = (oscM.getArgAsFloat(0) == 1);
		}
		catch { }
	}
	//
	[SerializeField] Vector3 pitchRollYaw = Vector3.zero;
	[SerializeField] float range = -90;
	[SerializeField] float rTriggerMin = 0;
	[SerializeField] float rTiriggerMax = 0;
	

	[SerializeField] bool useKalman = false;
	[SerializeField] Kalman.KalmanFilter1D kalmanFilter = new Kalman.KalmanFilter1D();

	void OnPostureHandler(OscMessage oscM)
	{
		float pitch = oscM.getArgAsFloat(0);
		float roll = oscM.getArgAsFloat(1);
		float yaw = oscM.getArgAsFloat(2);
		if (useKalman)
		{
			pitch = kalmanFilter.Update(pitch);
		}
		pitchRollYaw.x = pitch;
		pitchRollYaw.y = roll;
		pitchRollYaw.z = yaw;

		gamePad.LeftThumbX = Mathf.Clamp(pitch / range, -1, 1);

		// float t_angle = -Mathf.Atan2(acc.x, acc.y) * Mathf.Rad2Deg;
		gamePad.RightTrigger = Mathf.InverseLerp(rTriggerMin, rTiriggerMax, pitch);
		//Debug.LogWarning($"Posture: {pitch}, {roll}, {yaw}");
	}
	void OnAccHandler(OscMessage oscM)
	{
		Debug.LogWarning($"Acc: {oscM.getArgAsFloat(0)}, {oscM.getArgAsFloat(1)}, {oscM.getArgAsFloat(2)}");
	}
	void OnGyroHandler(OscMessage oscM)
	{
		Debug.LogWarning($"Gyro: {oscM.getArgAsFloat(0)}, {oscM.getArgAsFloat(1)}, {oscM.getArgAsFloat(2)}");
	}
	//

	void OnAllOscFunc(OscMessage oscM)
    {
        //Debug.Log($"oscM: { oscM.Address}" );
    }

    void Disconnect()
	{
		udpReciever?.Close();
	}

    private void OnDestroy()
    {
		Disconnect();

	}

}
