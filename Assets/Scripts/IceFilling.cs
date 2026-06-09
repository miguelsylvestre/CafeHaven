using UnityEngine;
using System.Collections;

public class IceFilling : MonoBehaviour
{
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private Sprite dropSprite;
    [SerializeField] private float dropDisplayTime = 1f;

    [SerializeField] private GameObject cup;
    [SerializeField] private Sprite tallHoverSprite;
    [SerializeField] private Sprite smallHoverSprite;

    private Sprite originalSprite;
    private Sprite cupOriginalSprite;

    private SpriteRenderer sr;
    private SpriteRenderer cupSr;

    private Vector3 originalPosition;
    private bool isOverCup = false;

    void Start()
    {
        originalPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        originalSprite = sr.sprite;

        cupSr = cup.GetComponent<SpriteRenderer>();
        cupOriginalSprite = cupSr.sprite;
    }

    void OnMouseDown()
    {
        sr.sprite = dragSprite;
    }

    void OnMouseDrag()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouseWorld.x, mouseWorld.y, originalPosition.z);

        bool wasOverCup = isOverCup;
        isOverCup = IsCursorOverCup();

        if (isOverCup && !wasOverCup)
        {
            CupContents contents = cup.GetComponent<CupContents>();
            if (contents != null && contents.drink != null)
                cupSr.sprite = contents.drink.size == Sizes.Tall ? tallHoverSprite : smallHoverSprite;
        }
        else if (!isOverCup && wasOverCup)
        {
            cupSr.sprite = cupOriginalSprite;
        }
    }

    void OnMouseUp()
    {
        if (isOverCup)
        {
            CupContents contents = cup.GetComponent<CupContents>();
            if (contents != null)
                contents.drink.hasIce = true;

            StartCoroutine(ReturnAfterDelay());
        }
        else
        {
            ReturnToOrigin();
        }

        cupSr.sprite = cupOriginalSprite;
        isOverCup = false;
    }

    private bool IsCursorOverCup()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
        CupContents contents = cup.GetComponent<CupContents>();
        return hit != null && hit.gameObject == cup && !contents.drink.hasIce;
    }

    private IEnumerator ReturnAfterDelay()
    {
        sr.sprite = dropSprite;
        yield return new WaitForSeconds(dropDisplayTime);
        ReturnToOrigin();
    }

    private void ReturnToOrigin()
    {
        transform.position = originalPosition;
        sr.sprite = originalSprite;
    }
}