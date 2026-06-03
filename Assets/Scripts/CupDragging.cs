using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CupDragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject cupDragLeft;
    public GameObject cupDragRight;

    public Sprite hoverImg;
    public Sprite dropImg;
    public Vector2 dropResultSize;

    private Sprite originalDragSprite;
    private Sprite originalHoverTargetSprite;

    private GameObject currentlyHovered;

    private Canvas canvas;
    private GraphicRaycaster raycaster;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = GetComponent<RectTransform>().position;

        originalDragSprite = GetComponent<Image>().sprite;

        canvas = GetComponentInParent<Canvas>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Image img = GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );

        rectTransform.localPosition = localPoint;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        GameObject newHover = null;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == cupDragLeft)
            {
                CupDropSlot slot = cupDragLeft.GetComponent<CupDropSlot>();

                if (!slot.occupied)
                {
                    newHover = cupDragLeft;
                }

                break;
            }

            if (result.gameObject == cupDragRight)
            {
                CupDropSlot slot = cupDragRight.GetComponent<CupDropSlot>();

                if (!slot.occupied)
                {
                    newHover = cupDragRight;
                }

                break;
            }
        }

        if (newHover != currentlyHovered)
        {
            Image dragImage = GetComponent<Image>();
            if (currentlyHovered != null)
            {
                CupDropSlot slot = currentlyHovered.GetComponent<CupDropSlot>();

                if (!slot.occupied)
                {
                    Image prevImage = currentlyHovered.GetComponent<Image>();

                    prevImage.sprite = originalHoverTargetSprite;

                    prevImage.color = new Color(prevImage.color.r, prevImage.color.g, prevImage.color.b, 0f);
                    dragImage.color = new Color( dragImage.color.r, dragImage.color.g, dragImage.color.b, 1f);
                }
            }

            if (newHover != null)
            {
                Image targetImage = newHover.GetComponent<Image>();

                originalHoverTargetSprite = targetImage.sprite;

                targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 1f);

                if (hoverImg != null)
                    targetImage.sprite = hoverImg;
                    dragImage.color = new Color( dragImage.color.r, dragImage.color.g, dragImage.color.b, 0f);
            }
            currentlyHovered = newHover;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Image dragImage = GetComponent<Image>();

        if (currentlyHovered != null)
        {
            CupDropSlot slot = currentlyHovered.GetComponent<CupDropSlot>();
            if (!slot.occupied)
            {
                Image targetImage = currentlyHovered.GetComponent<Image>();
                if (dropImg != null)
                    targetImage.sprite = dropImg;
                targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 1f);
                slot.occupied = true;
            }
        }

        dragImage.color = new Color( dragImage.color.r, dragImage.color.g, dragImage.color.b, 0f);
        dragImage.raycastTarget = true;
        GetComponent<RectTransform>().position = originalPosition;
        currentlyHovered = null;
    }
}