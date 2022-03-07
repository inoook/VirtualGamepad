// https://qiita.com/ayumegu/items/c07594f408363f73008c
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// areaRectTrans の範囲で dragObjRectTrans をDrag操作
/// Drag object area.
/// </summary>
[RequireComponent(typeof(Image))]
public class DragObjectArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
	public delegate void DragHandler(Vector2 normalizePos);
	public event DragHandler eventDrag;

    [SerializeField] RectTransform dragObjRectTrans = null; // dragObj
	[SerializeField] RectTransform areaRectTrans = null;

	[SerializeField] Rect rect;

    [SerializeField] Canvas canvas = null;

    [SerializeField] public bool isDragX = true;
    [SerializeField] public bool isDragY = true;

    [SerializeField] Vector2 normalizePos = Vector2.zero;
    
    private void Awake() {
        rect = areaRectTrans.rect;

        if (canvas == null) {
            Graphic g = this.gameObject.GetComponent<Graphic>();
            canvas = g.canvas;
        }
        SetPickerPos(normalizePos);
    }

    public void SetPickerPos(Vector2 normalizePos_) {
        normalizePos = normalizePos_;
        Vector2 orgPos = dragObjRectTrans.anchoredPosition;
        Vector2 localPos = new Vector2(normalizePos.x * 0.5f * rect.width, normalizePos.y * 0.5f * rect.height);
        if (isDragX)
        {
            orgPos.x = localPos.x;
        }
        if (isDragY)
        {
            orgPos.y = localPos.y;
        }
        dragObjRectTrans.anchoredPosition = orgPos;
    }

    [SerializeField] bool enableDrag = true;
    
	public void OnBeginDrag(PointerEventData pointerEventData)
	{

	}

	public void OnEndDrag(PointerEventData pointerEventData)
	{

	}

    public void OnPointerDown(PointerEventData pointerEventData) {
        PointerEventDataAct(pointerEventData);
    }

	public void OnDrag(PointerEventData pointerEventData)
	{
        PointerEventDataAct(pointerEventData);
    }

    void PointerEventDataAct(PointerEventData pointerEventData) {
        if (!enableDrag) { return; }

        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(areaRectTrans, pointerEventData.position, canvas.worldCamera, out localPos);
        Vector2 pos = dragObjRectTrans.anchoredPosition;
        if (isDragX)
        {
            pos.x = Mathf.Clamp(localPos.x, rect.x, rect.width + rect.x);
        }
        if (isDragY)
        {
            pos.y = Mathf.Clamp(localPos.y, rect.y, rect.height + rect.y);
        }

        dragObjRectTrans.anchoredPosition = pos;
        normalizePos = new Vector2(dragObjRectTrans.anchoredPosition.x / rect.width,  dragObjRectTrans.anchoredPosition.y / rect.height) * 2f;

        eventDrag?.Invoke(normalizePos);
    }

    /// <summary>
    /// 0 ~ 1
    /// </summary>
    public Vector2 NormalizePos {
        get { return normalizePos; }
    }

    public void SetPickerScale(float scale)
    {
        dragObjRectTrans.localScale = Vector3.one * scale;
    }
}
