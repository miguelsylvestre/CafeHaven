using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MilkPouring : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] private Sprite[] startFrames;
    [SerializeField] private Sprite[] pourFrames; 
    [SerializeField] private Sprite[] endFrames;  

    [SerializeField] private Image pourAnimationImage;
    [SerializeField] private Image milkCartonImage;
    [SerializeField] private Sprite milkCartonDefault;   
    [SerializeField] private Sprite milkCartonPouring;   

    [SerializeField] private RectTransform milkFillImage;
    [SerializeField] private CupContents currentCup;

    private Milk currentMilk;

    private const float FPS = 4f;
    private const float FRAME_TIME = 1f / FPS;

    private const float START_FILL_TIME = 3f;
    private const float ML_PER_PIXEL = 8.33333f;
    private const float STARTING_ML = 25f;
    private const float MAX_ML = 208.33333f;

    private float frameTimer;
    private int frameIndex;

    private float pourTime;
    private int pixelsFilled;
    private int pixelsFilledAtSessionStart;

    private Vector2 originalMilkPos;

    private enum PourState
    {
        Idle,
        Starting,
        Pouring,
        Ending
    }

    private PourState state = PourState.Idle;

    private void Start()
    {
        originalMilkPos = milkFillImage.anchoredPosition;

        if (milkCartonImage == null)
            milkCartonImage = GetComponent<Image>();

        currentMilk = null;
        pixelsFilled = 0;
        pixelsFilledAtSessionStart = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state != PourState.Idle)
            return;

        if (currentMilk == null)
        {
            currentMilk = new Milk
            {
                cold = true,
                steamed = false,
                frothed = false,
                amount = 0
            };
            pixelsFilled = 0;
            milkFillImage.anchoredPosition = originalMilkPos;
        }

        pourTime = 0f;
        pixelsFilledAtSessionStart = pixelsFilled;

        if (milkCartonImage != null && milkCartonPouring != null)
            milkCartonImage.sprite = milkCartonPouring;

        state = PourState.Starting;
        frameIndex = 0;
        frameTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (state == PourState.Starting || state == PourState.Pouring)
        {
            BeginEnding();
        }
    }

    private void Update()
    {
        switch (state)
        {
            case PourState.Starting:
                UpdateStarting();
                break;

            case PourState.Pouring:
                UpdatePouring();
                break;

            case PourState.Ending:
                UpdateEnding();
                break;
        }
    }

    private void UpdateStarting()
    {
        frameTimer += Time.deltaTime;

        if (frameTimer >= FRAME_TIME)
        {
            frameTimer -= FRAME_TIME;

            if (frameIndex < startFrames.Length)
            {
                pourAnimationImage.sprite = startFrames[frameIndex];
                frameIndex++;
            }

            if (frameIndex >= startFrames.Length)
            {
                state = PourState.Pouring;
                frameIndex = 0;
            }
        }
    }

    private void UpdatePouring()
    {
        frameTimer += Time.deltaTime;
        pourTime += Time.deltaTime;

        if (frameTimer >= FRAME_TIME)
        {
            frameTimer -= FRAME_TIME;

            pourAnimationImage.sprite = pourFrames[frameIndex];

            frameIndex++;
            if (frameIndex >= pourFrames.Length)
                frameIndex = 0;
        }

        int newPixels = Mathf.FloorToInt(pourTime / 0.5f);
        int desiredPixels = pixelsFilledAtSessionStart + newPixels;

        if (desiredPixels > pixelsFilled)
        {
            pixelsFilled = desiredPixels;

            currentMilk.amount = STARTING_ML + (pixelsFilled * ML_PER_PIXEL);

            if (currentMilk.amount > MAX_ML)
                currentMilk.amount = MAX_ML;

            milkFillImage.anchoredPosition =
                originalMilkPos + Vector2.up * pixelsFilled;
        }

        if (currentMilk.amount >= MAX_ML)
        {
            currentMilk.amount = MAX_ML;
            BeginEnding();
        }
    }

    private void UpdateEnding()
    {
        frameTimer += Time.deltaTime;

        if (frameTimer >= FRAME_TIME)
        {
            frameTimer -= FRAME_TIME;
            frameIndex++;

            if (frameIndex < endFrames.Length)
            {
                pourAnimationImage.sprite = endFrames[frameIndex];
            }
            else
            {
                state = PourState.Idle;
                frameIndex = 0;

                if (milkCartonImage != null && milkCartonDefault != null)
                    milkCartonImage.sprite = milkCartonDefault;

                if (currentCup != null)
                    currentCup.milk = currentMilk;
            }
        }
        if (currentCup != null)
        {
            currentCup.milk = currentMilk;
            currentCup.UpdateVisual();
        }
    }

    private void BeginEnding()
    {
        state = PourState.Ending;
        frameIndex = 0;
        frameTimer = 0f;
    }

    public void ResetPour()
    {
        currentMilk = null;
        pixelsFilled = 0;
        pixelsFilledAtSessionStart = 0;
        milkFillImage.anchoredPosition = originalMilkPos;
    }
}