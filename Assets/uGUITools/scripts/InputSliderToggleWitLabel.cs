using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputSliderToggleWitLabel : MonoBehaviour
{
    [SerializeField] string label = "label";
    [SerializeField] Slider slider = null;

    [SerializeField] Text labelText = null;
    [SerializeField] Text valueText = null;

    public delegate void OnChangeValue(float v, bool b);
    public event OnChangeValue eventOnChangeValue;

    public float Value
    {
        set {
            slider.value = value;
            valueText.text = slider.value.ToString("0.00");
        }
    }

    public void SetValue(float v, bool b)
    {
        Value = v;
        ToggleValue = b;
    }

    // Start is called before the first frame update
    void Awake()
    {
        slider.onValueChanged.AddListener((v) => {
            valueText.text = v.ToString("0.00");

            eventOnChangeValue?.Invoke(v, ToggleValue);
        });

        toggle.onValueChanged.AddListener((b) => {

            eventOnChangeValue?.Invoke(slider.value, b);
        });
    }

    [ContextMenu("Apply")]
    void ApplyLabel()
    {
        if (labelText != null)
        {
            labelText.text = label;
        }
    }

    private void OnValidate()
    {
        ApplyLabel();
    }
    [SerializeField] Toggle toggle = null;

    public bool ToggleValue
    {
        set { toggle.isOn = value; }
        get { return toggle.isOn; }
    }
}
