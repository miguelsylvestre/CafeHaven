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

    private const float FRAME_TIME = 1f / 4f; // 4 FPS
    private const float STARTING_ML = 25f;
    private const float MAX_ML = 208.33333f;
    private const float MAX_PIXELS_UP = 22f; 
    private const float POUR_SPEED_ML_PER_SEC = 16.66666f; 

    private float frameTimer;
    private int frameIndex;
    private Vector2 originalMilkPos;

    private enum PourState { Idle, Starting, Pouring, Ending }
    private PourState state = PourState.Idle;

    private void Start()
    {
        originalMilkPos = milkFillImage.anchoredPosition;
        if (milkCartonImage == null) milkCartonImage = GetComponent<Image>();
        
        // Sets up the static baseline frame immediately at game start
        SetStaticIdleFrame();
        ResetPour();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state != PourState.Idle) return;

        if (currentMilk == null)
        {
            currentMilk = new Milk { cold = true, amount = STARTING_ML };
            UpdateMilkVisuals();
        }

        if (milkCartonImage != null && milkCartonPouring != null)
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
            case PourState.Starting: UpdateAnimationSequence(startFrames, PourState.Pouring); break;
            case PourState.Pouring:  UpdatePouringLoop(); break;
            case PourState.Ending:   UpdateAnimationSequence(endFrames, PourState.Idle); break;
        }
    }

    private void UpdateAnimationSequence(Sprite[] frames, PourState nextState)
    {
        frameTimer += Time.deltaTime;
        if (frameTimer < FRAME_TIME) return;

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

    private void UpdatePouringLoop()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= FRAME_TIME)
        {
            frameTimer -= FRAME_TIME;
            pourAnimationImage.sprite = pourFrames[frameIndex];
            frameIndex = (frameIndex + 1) % pourFrames.Length;
        }

        // Add milk over time
        currentMilk.amount += POUR_SPEED_ML_PER_SEC * Time.deltaTime;
        currentMilk.amount = Mathf.Min(currentMilk.amount, MAX_ML);

        // Update pixel-snapped position
        UpdateMilkVisuals();

        if (currentMilk.amount >= MAX_ML)
        {
            SetState(PourState.Ending);
        }
    }

    private void UpdateMilkVisuals()
    {
        if (currentMilk == null) return;

        float progress = (currentMilk.amount - STARTING_ML) / (MAX_ML - STARTING_ML);
       
        // FloorToInt prevents sub-pixel sliding entirely, keeping it strictly aligned
        // with your target rate of 1 pixel per half-second (2 frames)
        int snappedPixelsUp = Mathf.FloorToInt(MAX_PIXELS_UP * progress);

        milkFillImage.anchoredPosition = originalMilkPos + Vector2.up * snappedPixelsUp;

        if (currentCup != null)
        {
            currentCup.milk = currentMilk;
            currentCup.UpdateVisual();
        }
    }

    private void SetState(PourState newState)
    {
        state = newState;
        frameIndex = 0;
        frameTimer = 0f;

        if (state == PourState.Idle)
        {
            if (milkCartonImage != null && milkCartonDefault != null)
                milkCartonImage.sprite = milkCartonDefault;
            
            // Replaced the SetActive(false) logic to forcefully lock 
            // the graphic back onto the final frame of the array
            SetStaticIdleFrame();
        }
    }

    private void SetStaticIdleFrame()
    {
        if (pourAnimationImage != null && endFrames != null && endFrames.Length > 0)
        {
            pourAnimationImage.sprite = endFrames[endFrames.Length - 1];
        }
    }

    public void ResetPour()
    {
        currentMilk = null;
        milkFillImage.anchoredPosition = originalMilkPos;
        SetState(PourState.Idle);
    }
}