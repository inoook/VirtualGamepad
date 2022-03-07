using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliderValutText : MonoBehaviour
{
    [SerializeField] Text valueText = null;
    [SerializeField] Slider slider = null;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(UpdateValue);
    }

    public void UpdateValue(float v)
    {
        valueText.text = Mathf.Round(v*100).ToString();
    }
}
