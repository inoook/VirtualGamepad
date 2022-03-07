using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HSliderFill : MonoBehaviour
{
    [SerializeField] Slider slider = null;
    [SerializeField] Transform trans = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trans.localScale = new Vector3(slider.value, 1, 1);
    }
}
