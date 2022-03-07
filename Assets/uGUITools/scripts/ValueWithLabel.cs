using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueWithLabel : MonoBehaviour
{
    [SerializeField] Text valueText = null;

    public void SetValue(float v)
    {
        valueText.text = v.ToString("0.0000");
    }
    public void SetValue(string str)
    {
        valueText.text = str;
    }
}
