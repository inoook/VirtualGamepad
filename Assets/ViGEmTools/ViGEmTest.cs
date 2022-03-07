using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using Nefarius.ViGEm.Client.Targets.DualShock4;

public class ViGEmTest : MonoBehaviour
{
    ViGEmClient viGEmClient = null;

    [SerializeField, Range(-1, 1)] float leftThumbX = 0;
    [SerializeField, Range(0, 1)] float rightTrigger = 0;
    [SerializeField, Range(0, 1)] float leftTrigger = 0;


    IXbox360Controller xboxController = null;
    IDualShock4Controller psController = null;

    [SerializeField] byte ledNumber;
    [SerializeField] byte largeMotor;
    [SerializeField] byte smallMotor;

    // Start is called before the first frame update
    void Start()
    {
        viGEmClient = new ViGEmClient();
        xboxController = viGEmClient.CreateXbox360Controller();
        xboxController.Connect();
        xboxController.FeedbackReceived += Controller_FeedbackReceived;

        //psController = viGEmClient.CreateDualShock4Controller();
        //psController.Connect();
        //psController.FeedbackReceived += PsController_FeedbackReceived;
    }

    private void PsController_FeedbackReceived(object sender, DualShock4FeedbackReceivedEventArgs e)
    {
        //ledNumber = e.LedNumber;
        largeMotor = e.LargeMotor;
        smallMotor = e.SmallMotor;
    }

    private void Controller_FeedbackReceived(object sender, Xbox360FeedbackReceivedEventArgs e)
    {
        ledNumber = e.LedNumber;
        largeMotor = e.LargeMotor;
        smallMotor = e.SmallMotor;
    }

    // Update is called once per frame
    void Update()
    {
        leftThumbX = Mathf.Sin(Time.time);
        rightTrigger = (Mathf.Sin(Time.time) + 1) / 2;
        leftTrigger = (Mathf.Cos(Time.time) + 1) / 2;

        xboxController.SetAxisValue(Xbox360Axis.LeftThumbX, (short)(leftThumbX * 32767));
        //xboxController.SetSliderValue(Xbox360Slider.RightTrigger, (byte)(rightTrigger * 255)); // accel
        //xboxController.SetSliderValue(Xbox360Slider.LeftTrigger, (byte)(leftTrigger * 255));// brake

        //psController.SetAxisValue(DualShock4Axis.LeftThumbX, (byte)(((leftThumbX+1)/2) * 255));
        //psController.SetSliderValue(DualShock4Slider.RightTrigger, (byte)(rightTrigger * 255));
        //psController.SetSliderValue(DualShock4Slider.LeftTrigger, (byte)(leftTrigger * 255));
    }

    private void OnDisable()
    {
        xboxController?.Disconnect();
        psController?.Disconnect();

        viGEmClient.Dispose();
    }
}
