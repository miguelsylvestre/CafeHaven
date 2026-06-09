using UnityEngine;

public class CupContents : MonoBehaviour
{
    public Drink drink = null;
    public bool filled = false;
    [SerializeField] private SpriteRenderer cupImage;
    [SerializeField] private Sprite tallSprite;
    [SerializeField] private Sprite smallSprite;

    public void SetDrink(Drink incoming)
    {
        drink = incoming;
        filled = true;
    }

    public void updateCup()
    {
        if (drink == null) return;

        if (drink.size == Sizes.Tall)
            cupImage.sprite = tallSprite;
        else if (drink.size == Sizes.Small)
            cupImage.sprite = smallSprite;
    }
}