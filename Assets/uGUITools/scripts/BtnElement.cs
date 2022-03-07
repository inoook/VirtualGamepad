using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnElement : MonoBehaviour
{
    [SerializeField] string label = "name";
    [SerializeField] Button btn = null;
    [SerializeField] Text btnLabel = null;
    [SerializeField]  public int id = -1;

    [SerializeField] EventTrigger eventTrigger = null;

    public delegate void BtnEventHandler(BtnElement btn);
    public BtnEventHandler eventOnClick = null;
    public BtnEventHandler eventOnPress = null;
    public BtnEventHandler eventOnRelease = null;


    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() => {
            eventOnClick?.Invoke(this);
        });
        foreach ( var t in eventTrigger.triggers)
        {
            if(t.eventID == EventTriggerType.PointerDown)
            {
                t.callback.AddListener((e) => {
                    eventOnPress?.Invoke(this);
                });
            }else if(t.eventID == EventTriggerType.PointerUp)
            {
                t.callback.AddListener((e) => {
                    eventOnRelease?.Invoke(this);
                });
            }
        }
    }

    public void SetLabel(string str)
    {
        label = str ;
        if (btnLabel != null)
        {
            btnLabel.text = label;
        }
        this.gameObject.name = str;
    }

    [ContextMenu("Apply")]
    void ApplyLabel()
    {
        string str = label + "_Button";
        if (btnLabel != null)
        {
            btnLabel.text = label;
        }
        //this.gameObject.name = str;
    }

    private void OnValidate()
    {
        ApplyLabel();
    }

    public Color btnColor
    {
        set { btn.image.color = value; }
    }

    //
    [ContextMenu("SetPointrDown")]
    public void SetPointrDown()
    {
        btn.OnPointerDown(new UnityEngine.EventSystems.PointerEventData(null));
    }
    public void SetPointrUp()
    {
        btn.OnPointerUp(new UnityEngine.EventSystems.PointerEventData(null));
    }

    public void Press(bool press)
    {
        if (press)
        {
            SetPointrDown();
        }
        else
        {
            SetPointrUp();
        }
    }

    Color defaultColor = new Color(0.1960784f, 0.1960784f, 0.1960784f);

    public void EnableBtn(bool enable)
    {
        btn.GetComponent<Image>().color = enable ? Color.red : defaultColor;
    }
}
