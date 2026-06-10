using UnityEngine;
using UnityEngine.UI;

public class SyrupManager : MonoBehaviour
{
    [SerializeField] private SyrupTypes syrupType;
    [SerializeField] private CupContents cupContents;

    [SerializeField] private Image cupImage;
    [SerializeField] private Sprite cupTallSprite;
    [SerializeField] private Sprite cupSmallSprite;

    [SerializeField] private Image syrupBottleImage;
    [SerializeField] private Sprite syrupBottleDefault;
    [SerializeField] private Sprite syrupBottlePressed;

    private bool hasAdded = false;
    private float pressTimer = 0f;
    private bool isPressed = false;

    private void OnEnable()
    {
        if (cupContents != null && cupContents.filled && cupContents.drink != null)
        {
            cupImage.gameObject.SetActive(true);
            UpdateCupImage();
        }
        else
        {
            cupImage.gameObject.SetActive(false);
        }
    }

    private void UpdateCupImage()
    {
        if (cupContents.drink.size == Sizes.Tall)
            cupImage.sprite = cupTallSprite;
        else if (cupContents.drink.size == Sizes.Small)
            cupImage.sprite = cupSmallSprite;
    }

    private void Update()
    {
        if (cupContents != null && cupContents.filled && cupContents.drink != null)
        {
            cupImage.gameObject.SetActive(true);
            UpdateCupImage();
        }
        else
        {
            cupImage.gameObject.SetActive(false);
        }

        if (isPressed)
        {
            pressTimer -= Time.deltaTime;
            if (pressTimer <= 0f)
            {
                isPressed = false;
                syrupBottleImage.sprite = syrupBottleDefault;
            }
        }
    }

    public void OnSyrupBottleClicked()
    {
        if (hasAdded) return;
        if (cupContents == null || !cupContents.filled || cupContents.drink == null) return;
        if (cupContents.drink.syrupFlavor != SyrupTypes.None) return;

        cupContents.drink.syrupFlavor = syrupType;
        hasAdded = true;

        isPressed = true;
        pressTimer = 1f;
        syrupBottleImage.sprite = syrupBottlePressed;
    }

    public void Reset()
    {
        hasAdded = false;
        isPressed = false;
        pressTimer = 0f;
        if (syrupBottleImage != null)
            syrupBottleImage.sprite = syrupBottleDefault;
    }
}