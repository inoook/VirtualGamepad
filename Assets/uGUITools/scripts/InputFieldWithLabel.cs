using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldWithLabel : MonoBehaviour
{
    [SerializeField] InputField inputField = null;

    public delegate void OnValueChangedHandler(float v);
    public OnValueChangedHandler eventOnValueChanged;

    [SerializeField] float currentV = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputField.onValueChanged.AddListener((str) => {
            if (!string.IsNullOrEmpty(str))
            {
                currentV = float.Parse(str);
                eventOnValueChanged?.Invoke(currentV);
            }
        });
    }

    public float Value
    {
        set {
            currentV = value;
            inputField.text = currentV.ToString();
        }
        get { return currentV; }
    }
}
