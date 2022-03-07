using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// xbox, PS4 共に扱いやすくするための基底class
/// 値の更新時に event を発生。UIなどへの反映に使用する
/// また、値の更新時の event 発生と同じタイミングで OnXXX が発生する。
/// OnXXX を overide し、それぞれのコントローラーの入力を行う。
/// </summary>
public class ViGEmGamePad : MonoBehaviour
{
    public enum Direction
    {
        None, Northwest, West, Southwest, South, Southeast, East, Northeast, North
    }

    // thumb
    public delegate void UpdateThumbHandler(Vector2 v);
    public event UpdateThumbHandler eventUpdateLeftThumb;
    public event UpdateThumbHandler eventUpdateRightThumb;

    public delegate void UpdateTriggerHandler(float v);
    public event UpdateTriggerHandler eventUpdateLeftTrigger;
    public event UpdateTriggerHandler eventUpdateRightTrigger;

    [Header("leftThumb")]
    [SerializeField, Range(-1, 1)] protected float leftThumbX = 0;
    [SerializeField, Range(-1, 1)] protected float leftThumbY = 0;

    [Header("rightThumb")]
    [SerializeField, Range(-1, 1)] protected float rightThumbX = 0;
    [SerializeField, Range(-1, 1)] protected float rightThumbY = 0;

    [Header("trigger")]
    [SerializeField, Range(0, 1)] protected float leftTrigger = 0;
    [SerializeField, Range(0, 1)] protected float rightTrigger = 0;

    public virtual void OnLeftThumb(Vector2 v) { }
    public virtual void OnRightThumb(Vector2 v) { }
    public virtual void OnLeftTrigger(float v) { }
    public virtual void OnRightTrigger(float v) { }

    public Vector2 LeftThumb
    {
        set {
            leftThumbX = value.x;
            leftThumbY = value.y;
            OnLeftThumb(value);
            eventUpdateLeftThumb?.Invoke(value);
        }
        get => new Vector2(leftThumbX, leftThumbY);
    }
    public Vector2 RightThumb
    {
        set {
            rightThumbX = value.x;
            rightThumbY = value.y;
            OnRightThumb(value);
            eventUpdateRightThumb?.Invoke(value);
        }
        get => new Vector2(rightThumbX, rightThumbY);
    }

    public float LeftTrigger
    {
        set {
            float v = value;
            leftTrigger = v;
            OnLeftTrigger(v);
            eventUpdateLeftTrigger?.Invoke(v);
        }
        get => leftTrigger;
    }
    public float RightTrigger
    {
        set {
            float v = value;
            rightTrigger = v;
            OnRightTrigger(v);
            eventUpdateRightTrigger?.Invoke(v);
        }
        get => rightTrigger;
    }

    // -----
    // btn
    public delegate void UpdateBtnHandler(bool v);

    public event UpdateBtnHandler eventBtnA;
    public event UpdateBtnHandler eventBtnB;
    public event UpdateBtnHandler eventBtnX;
    public event UpdateBtnHandler eventBtnY;

    public event UpdateBtnHandler eventBtnSholderL;
    public event UpdateBtnHandler eventBtnSholderR;

    public event UpdateBtnHandler eventBtnGuide;
    public event UpdateBtnHandler eventBtnStart;

    public event UpdateBtnHandler eventBtnPad;

    public event UpdateBtnHandler eventBtnLeftThumb;
    public event UpdateBtnHandler eventBtnRightThumb;

    //public event UpdateBtnHandler eventBtnPovUp;
    //public event UpdateBtnHandler eventBtnPovDown;
    //public event UpdateBtnHandler eventBtnPovLeft;
    //public event UpdateBtnHandler eventBtnPovRight;

    [SerializeField] protected bool btnA = false;
    [SerializeField] protected bool btnB = false;
    [SerializeField] protected bool btnX = false;
    [SerializeField] protected bool btnY = false;
    [SerializeField] protected bool btnShoulderL = false;
    [SerializeField] protected bool btnShoulderR = false;
    [SerializeField] protected bool btnGuide = false;
    [SerializeField] protected bool btnStart = false;
    [SerializeField] protected bool btnLeftThumb = false;
    [SerializeField] protected bool btnRightThumb = false;
    [SerializeField] protected bool btnPad = false;
    [SerializeField] protected bool btn6 = false;
    [SerializeField] protected bool btn7 = false;
    [SerializeField] protected bool btn8 = false;
    [SerializeField] protected bool btn9 = false;
    [SerializeField] protected bool btn10 = false;

    // 値変更時
    public virtual void OnBtnA(bool press) { }
    public virtual void OnBtnB(bool press) { }
    public virtual void OnBtnX(bool press) { }
    public virtual void OnBtnY(bool press) { }
    public virtual void OnBtnShoulderL(bool press) { }
    public virtual void OnBtnShoulderR(bool press) { }
    //
    public virtual void OnBtnGuide(bool press) { }
    public virtual void OnBtnStart(bool press) { }
    public virtual void OnBtnPad(bool press) { }
    public virtual void OnBtnLeftThumb(bool press) { }
    public virtual void OnBtnRightThumb(bool press) { }

    public bool BtnA
    {
        set {
            if (btnA != value)
            {
                btnA = value;
                OnBtnA(value);
                eventBtnA?.Invoke(value);
            }
        }
        get => btnA;
    }
    public bool BtnB
    {
        set {
            if (btnB != value)
            {
                btnB = value;
                OnBtnB(value);
                eventBtnB?.Invoke(value);
            }
        }
        get => btnB;
    }
    public bool BtnX
    {
        set {
            if (btnX != value)
            {
                btnX = value;
                OnBtnX(value);
                eventBtnX?.Invoke(value);
            }
        }
        get => btnX;
    }
    public bool BtnY
    {
        set {
            if (btnY != value)
            {
                btnY = value;
                OnBtnY(value);
                eventBtnY?.Invoke(value);
            }
        }
        get => btnY;
    }
    //
    public bool BtnShoulderL
    {
        set {
            if (btnShoulderL != value)
            {
                btnShoulderL = value;
                OnBtnShoulderL(value);
                eventBtnSholderL?.Invoke(value);
            }
        }
        get => btnShoulderL;
    }
    public bool BtnShoulderR
    {
        set {
            if (btnShoulderR != value)
            {
                btnShoulderR = value;
                OnBtnShoulderR(value);
                eventBtnSholderR?.Invoke(value);
            }
        }
        get => btnShoulderR;
    }
    // 
    // -----
    public bool BtnGuide
    {
        set {
            if (btnGuide != value)
            {
                btnGuide = value;
                OnBtnGuide(value);
                eventBtnGuide?.Invoke(value);
            }
        }
        get => btnGuide;
    }
    public bool BtnStart
    {
        set {
            if (btnStart != value)
            {
                btnStart = value;
                OnBtnStart(value);
                eventBtnStart?.Invoke(value);
            }
        }
        get => btnStart;
    }
    //
    public bool BtnLeftThumb
    {
        set {
            if (btnLeftThumb != value)
            {
                btnLeftThumb = value;
                OnBtnLeftThumb(value);
                eventBtnLeftThumb?.Invoke(value);
            }
        }
        get => btnLeftThumb;
    }
    public bool BtnRightThumb
    {
        set {
            if (btnRightThumb != value)
            {
                btnRightThumb = value;
                OnBtnRightThumb(value);
                eventBtnRightThumb?.Invoke(value);
            }
        }
        get => btnRightThumb;
    }

    public bool BtnPad
    {
        set {
            if (btnPad != value)
            {
                btnPad = value;
                OnBtnPad(value);
                eventBtnPad?.Invoke(value);
            }
        }
        get => BtnPad;
    }
    //

    // -----
    // pov
    [SerializeField] protected Direction povDirection = Direction.None;

    public delegate void UpdatePovHandler(Direction dir);
    public event UpdatePovHandler eventUpdatePov;

    public virtual void OnBtnPov(Direction direction) { }

    public Direction BtnPovDirction
    {
        set {
            if (povDirection != value)
            {
                povDirection = value;
                OnBtnPov(value);
                eventUpdatePov?.Invoke(value);
            }
        }
        get => povDirection;
    }
    
    //
    [SerializeField] protected byte largeMotor;
    [SerializeField] protected byte smallMotor;

    public float LargeMotor
    {
        get { return ((float)largeMotor) / 255; }
    }
    public float SmallMotor
    {
        get { return ((float)smallMotor) / 255; }
    }

    public byte LargeMotorByte
    {
        get { return largeMotor; }
    }
    public byte SmallMotorByte
    {
        get { return smallMotor; }
    }

    ////
    //[SerializeField] bool useUpdateValue = false;

    //public virtual void ForceUpdate()
    //{
    //    LeftThumb = new Vector2(leftThumbX, leftThumbY);
    //    RightThumb = new Vector2(rightThumbX, rightThumbY);
    //    RightTrigger = rightTrigger;
    //    LeftTrigger = leftTrigger;

    //    BtnA = btnA;
    //    BtnB = btnB;
    //    BtnX = btnX;
    //    BtnY = btnY;
    //    // TODO: 更新の必要なもの追加
    //}
    //private void Update()
    //{
    //    if (useUpdateValue)
    //    {
    //        // イベントではなくこのMonoで値を更新するとき
    //        ForceUpdate();
    //    }
    //}
}
