using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

public class ViGEmXboxGamepad : ViGEmGamePad, IViGEmGamePad
{
    ViGEmClient viGEmClient = null;

    //[SerializeField] public Direction direction = Direction.None;

    //public enum Direction
    //{
    //    None, Northwest, West, Southwest, South, Southeast, East, Northeast, North
    //}
    public static Dictionary<Direction, bool[]> directionDic = new Dictionary<Direction, bool[]>()
    {
        // left,  up, right ,down
        { Direction.None, new bool[4]{false, false,false, false} },
        { Direction.Northwest, new bool[4]{true, true, false, false}},
        { Direction.West, new bool[4]{true, false,false, false}},
        { Direction.Southwest, new bool[4]{true, false, false, true}},
        { Direction.South, new bool[4]{false, false,false, true}},
        { Direction.Southeast, new bool[4]{false, false, true, true}},
        { Direction.East, new bool[4]{false, false,true, false}},
        { Direction.Northeast, new bool[4]{false, true,true, false}},
        { Direction.North, new bool[4]{false, true,false, false}}
    };

    [SerializeField] byte ledNumber;

    IXbox360Controller xboxController = null;
    // Start is called before the first frame update
    void Start()
    {
        viGEmClient = new ViGEmClient();

        xboxController = viGEmClient.CreateXbox360Controller();
        xboxController.Connect();
        xboxController.FeedbackReceived += Controller_FeedbackReceived;

    }
    private void Controller_FeedbackReceived(object sender, Xbox360FeedbackReceivedEventArgs e)
    {
        ledNumber = e.LedNumber;
        largeMotor = e.LargeMotor;
        smallMotor = e.SmallMotor;
    }

    public override void OnLeftThumb(Vector2 v) {
        xboxController.SetAxisValue(Xbox360Axis.LeftThumbX, (short)(v.x * 32767));
        xboxController.SetAxisValue(Xbox360Axis.LeftThumbY, (short)(v.y * 32767));
    }
    public override void OnRightThumb(Vector2 v) {
        xboxController.SetAxisValue(Xbox360Axis.RightThumbX, (short)(v.x * 32767));
        xboxController.SetAxisValue(Xbox360Axis.RightThumbY, (short)(v.y * 32767));
    }
    public override void OnLeftTrigger(float v) {
        xboxController.SetSliderValue(Xbox360Slider.LeftTrigger, (byte)(leftTrigger * 255));// brake
    }
    public override void OnRightTrigger(float v) {
        xboxController.SetSliderValue(Xbox360Slider.RightTrigger, (byte)(v * 255)); // accel
    }
    //
    public override void OnBtnA(bool press) {
        btnA = press;
        xboxController.SetButtonState(Xbox360Button.A, press);
    }
    public override void OnBtnB(bool press) {
        btnB = press;
        xboxController.SetButtonState(Xbox360Button.B, press);
    }
    public override void OnBtnX(bool press) {
        btnX = press;
        xboxController.SetButtonState(Xbox360Button.X, press);
    }
    public override void OnBtnY(bool press) {
        btnY = press;
        xboxController.SetButtonState(Xbox360Button.Y, press);
    }
    public override void OnBtnShoulderL(bool press) {
        btnShoulderL = press;
        xboxController.SetButtonState(Xbox360Button.LeftShoulder, press);
    }
    public override void OnBtnShoulderR(bool press) {
        btnShoulderR = press;
        xboxController.SetButtonState(Xbox360Button.RightShoulder, press);
    }
    //
    public override void OnBtnGuide(bool press) {
        btn6 = press;
        //xboxController.SetButtonState(Xbox360Button.Guide, press);
        xboxController.SetButtonState(Xbox360Button.Back, press);
    }
    public override void OnBtnStart(bool press) {
        btn7 = press;
        xboxController.SetButtonState(Xbox360Button.Start, press);
    }
    public override void OnBtnLeftThumb(bool press) {
        btn8 = press;
        xboxController.SetButtonState(Xbox360Button.LeftThumb, press);
    }
    public override void OnBtnRightThumb(bool press) {
        btn9 = press;
        xboxController.SetButtonState(Xbox360Button.RightThumb, press);
    }
    //
    public override void OnBtnPov(ViGEmGamePad.Direction direction)
    {
        bool[] bools = directionDic[direction];
        xboxController.SetButtonState(Xbox360Button.Left, bools[0]);
        xboxController.SetButtonState(Xbox360Button.Up, bools[1]);
        xboxController.SetButtonState(Xbox360Button.Right, bools[2]);
        xboxController.SetButtonState(Xbox360Button.Down, bools[3]);
    }

    private void OnDisable()
    {
        xboxController?.Disconnect();

        viGEmClient.Dispose();
    }
}
