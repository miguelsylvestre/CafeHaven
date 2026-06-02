using System.Collections.Generic;
public class Recipes
{
    private static Dictionary<DrinkTypes, Drink> baseRecipes = new()
    {
        {
            DrinkTypes.Espresso,
            new Drink
            {
                type = DrinkTypes.Espresso,
                intensity = 3
            }
        },

        {
            DrinkTypes.Latte,
            new Drink
            {
                type = DrinkTypes.Latte,
                intensity = 2,
                milk =
                    new Milk
                    {
                        amount = 175,
                        steamed = true,
                    }
            }
        },

        {
            DrinkTypes.IcedLatte,
            new Drink
            {
                type = DrinkTypes.IcedLatte,
                intensity = 2,
                hasIce = true,
                milk =
                    new Milk
                    {
                        amount = 200,
                        cold = true
                    }
            }
        },

        {
            DrinkTypes.Cappuccino,
            new Drink
            {
                type = DrinkTypes.Cappuccino,
                intensity = 2,
                milk =
                    new Milk
                    {
                        amount = 100,
                        frothed = true
                    }
            }
        },

        {
            DrinkTypes.Americano,
            new Drink
            {
                type = DrinkTypes.Americano,
                intensity = 2,
                water =
                    new Water
                    {
                        amount = 150,
                    }
            }
        },

        {
            DrinkTypes.ColdAmericano,
            new Drink
            {
                type = DrinkTypes.ColdAmericano,
                intensity = 2,
                hasIce = true,
                water =
                    new Water
                    {
                        amount = 150,
                        cold = true
                    }
            }
        }

    };
    public static Drink GetRecipe(DrinkTypes type)
    {
        Drink recipe = baseRecipes[type];
        return new Drink
        {
            type = recipe.type,
            intensity = recipe.intensity,
            hasIce = recipe.hasIce,

            milk = recipe.milk == null ? null : new Milk
            {
                steamed = recipe.milk.steamed,
                frothed = recipe.milk.frothed,
                amount = recipe.milk.amount,
                cold = recipe.milk.cold
            },

            water = recipe.water == null ? null : new Water
            {
                amount = recipe.water.amount,
                cold = recipe.water.cold
            }
        };
    }
}