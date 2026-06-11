using UnityEngine;
using TMPro;

public class SubmitOrder : MonoBehaviour
{
    [SerializeField] private CupContents cup;
    [SerializeField] private OrderMaker orderMaker;
    [SerializeField] private GameObject cupMenu;
    [SerializeField] private TextMeshProUGUI pointsText;

    private static int totalPoints = 0;

    public void OnSubmit()
    {
        if (cup == null || cup.drink == null) return;

        Drink made = cup.drink;
        Drink order = orderMaker.GetCurrentOrder();
        made.type = DetermineDrinkType();

        int score = GradeDrink(made, order);
        Debug.Log(score);
        totalPoints += score;
        Debug.Log(totalPoints);

        pointsText.text = $"{totalPoints}";
        OpenMenus.CloseMenus();

        // 1. Close the cup menu
        cupMenu.SetActive(false);

        // 4. Generate a new order
        orderMaker.PickNewOrder();

        // 5. Clear the cup
        cup.drink = null;
        cup.filled = false;
        cup.gameObject.SetActive(false);
    }

    private DrinkTypes DetermineDrinkType()
    {
        Drink drink = cup.drink;

        float bestScore = float.MaxValue;
        DrinkTypes bestMatch = DrinkTypes.Espresso;

        foreach (DrinkTypes type in new[] { DrinkTypes.Espresso, DrinkTypes.Latte, DrinkTypes.IcedLatte, DrinkTypes.Cappuccino })
        {
            Drink recipe = Recipes.GetRecipe(type, cup.drink.size);
            float score = CompareDrinkToRecipe(drink, recipe);

            if (score < bestScore)
            {
                bestScore = score;
                bestMatch = type;
            }
        }

        return bestMatch;
    }

    private float CompareDrinkToRecipe(Drink drink, Drink recipe)
    {
        float score = 0f;

        if (drink.milk != null && recipe.milk != null)
            score += Mathf.Abs(drink.milk.amount - recipe.milk.amount);

        if (drink.water != null && recipe.water != null)
            score += Mathf.Abs(drink.water.amount - recipe.water.amount);

        if (drink.hasIce != recipe.hasIce)
            score += 100f;

        bool drinkSteamed = drink.milk != null && drink.milk.steamed;
        bool recipeSteamed = recipe.milk != null && recipe.milk.steamed;
        if (drinkSteamed != recipeSteamed) score += 100f;

        bool drinkFrothed = drink.milk != null && drink.milk.frothed;
        bool recipeFrothed = recipe.milk != null && recipe.milk.frothed;
        if (drinkFrothed != recipeFrothed) score += 100f;

        return score;
    }

    private int GradeDrink(Drink made, Drink order)
    {
        // Wrong type = 0
        if (made.type != order.type)
            return 0;

        float score = 100f;

        // Size mismatch — heavy penalty
        if (made.size != order.size)
            score -= 20f;

        // Ice mismatch
        if (made.hasIce != order.hasIce)
            score -= 15f;

        // Syrup mismatch
        if (made.syrupFlavor != order.syrupFlavor)
            score -= 15f;

        // Coffee intensity (each step off = -10)
        if (made.coffee != null && order.coffee != null)
        {
            int intensityDiff = Mathf.Abs(made.coffee.intensity - order.coffee.intensity);
            score -= intensityDiff * 10f;

            if (made.coffee.decaf != order.coffee.decaf)
                score -= 15f;
        }

        // Milk amount — lose up to 20 points based on how far off
        if (order.milk != null)
        {
            if (made.milk == null)
            {
                score -= 20f;
            }
            else
            {
                float milkDiff = Mathf.Abs(made.milk.amount - order.milk.amount);
                float milkPenalty = Mathf.Clamp(milkDiff / order.milk.amount, 0f, 1f) * 20f;
                score -= milkPenalty;

                // Steam/froth quality — lose up to 10 points each
                if (order.milk.steamed)
                    score -= (1f - made.milk.SteamQuality()) * 10f;

                if (order.milk.frothed)
                    score -= (1f - made.milk.FrothQuality()) * 10f;
            }
        }

        return Mathf.Clamp(Mathf.RoundToInt(score), 0, 100);
    }
}