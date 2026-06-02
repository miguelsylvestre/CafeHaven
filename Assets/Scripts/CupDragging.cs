using UnityEngine;
using UnityEngine.EventSystems;

public class CupDragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    public Transform trans;
    float x;
    float y;
    Vector3 originalPosition;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start() {
        x = transform.position.x;
        y = transform.position.y;
        originalPosition = new Vector3(x, y, 0f);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;
        trans.position = originalPosition;
    }
}
