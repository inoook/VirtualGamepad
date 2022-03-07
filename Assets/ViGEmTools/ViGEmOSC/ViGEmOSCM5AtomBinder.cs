using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;

/// <summary>
/// OSCでの入力を仮想Gamepad入力へ反映
/// </summary>
public class ViGEmOSCM5AtomBinder : MonoBehaviour
{
    [SerializeField] ViGEmGamePad gamePad = null;

	[SerializeField] uOscServer oscReciever;

	[SerializeField] bool autoConnect = true;

	// Use this for initialization
	void Start()
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
		if (address == "/btnA")
		{
			gamePad.BtnA = (int)message.values[0] == 1;
		}
		else if (address == "/btnB")
		{
			gamePad.BtnB = (int)message.values[0] == 1;
		}
		else if (address == "/posture")
		{
			float pitch = (float)message.values[0];
			float roll = (float)message.values[1];
			float yaw = (float)message.values[2];
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
		else if (address == "/acc")
		{
			float x = (float)message.values[0];
			float y = (float)message.values[1];
			float z = (float)message.values[2];
			//Debug.LogWarning($"Acc: {x}, {y}, {z}");
		}
		else if (address == "/gyro")
		{
			float x = (float)message.values[0];
			float y = (float)message.values[1];
			float z = (float)message.values[2];
			//Debug.LogWarning($"Gyro: {x}, {y}, {z}");
		}
	}

	//
	[SerializeField] Vector3 pitchRollYaw = Vector3.zero;
	[SerializeField] float range = -90;
	[SerializeField] float rTriggerMin = 0;
	[SerializeField] float rTiriggerMax = 0;
	

	[SerializeField] bool useKalman = false;
	[SerializeField] Kalman.KalmanFilter1D kalmanFilter = new Kalman.KalmanFilter1D();

}
