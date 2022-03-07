using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleWithLabel : MonoBehaviour
{
    [SerializeField] Toggle toggle = null;

    public delegate void OnValueChangedHandler(bool v);
    public OnValueChangedHandler eventOnValueChanged;

    // Start is called before the first frame update
    void Start()
    {
        toggle.onValueChanged.AddListener((b) => {
            eventOnValueChanged?.Invoke(b);
        });
    }

    public bool Value
    {
        set { toggle.isOn = value; }
        get { return toggle.isOn; }
    }
}
