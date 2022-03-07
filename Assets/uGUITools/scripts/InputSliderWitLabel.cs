using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputSliderWitLabel : MonoBehaviour
{
    [SerializeField] string label = "label";
    [SerializeField] Slider slider = null;

    [SerializeField] Text labelText = null;
    [SerializeField] Text valueText = null;

    public delegate void OnChangeValue(float v);
    public event OnChangeValue eventOnChangeValue;

    public float Value
    {
        set {
            slider.value = value;
            valueText.text = slider.value.ToString("0.00");
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        slider.onValueChanged.AddListener((v) => {
            valueText.text = v.ToString("0.00");

            eventOnChangeValue?.Invoke(v);
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
}
