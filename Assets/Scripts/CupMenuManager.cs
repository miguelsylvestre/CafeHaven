using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CupMenuManager : MonoBehaviour
{
    [SerializeField] private CupContents cup;

    [SerializeField] private Image water;
    [SerializeField] private Image milk;
    [SerializeField] private Image latteArt;
    [SerializeField] private Image foam;
    [SerializeField] private Image handle;
    [SerializeField] private Image ice;

    [SerializeField] private TextMeshProUGUI infoText;

    private DrinkTypes tempType;

    private void Update()
    {
        if (cup == null || cup.drink == null)
        {
            if (infoText != null) infoText.text = "";
            return;
        }

        tempType = DetermineDrinkType();

        Drink recipe = Recipes.GetRecipe(tempType, cup.drink.size);

        UpdateVisuals(recipe);
        UpdateInfoText();
    }

    private void UpdateInfoText()
    {
        if (infoText == null) return;

        Drink d = cup.drink;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"Type: {tempType}");
        sb.AppendLine($"Size: {d.size}");

        if (d.hasIce)
            sb.AppendLine("Iced");

        if (d.coffee != null)
            sb.AppendLine($"Coffee: intensity {d.coffee.intensity}{(d.coffee.decaf ? " (decaf)" : "")}");

        if (d.milk != null)
        {
            sb.AppendLine($"Milk: {d.milk.amount}ml");
            if (d.milk.steamed) sb.AppendLine($"  Steamed (quality: {d.milk.SteamQuality():P0})");
            if (d.milk.frothed) sb.AppendLine($"  Frothed (quality: {d.milk.FrothQuality():P0})");
        }

        if (d.water != null)
            sb.AppendLine($"Water: {d.water.amount}ml");

        if (d.syrupFlavor != default)
            sb.AppendLine($"Syrup: {d.syrupFlavor}");

        infoText.text = sb.ToString();
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

    private void UpdateVisuals(Drink recipe)
    {
        Drink drink = cup.drink;

        UpdateMilk(drink, recipe);
        UpdateWater(drink, recipe);
        UpdateFoam(drink);
        UpdateIce(drink);
        UpdateHandle(drink);
        UpdateLatteArt(drink, recipe);
    }

    private void UpdateMilk(Drink drink, Drink recipe)
    {
        float alpha = 0f;

        if (drink.milk != null && recipe.milk != null)
        {
            float targetAlpha = 0f;

            switch (tempType)
            {
                case DrinkTypes.Latte:
                case DrinkTypes.IcedLatte:
                    targetAlpha = 0.4f;
                    break;
                case DrinkTypes.Cappuccino:
                    targetAlpha = 0.8f;
                    break;
            }

            float closeness = Mathf.Clamp01(1f - Mathf.Abs(drink.milk.amount - recipe.milk.amount) / 100f);
            alpha = targetAlpha * closeness;
        }

        SetAlpha(milk, alpha);
    }

    private void UpdateWater(Drink drink, Drink recipe)
    {
        float alpha = 0f;

        if (drink.water != null && recipe.water != null)
        {
            float closeness = Mathf.Clamp01(1f - Mathf.Abs(drink.water.amount - recipe.water.amount) / 100f);
            alpha = 0.3f * closeness;
        }

        SetAlpha(water, alpha);
    }

    private void UpdateFoam(Drink drink)
    {
        SetAlpha(foam, drink.milk != null && drink.milk.frothed ? 1f : 0f);
    }

    private void UpdateIce(Drink drink)
    {
        SetAlpha(ice, drink.hasIce ? 1f : 0f);
    }

    private void UpdateHandle(Drink drink)
    {
        handle.gameObject.SetActive(drink.size == Sizes.Small);
    }

    private void UpdateLatteArt(Drink drink, Drink recipe)
    {
        if (drink.hasIce || tempType != DrinkTypes.Latte || drink.milk == null || recipe.milk == null)
        {
            SetAlpha(latteArt, 0f);
            return;
        }

        float milkQuality = Mathf.Clamp01(1f - Mathf.Abs(drink.milk.amount - recipe.milk.amount) / 100f);
        float artAlpha = milkQuality * drink.milk.SteamQuality();
        SetAlpha(latteArt, artAlpha);
    }

    private void SetAlpha(Image image, float alpha)
    {
        if (image == null) return;
        Color c = image.color;
        c.a = Mathf.Clamp01(alpha);
        image.color = c;
    }
}