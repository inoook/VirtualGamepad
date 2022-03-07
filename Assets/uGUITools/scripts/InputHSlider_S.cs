using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHSlider_S : MonoBehaviour
{

    [SerializeField] Slider slider = null;

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

}
