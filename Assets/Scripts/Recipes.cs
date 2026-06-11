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
                coffee = 
                    new Coffee
                    {
                        intensity = 3
                    }
            }
        },

        {
            DrinkTypes.Latte,
            new Drink
            {
                type = DrinkTypes.Latte,
                coffee = 
                    new Coffee
                    {
                        intensity = 2
                    },
                milk =
                    new Milk
                    {
                        amount = 125,
                        steamed = true,
                    }
            }
        },

        {
            DrinkTypes.IcedLatte,
            new Drink
            {
                type = DrinkTypes.IcedLatte,
                coffee = 
                    new Coffee
                    {
                        intensity = 2
                    },
                hasIce = true,
                milk =
                    new Milk
                    {
                        amount = 150,
                        cold = true
                    }
            }
        },

        {
            DrinkTypes.Cappuccino,
            new Drink
            {
                type = DrinkTypes.Cappuccino,
                coffee = 
                    new Coffee
                    {
                        intensity = 2
                    },
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
                coffee = 
                    new Coffee
                    {
                        intensity = 2
                    },
                water =
                    new Water
                    {
                        amount = 100,
                    }
            }
        },

        {
            DrinkTypes.ColdAmericano,
            new Drink
            {
                type = DrinkTypes.ColdAmericano,
                coffee = 
                    new Coffee
                    {
                        intensity = 2
                    },
                hasIce = true,
                water =
                    new Water
                    {
                        amount = 100,
                        cold = true
                    }
            }
        }

    };
    public static Drink GetRecipe(DrinkTypes type, Sizes size)
    {
        Drink recipe = baseRecipes[type];

        Drink drink = new Drink
        {
            type = recipe.type,
            size = size,
            syrupFlavor = recipe.syrupFlavor,
            hasIce = recipe.hasIce,

            coffee = recipe.coffee == null ? null : new Coffee
            {
                decaf = recipe.coffee.decaf,
                intensity = recipe.coffee.intensity
            },

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

        if (size == Sizes.Tall)
        {
            if (drink.milk != null)
                drink.milk.amount += 50;

            if (drink.water != null)
                drink.water.amount += 50;
        }

        return drink;
    }
}