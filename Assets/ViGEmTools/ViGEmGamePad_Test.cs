using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ViGEmを使った仮想ゲームパッド入力
/// https://vigem.org/
/// https://github.com/ViGEm/ViGEm.NET
/// </summary>
public class ViGEmGamePad_Test : MonoBehaviour
{
    [SerializeField] ViGEmGamePad gamePad = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v = Time.time;
        gamePad.LeftThumb = new Vector2( Mathf.Sin(v), Mathf.Cos(v)) ;
        gamePad.RightThumb = new Vector2( -Mathf.Cos(v), -Mathf.Sin(v));

        gamePad.RightTrigger = (Mathf.Sin(v) + 1) / 2f;

        gamePad.BtnA = Mathf.Abs(Mathf.Sin(v)) > 0.5f;
        //
        vx = Input.GetAxisRaw("Horizontal");
        vy = Input.GetAxisRaw("Vertical");
    }
    float vx = 0;
    float vy = 0;

    [SerializeField] Rect drawRect = new Rect(10,10,200,200);
    private void OnGUI()
    {
        GUILayout.BeginArea(drawRect);
        GUILayout.Label("UnityのInputでのvirtualGamePad入力の表示");
        GUILayout.HorizontalSlider(vx, -1, 1);
        GUILayout.HorizontalSlider(vy, -1, 1);
        GUILayout.Label($"vibration: {gamePad.SmallMotor} / {gamePad.LargeMotor} ");
        GUILayout.EndArea();
    }
}
