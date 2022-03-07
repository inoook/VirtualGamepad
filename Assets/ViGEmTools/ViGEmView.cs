using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViGEmView : MonoBehaviour
{
    [SerializeField] ViGEmGamePad viGEmGamePad = null;

    [Header("vibration")]
    [SerializeField] Image smallMotorImage = null;
    [SerializeField] Image largeMotorImage = null;
    [SerializeField] Text motorText = null;

    [Header("input")]
    [SerializeField] DragObjectArea leftStickInput = null;
    [SerializeField] DragObjectArea rightStickInput = null;

    [SerializeField] Slider leftTriggerSlider = null;
    [SerializeField] Slider rightTriggerSlider = null;

    [Header("btns")]
    [SerializeField] BtnElement a_button = null;
    [SerializeField] BtnElement b_button = null;
    [SerializeField] BtnElement x_button = null;
    [SerializeField] BtnElement y_button = null;

    [Header("Pov")]
    [SerializeField] BtnElement d_button = null;
    [SerializeField] BtnElement r_button = null;
    [SerializeField] BtnElement u_button = null;
    [SerializeField] BtnElement l_button = null;

    [SerializeField] BtnElement l_shoulder_button = null;
    [SerializeField] BtnElement r_shoulder_button = null;

    [SerializeField] BtnElement l_thumb_button = null;
    [SerializeField] BtnElement r_thumb_button = null;

    [SerializeField] BtnElement guide_button = null;
    [SerializeField] BtnElement start_button = null;

    void Start()
    {
        Debug.LogWarning("ViGEmView.Start");
        if(viGEmGamePad != null)
        {
            SetViGEmGamePad(viGEmGamePad);
        }
    }

    // Start is called before the first frame update
    public void SetViGEmGamePad(ViGEmGamePad viGEmGamePad_)
    {
        viGEmGamePad = viGEmGamePad_;
        // -----
        // LeftThumb
        leftStickInput.eventDrag += (Vector2 normalizePos) =>
        {
            viGEmGamePad.LeftThumb = normalizePos;
        };
        viGEmGamePad.eventUpdateLeftThumb += (Vector2 v) =>
        {
            leftStickInput.SetPickerPos(v);
        };
   
        // -----
        // RightThumb
        rightStickInput.eventDrag += (Vector2 normalizePos) =>
        {
            viGEmGamePad.RightThumb = normalizePos;
        };
        viGEmGamePad.eventUpdateRightThumb += (Vector2 v) =>
        {
            rightStickInput.SetPickerPos(v);
        };

        // -----
        // LeftSlider
        leftTriggerSlider.onValueChanged.AddListener((v) => {
            viGEmGamePad.LeftTrigger = v;
        });
        viGEmGamePad.eventUpdateLeftTrigger += (float v) =>
        {
            leftTriggerSlider.value = v;
        };
        // RightSlider
        rightTriggerSlider.onValueChanged.AddListener((v) => {
            viGEmGamePad.RightTrigger = v;
        });
        viGEmGamePad.eventUpdateRightTrigger += (float v) =>
        {
            rightTriggerSlider.value = v;
        };
        //
        // -----
        // POV
        viGEmGamePad.eventUpdatePov += (ViGEmGamePad.Direction dir) =>
        {
            if (dir == ViGEmGamePad.Direction.East)
            {
                r_button.Press(true);
                l_button.Press(false);
                u_button.Press(false);
                d_button.Press(false);
            }
            else if (dir == ViGEmGamePad.Direction.West)
            {
                r_button.Press(false);
                l_button.Press(true);
                u_button.Press(false);
                d_button.Press(false);
            }
            else if (dir == ViGEmGamePad.Direction.North)
            {
                r_button.Press(false);
                l_button.Press(false);
                u_button.Press(true);
                d_button.Press(false);
            }
            else if (dir == ViGEmGamePad.Direction.South)
            {
                r_button.Press(false);
                l_button.Press(false);
                u_button.Press(false);
                d_button.Press(true);
            }
            else
            {
                r_button.Press(false);
                l_button.Press(false);
                u_button.Press(false);
                d_button.Press(false);
            }
        };
        //SetBtn(start_button, (b) =>
        //{
        //    viGEmGamePad.BtnStart = b;
        //});
        //viGEmGamePad.eventBtnStart += (bool v) =>
        //{
        //    start_button.Press(v);
        //};

        // -----
        // A
        //a_button.eventOnPress += (e) => {
        //    viGEmGamePad.BtnA = true;
        //};
        //a_button.eventOnRelease += (e) => {
        //    viGEmGamePad.BtnA = false;
        //};
        //viGEmGamePad.eventBtnA += (bool v) => {
        //    a_button.Press(v);
        //};
        SetBtn(a_button, (b) =>
        {
            viGEmGamePad.BtnA = b;
        });
        viGEmGamePad.eventBtnA += (bool v) =>
        {
            a_button.Press(v);
        };
        // B
        SetBtn(b_button, (b) =>
        {
            viGEmGamePad.BtnB = b;
        });
        viGEmGamePad.eventBtnB += (bool v) =>
        {
            b_button.Press(v);
        };
        // X
        SetBtn(x_button, (b) =>
        {
            viGEmGamePad.BtnX = b;
        });
        viGEmGamePad.eventBtnX += (bool v) =>
        {
            x_button.Press(v);
        };
        // Y
        SetBtn(y_button, (b) =>
        {
            viGEmGamePad.BtnY = b;
        });
        viGEmGamePad.eventBtnY += (bool v) =>
        {
            y_button.Press(v);
        };
        // -----
        //
        SetBtn(l_shoulder_button, (b) =>
        {
            viGEmGamePad.BtnShoulderL = b;
        });
        viGEmGamePad.eventBtnSholderL += (bool v) =>
        {
            l_shoulder_button.Press(v);
        };
        //
        SetBtn(r_shoulder_button, (b) =>
        {
            viGEmGamePad.BtnShoulderR = b;
        });
        viGEmGamePad.eventBtnSholderR += (bool v) =>
        {
            r_shoulder_button.Press(v);
        };

        //
        // LeftThumb Btn
        SetBtn(l_thumb_button, (b) =>
        {
            viGEmGamePad.BtnLeftThumb = b;
        });
        viGEmGamePad.eventBtnLeftThumb += (bool v) =>
        {
            l_thumb_button.Press(v);
        };
        // RightThumb Btn
        SetBtn(r_thumb_button, (b) =>
        {
            viGEmGamePad.BtnRightThumb = b;
        });
        viGEmGamePad.eventBtnRightThumb += (bool v) =>
        {
            r_thumb_button.Press(v);
        };

        //
        // Guide
        SetBtn(guide_button, (b) =>
        {
            viGEmGamePad.BtnGuide = b;
        });
        viGEmGamePad.eventBtnGuide += (bool v) =>
        {
            guide_button.Press(v);
        };
        // Start
        SetBtn(start_button, (b) =>
        {
            viGEmGamePad.BtnStart = b;
        });
        viGEmGamePad.eventBtnStart += (bool v) =>
        {
            start_button.Press(v);
        };
    }

    void SetBtn(BtnElement btn, System.Action<bool> btnEvent)
    {
        //GUI 上の btn 押した時
        btn.eventOnPress += (e) => {
            btnEvent(true);
        };
        btn.eventOnRelease += (e) => {
            btnEvent(false);
        };
    }


    // Update is called once per frame
    void Update()
    {
        // 振動の表示
        smallMotorImage.transform.localScale = Vector3.one * (1+ viGEmGamePad.SmallMotor);
        largeMotorImage.transform.localScale = Vector3.one * (1+ viGEmGamePad.LargeMotor);
        //
        motorText.text = $"small: {viGEmGamePad.SmallMotor} / large: {viGEmGamePad.LargeMotor}";
    }

    public bool IsVisible => this.gameObject.activeSelf;
    public void EnableVisible(bool enable)
    {
        if(enable == IsVisible) { return; }

        this.gameObject.SetActive(enable);
    }

    public bool IsStickVisible => leftStickInput.gameObject.activeSelf;

    public void EnableStickVisible(bool enable)
    {
        if (enable == IsStickVisible) { return; }

        leftStickInput.gameObject.SetActive(enable);
        rightStickInput.gameObject.SetActive(enable);
    }
}
