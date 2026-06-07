using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MilkPouring : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite heldSprite;

    private Image image;
    private bool isHeld;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = normalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = true;
        image.sprite = heldSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
        image.sprite = normalSprite;
    }

    void Update()
    {
        if (isHeld)
        {
            Debug.Log("Holding");
        }
    }
}