using UnityEngine;
using UnityEngine.UI;

public class MilkIntoCup : MonoBehaviour
{
    [SerializeField] private GameObject milkCupObject;
    [SerializeField] private MilkPouring milkPouring;
    [SerializeField] private CupContents cupContents;
    [SerializeField] private Image animationImage;
    [SerializeField] private Image milkBack;
    [SerializeField] private Image milkFront;
    [SerializeField] private Image frame;
    [SerializeField] private Sprite Frame2;
    [SerializeField] private Sprite Frame1;
    [SerializeField] private Sprite[] animationFrames1;
    [SerializeField] private Sprite[] animationFrames2;
    private Sprite[] animationFrames;
    [SerializeField] private float heightMin;
    [SerializeField] private float heightMax;
    private Image selfImage;

    private const float FPS = 4f;
    private const float FRAME_TIME = 1f / FPS;
    private const float ML_PER_PIXEL = 8.33333f;
    private const float STARTING_ML = 25f;

    private MilkCupContents contents;

    private float frameTimer;
    private int frameIndex;
    private bool isPlaying;

    private void Start()
    {
        contents = milkCupObject.GetComponent<MilkCupContents>();
        selfImage = GetComponent<Image>();
        if (cupContents.drink.size == Sizes.Tall)
        {
            frame.sprite = Frame1;
        } else
        {
            frame.sprite = Frame2;
        }
        UpdateHeight();
        if (cupContents.drink.size == Sizes.Tall)
        {
            animationFrames = animationFrames1;
        }
        else
        {
            animationFrames = animationFrames2;
        }
    }

    private bool CanPour()
    {
        if (contents == null || contents.milk == null || contents.milk.amount <= 0) return false;
        if (cupContents == null) return false;
        if (cupContents.drink != null && cupContents.drink.milk != null) return false;
        return true;
    }

    private void Update()
    {
        UpdateHeight();


        if (!isPlaying)
        {
            if (frame != null)
                frame.gameObject.SetActive(cupContents != null && cupContents.filled);

            if (Input.GetMouseButtonDown(0))
            {
                if (!CanPour()) return;
                if (animationFrames.Length == 0) return;

                frameIndex = 0;
                frameTimer = 0f;
                isPlaying = true;
                milkBack.gameObject.SetActive(false);
                milkFront.gameObject.SetActive(false);
                if (selfImage != null) selfImage.enabled = false;
                animationImage.sprite = animationFrames[0];
            }
            return;
        }

        frameTimer += Time.deltaTime;

        if (frameTimer >= FRAME_TIME)
        {
            frameTimer -= FRAME_TIME;
            frameIndex++;

            if (frameIndex < animationFrames.Length)
            {
                animationImage.sprite = animationFrames[frameIndex];
            }
            else
            {
                isPlaying = false;
                frameIndex = 0;
                milkBack.gameObject.SetActive(true);
                milkFront.gameObject.SetActive(true);
                if (selfImage != null) selfImage.enabled = true;

                if (cupContents != null && contents.milk != null)
                {
                    if (cupContents.drink == null)
                        cupContents.drink = new Drink();

                    cupContents.drink.milk = contents.milk;
                }

                ClearMilk();
            }
        }
    }

    public void UpdateHeight()
    {
        if (contents == null || contents.milk == null)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, heightMin, transform.localPosition.z);
            return;
        }

        float pixels = Mathf.FloorToInt((contents.milk.amount - STARTING_ML) / ML_PER_PIXEL);
        float t = pixels / 22f;
        float y = Mathf.Lerp(heightMin, heightMax, t);

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            y,
            transform.localPosition.z
        );
    }

    public void ClearMilk()
    {
        if (contents != null)
        {
            contents.milk = null;
            contents.UpdateVisual();
        }

        if (milkPouring != null)
            milkPouring.ResetPour();

        UpdateHeight();
    }
}