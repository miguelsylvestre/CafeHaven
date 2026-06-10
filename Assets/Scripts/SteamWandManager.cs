using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SteamWandManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform milkCupRect;
    [SerializeField] private MilkCupContents milkCupContents;
    [SerializeField] private RectTransform zone;
    [SerializeField] private RectTransform frothLine;
    [SerializeField] private Button steamButton;

    [SerializeField] private Image steamWandImage;
    [SerializeField] private Sprite wandNormal;
    [SerializeField] private Sprite wandOn;

    [SerializeField] private float heightMin;
    [SerializeField] private float heightMax;

    private const float time = 10f;

    private Vector2 startPosition;
    private bool isOnWand = false;
    private bool isSteaming = false;
    private float steamTimer = 0f;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        startPosition = milkCupRect.anchoredPosition;
        steamButton.onClick.AddListener(OnSteamButtonClicked);
        steamButton.interactable = false;
        UpdateCupHeight();
    }

    private void Update()
    {
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
        if (isSteaming) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSteaming) return;

        milkCupRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSteaming) return;

        Vector2 pos = milkCupRect.anchoredPosition;
        milkCupRect.anchoredPosition = new Vector2(
            Mathf.Round(pos.x),
            Mathf.Round(pos.y)
        );

        isOnWand = IsOverWand();
        steamButton.interactable = isOnWand && milkCupContents != null && milkCupContents.milk != null;
    }

    private bool IsOverWand()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            zone,
            RectTransformUtility.WorldToScreenPoint(null, milkCupRect.position),
            null
        );
    }

    private bool IsAboveFrothLine()
    {
        return milkCupRect.anchoredPosition.y > frothLine.anchoredPosition.y;
    }

    private void OnSteamButtonClicked()
    {
        if (!isOnWand || isSteaming) return;
        if (milkCupContents == null || milkCupContents.milk == null) return;

        isSteaming = true;
        steamTimer = 0f;
        steamButton.interactable = false;

        if (steamWandImage != null && wandOn != null)
            steamWandImage.sprite = wandOn;
    }

    private void FinishSteaming()
    {
        isSteaming = false;

        if (milkCupContents.milk != null)
        {
            if (IsAboveFrothLine())
            {
                milkCupContents.milk.frothed = true;
                milkCupContents.milk.steamed = false;
            }
            else
            {
                milkCupContents.milk.steamed = true;
                milkCupContents.milk.frothed = false;
            }
        }

        milkCupRect.anchoredPosition = startPosition;
        isOnWand = false;

        if (steamWandImage != null && wandNormal != null)
            steamWandImage.sprite = wandNormal;
    }

    private void UpdateCupHeight()
    {
        if (milkCupContents == null || milkCupContents.milk == null) return;

        const float STARTING_ML = 25f;
        const float ML_PER_PIXEL = 8.33333f;

        float pixels = Mathf.FloorToInt((milkCupContents.milk.amount - STARTING_ML) / ML_PER_PIXEL);
        float t = pixels / 22f;
        float y = Mathf.Lerp(heightMin, heightMax, t);
    }
}