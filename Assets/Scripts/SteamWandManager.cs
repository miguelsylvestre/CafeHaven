using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SteamWandManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image milk;
    [SerializeField] private RectTransform milkT;
    [SerializeField] private RectTransform backCup;
    [SerializeField] private Image steamWand;
    [SerializeField] private Sprite steamWandNormal;
    [SerializeField] private Sprite steamWandOn;
    [SerializeField] private Button button;
    [SerializeField] private MilkCupContents contents;
    [SerializeField] private RectTransform zone;
    [SerializeField] private RectTransform frothLine;
    [SerializeField] private RectTransform frontCup;




    private const float time = 10f;

    private Vector2 startPosition;
    private RectTransform rectTransform;
    private bool isOnWand = false;
    private bool isSteaming = false;
    private float steamTimer = 0f;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        button.interactable = false;
        button.gameObject.SetActive(false);
        UpdateMilkHeight();
    }

    private void Update()
    {
        backCup.anchoredPosition = rectTransform.anchoredPosition;
        if (isSteaming)
        {
            steamTimer += Time.deltaTime;

            if (steamTimer >= time)
            {
                FinishSteaming();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSteaming || contents == null || contents.milk == null || contents.milk.steamed || contents.milk.frothed) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSteaming) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );

        rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSteaming) return;

        isOnWand = IsOverWand();
        if (isOnWand && contents != null && contents.milk.amount >= 75)
        {
            Vector2 currentPos = rectTransform.anchoredPosition;
            float snappedX = Mathf.Floor(currentPos.x) + 0.5f;
            float snappedY = Mathf.Floor(currentPos.y) + 0.5f;
            rectTransform.anchoredPosition = new Vector2(snappedX, snappedY);
            button.interactable = isOnWand && contents != null && contents.milk != null;
            button.gameObject.SetActive(button.interactable);
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
        
    }

    private bool IsOverWand()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.worldCamera;

        if (uiCamera == null)
        {
            return false;
        }
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, transform.position);
        return RectTransformUtility.RectangleContainsScreenPoint(zone, screenPoint, uiCamera);
    }

    private bool IsAboveFrothLine()
    {
        return rectTransform.anchoredPosition.y > frothLine.anchoredPosition.y;
    }

    public void OnSteamButtonClicked()
    {
        if (!isOnWand || isSteaming) return;
        if (contents == null || contents.milk == null) return;

        isSteaming = true;
        steamTimer = 0f;
        button.interactable = false;
        button.gameObject.SetActive(false);

        if (steamWand != null && steamWandOn != null)
            steamWand.sprite = steamWandOn;
    }

    private void FinishSteaming()
    {
        isSteaming = false;

        if (contents.milk != null)
        {
            if (IsAboveFrothLine())
            {
                contents.milk.frothed = true;
            }
            else
            {
                contents.milk.steamed = true;
            }
        }

        rectTransform.anchoredPosition = startPosition;
        isOnWand = false;

        if (steamWand != null && steamWandNormal != null)
            steamWand.sprite = steamWandNormal;
    }

    public void UpdateMilkHeight()
    {
        if (contents == null || contents.milk == null)
        {
            milk.gameObject.SetActive(false);
            return;
        }
        float pixelsToMoveDown = 25f - (0.12f * contents.milk.amount);
        milkT.anchoredPosition = new Vector2(milkT.anchoredPosition.x, 26.5f - pixelsToMoveDown);
    }
}