using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input2DValueText : MonoBehaviour
{
    [SerializeField] DragObjectArea dragObjectArea = null;
    [SerializeField] Text text = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "x: "+dragObjectArea.NormalizePos.x.ToString("0.00") + " / y: " + dragObjectArea.NormalizePos.y.ToString("0.00");
    }
}
