using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MilkPouring : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite[] startFrames;
    [SerializeField] private Sprite[] pourFrames;
    [SerializeField] private Sprite[] endFrames;

    [SerializeField] private Image pourAnimationImage;
    [SerializeField] private Image milkCartonImage;
    [SerializeField] private RectTransform milkFillImage;

    [SerializeField] private Sprite milkCartonDefault;
    [SerializeField] private Sprite milkCartonPouring;

    [SerializeField] private MilkCupContents currentCup;

    private Milk currentMilk;

    private const float FRAME_TIME = 1f / 4f;

    private const float STARTING_ML = 25f / 3f;
    private const float ML_PER_PIXEL = 25f / 3f;

    private const int MAX_PIXELS_UP = 23;

    private const int FRAMES_PER_PIXEL = 2;

    private float frameTimer;
    private int frameIndex;
    private int frameCounter;

    private int pixelSteps;

    private Vector2 originalMilkPos;

    private enum PourState { Idle, Starting, Pouring, Ending }
    private PourState state = PourState.Idle;

    private void Start()
    {
        originalMilkPos = milkFillImage.anchoredPosition;

        if (milkCartonImage == null)
            milkCartonImage = GetComponent<Image>();

        SetStaticIdleFrame();
        ResetPour();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state != PourState.Idle) return;

        if (currentMilk == null)
        {
            currentMilk = new Milk
            {
                cold = true,
                amount = STARTING_ML
            };

            PushToCup();
        }

        if (milkCartonImage != null)
            milkCartonImage.sprite = milkCartonPouring;

        SetState(PourState.Starting);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (state == PourState.Starting || state == PourState.Pouring)
        {
            SetState(PourState.Ending);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case PourState.Starting:
                UpdateAnimationSequence(startFrames, PourState.Pouring);
                break;

            case PourState.Pouring:
                UpdatePouring();
                break;

            case PourState.Ending:
                UpdateAnimationSequence(endFrames, PourState.Idle);
                break;
        }
    }

    private void UpdatePouring()
    {
        frameTimer += Time.deltaTime;

        if (frameTimer >= FRAME_TIME)
        {
            frameTimer -= FRAME_TIME;

            pourAnimationImage.sprite = pourFrames[frameIndex];
            frameIndex = (frameIndex + 1) % pourFrames.Length;

            frameCounter++;

            if (frameCounter >= FRAMES_PER_PIXEL)
            {
                frameCounter = 0;
                AddPixel();
            }
        }
    }

    private void AddPixel()
    {
        if (currentMilk == null) return;

        if (pixelSteps >= MAX_PIXELS_UP)
        {
            SetState(PourState.Ending);
            return;
        }

        pixelSteps++;

        milkFillImage.anchoredPosition =
            originalMilkPos + Vector2.up * pixelSteps;

        float rawMilk =
            STARTING_ML + (pixelSteps * ML_PER_PIXEL);

        currentMilk.amount =
            Mathf.Round(rawMilk * 100f) / 100f;

        Debug.Log(currentMilk.amount);

        PushToCup();

        if (pixelSteps >= MAX_PIXELS_UP)
        {
            SetState(PourState.Ending);
            return;
        }
    }

    private void UpdateAnimationSequence(Sprite[] frames, PourState nextState)
    {
        frameTimer += Time.deltaTime;

        if (frameTimer < FRAME_TIME)
            return;

        frameTimer -= FRAME_TIME;

        if (frameIndex < frames.Length)
        {
            pourAnimationImage.sprite = frames[frameIndex++];
        }
        else
        {
            SetState(nextState);
        }
    }


    private void PushToCup()
    {
        if (currentCup == null || currentMilk == null)
            return;

        currentCup.milk = currentMilk;
        currentCup.UpdateVisual();
    }

    private void SetState(PourState newState)
    {
        state = newState;

        frameIndex = 0;
        frameTimer = 0f;
        frameCounter = 0;

        if (state == PourState.Idle)
        {
            if (milkCartonImage != null)
                milkCartonImage.sprite = milkCartonDefault;

            SetStaticIdleFrame();
        }
    }

    private void SetStaticIdleFrame()
    {
        if (pourAnimationImage != null &&
            endFrames != null &&
            endFrames.Length > 0)
        {
            pourAnimationImage.sprite = endFrames[endFrames.Length - 1];
        }
    }


    public void ResetPour()
    {
        currentMilk = null;
        pixelSteps = 0;
        frameCounter = 0;

        milkFillImage.anchoredPosition = originalMilkPos;

        SetState(PourState.Idle);
    }
}