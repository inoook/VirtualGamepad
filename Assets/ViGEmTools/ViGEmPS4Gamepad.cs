using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.DualShock4;

public class ViGEmPS4Gamepad : ViGEmGamePad, IViGEmGamePad
{
    ViGEmClient viGEmClient = null;

    public bool btn11 = false;
    public bool btn12 = false;
    public bool btn13 = false;


    public static Dictionary<Direction, DualShock4DPadDirection> directionDic = new Dictionary<Direction, DualShock4DPadDirection>()
    {
        { Direction.None, DualShock4DPadDirection.None},
        { Direction.Northwest, DualShock4DPadDirection.Northwest},
        { Direction.West, DualShock4DPadDirection.West},
        { Direction.Southwest, DualShock4DPadDirection.Southwest},
        { Direction.South, DualShock4DPadDirection.South},
        { Direction.Southeast, DualShock4DPadDirection.Southeast},
        { Direction.East, DualShock4DPadDirection.East},
        { Direction.Northeast, DualShock4DPadDirection.Northeast},
        { Direction.North, DualShock4DPadDirection.North}
    };


    [SerializeField] byte ledNumber;

    IDualShock4Controller psController = null;

    // Start is called before the first frame update
    void Start()
    {
        viGEmClient = new ViGEmClient();
        psController = viGEmClient.CreateDualShock4Controller();
        psController.Connect();
        psController.FeedbackReceived += PsController_FeedbackReceived;
    }

    private void PsController_FeedbackReceived(object sender, DualShock4FeedbackReceivedEventArgs e)
    {
        //ledNumber = e.LedNumber;
        largeMotor = e.LargeMotor;
        smallMotor = e.SmallMotor;
    }

    public override void OnLeftThumb(Vector2 v)
    {
        //Debug.Log("OnLeftThumb: " + v);
        psController.SetAxisValue(DualShock4Axis.LeftThumbX, (byte)(((v.x + 1) / 2) * 255));
        psController.SetAxisValue(DualShock4Axis.LeftThumbY, (byte)(((1 - v.y) / 2) * 255));
    }
    public override void OnRightThumb(Vector2 v)
    {
        //Debug.Log("OnRightThumb: " + v);
        psController.SetAxisValue(DualShock4Axis.RightThumbX, (byte)(((v.x + 1) / 2) * 255));
        psController.SetAxisValue(DualShock4Axis.RightThumbY, (byte)(((1 - v.y) / 2) * 255));
    }
    public override void OnLeftTrigger(float v)
    {
        //Debug.Log("OnLeftTrigger: " + v);
        psController.SetSliderValue(DualShock4Slider.LeftTrigger, (byte)(v * 255));// brake
    }
    public override void OnRightTrigger(float v)
    {
        //Debug.Log("OnRightTrigger: " + v);
        psController.SetSliderValue(DualShock4Slider.RightTrigger, (byte)(v * 255));// accel
    }
    //
    public override void OnBtnA(bool press)
    {
        //Debug.Log("OnBtnA: "+press);
        psController.SetButtonState(DualShock4Button.Cross, press);
    }
    public override void OnBtnB(bool press)
    {
        //Debug.Log("OnBtnB: " + press);
        psController.SetButtonState(DualShock4Button.Circle, press);
    }
    public override void OnBtnX(bool press)
    {
        //Debug.Log("OnBtnX: " + press);
        psController.SetButtonState(DualShock4Button.Square, press);
    }
    public override void OnBtnY(bool press)
    {
        //Debug.Log("OnBtnY: " + press);
        psController.SetButtonState(DualShock4Button.Triangle, press);
    }
    public override void OnBtnShoulderL(bool press)
    {
        //Debug.Log("OnBtnShoulderL: " + press);
        psController.SetButtonState(DualShock4Button.ShoulderLeft, press);
    }
    public override void OnBtnShoulderR(bool press)
    {
        //Debug.Log("OnBtnShoulderR: " + press);
        psController.SetButtonState(DualShock4Button.ShoulderRight, press);
    }
    //
    public override void OnBtnGuide(bool press)
    {
        //Debug.Log("OnBtnGuide: " + press);
        psController.SetButtonState(DualShock4Button.Share, press);
    }
    public override void OnBtnStart(bool press)
    {
        //Debug.Log("OnBtnStart: " + press);
        psController.SetButtonState(DualShock4Button.Options, press);
    }
    public override void OnBtnLeftThumb(bool press)
    {
        //Debug.Log("OnBtnLeftThumb: " + press);
        psController.SetButtonState(DualShock4Button.ThumbLeft, press);
    }
    public override void OnBtnRightThumb(bool press)
    {
        //Debug.Log("OnBtnRightThumb: " + press);
        psController.SetButtonState(DualShock4Button.ThumbRight, press);
    }
    public override void OnBtnPad(bool press)
    {
        //Debug.Log("OnBtnPad: " + press);
        //psController.SetButtonState(13, btn13);
    }
    //
    public override void OnBtnPov(ViGEmGamePad.Direction direction_)
    {
        //Debug.Log("OnBtnPov: " + direction_);
        psController.SetDPadDirection(directionDic[direction_]);
    }

    private void OnDisable()
    {
        psController?.Disconnect();

        viGEmClient.Dispose();
    }

    [ContextMenu("Test_Connect")]
    void Test_Connect()
    {
        psController?.Connect();
    }

    [ContextMenu("Test_Disconnect")]
    void Test_Disconnect()
    {
        psController?.Disconnect();
    }
}
