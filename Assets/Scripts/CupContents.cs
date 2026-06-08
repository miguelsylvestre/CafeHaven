using UnityEngine;

public class CupContents : MonoBehaviour
{
    public Drink drink;

    public void SetDrink(Drink incoming)
    {
        drink = incoming;
    }
}