using UnityEngine;
using TMPro;

public class OrderMaker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderText;

    private Drink currentOrder;

    // All possible orders — normal ones appear multiple times to weight them higher
    private static readonly Drink[] orders = new Drink[]
    {
        // ── Espresso ────────────────────────────────────────────────
        new Drink { type = DrinkTypes.Espresso, size = Sizes.Small,
            coffee = new Coffee { intensity = 3 } },

        new Drink { type = DrinkTypes.Espresso, size = Sizes.Small,
            coffee = new Coffee { intensity = 3 } },

        new Drink { type = DrinkTypes.Espresso, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 } },

        new Drink { type = DrinkTypes.Espresso, size = Sizes.Small,
            coffee = new Coffee { decaf = true, intensity = 3 } },

        // ── Latte ───────────────────────────────────────────────────
        new Drink { type = DrinkTypes.Latte, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 125, steamed = true } },

        new Drink { type = DrinkTypes.Latte, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 125, steamed = true } },

        new Drink { type = DrinkTypes.Latte, size = Sizes.Tall,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 175, steamed = true } },

        new Drink { type = DrinkTypes.Latte, size = Sizes.Tall,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 175, steamed = true } },

        new Drink { type = DrinkTypes.Latte, size = Sizes.Small,
            coffee = new Coffee { decaf = true, intensity = 2 },
            milk = new Milk { amount = 125, steamed = true },
            syrupFlavor = SyrupTypes.Vanilla },

        new Drink { type = DrinkTypes.Latte, size = Sizes.Tall,
            coffee = new Coffee { intensity = 1 },
            milk = new Milk { amount = 175, steamed = true },
            syrupFlavor = SyrupTypes.Caramel },

        new Drink { type = DrinkTypes.Latte, size = Sizes.Small,
            coffee = new Coffee { intensity = 3 },
            milk = new Milk { amount = 125, steamed = true },
            syrupFlavor = SyrupTypes.Mocha },

        // ── Iced Latte ──────────────────────────────────────────────
        new Drink { type = DrinkTypes.IcedLatte, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 },
            hasIce = true,
            milk = new Milk { amount = 150, cold = true } },

        new Drink { type = DrinkTypes.IcedLatte, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 },
            hasIce = true,
            milk = new Milk { amount = 150, cold = true } },

        new Drink { type = DrinkTypes.IcedLatte, size = Sizes.Tall,
            coffee = new Coffee { intensity = 2 },
            hasIce = true,
            milk = new Milk { amount = 200, cold = true } },

        new Drink { type = DrinkTypes.IcedLatte, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 },
            hasIce = true,
            milk = new Milk { amount = 150, cold = true },
            syrupFlavor = SyrupTypes.Vanilla },

        new Drink { type = DrinkTypes.IcedLatte, size = Sizes.Tall,
            coffee = new Coffee { decaf = true, intensity = 2 },
            hasIce = true,
            milk = new Milk { amount = 200, cold = true },
            syrupFlavor = SyrupTypes.Caramel },

        // ── Cappuccino ──────────────────────────────────────────────
        new Drink { type = DrinkTypes.Cappuccino, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 100, frothed = true } },

        new Drink { type = DrinkTypes.Cappuccino, size = Sizes.Small,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 100, frothed = true } },

        new Drink { type = DrinkTypes.Cappuccino, size = Sizes.Tall,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 150, frothed = true } },

        new Drink { type = DrinkTypes.Cappuccino, size = Sizes.Tall,
            coffee = new Coffee { intensity = 2 },
            milk = new Milk { amount = 150, frothed = true } },

        new Drink { type = DrinkTypes.Cappuccino, size = Sizes.Small,
            coffee = new Coffee { intensity = 3 },
            milk = new Milk { amount = 100, frothed = true } },

        new Drink { type = DrinkTypes.Cappuccino, size = Sizes.Small,
            coffee = new Coffee { decaf = true, intensity = 2 },
            milk = new Milk { amount = 100, frothed = true },
            syrupFlavor = SyrupTypes.Mocha },
    };

    void Start()
    {
        PickNewOrder();
    }

    public void PickNewOrder()
    {
        currentOrder = orders[Random.Range(0, orders.Length)];
        UpdateText();
    }

    private void UpdateText()
    {
        if (orderText == null) return;

        Drink d = currentOrder;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine(d.type.ToString());
        sb.AppendLine(d.size.ToString());

        if (d.coffee != null)
            sb.AppendLine($"Intensity: {d.coffee.intensity}{(d.coffee.decaf ? " (Decaf)" : "")}");

        if (d.milk != null)
        {
            sb.AppendLine($"Milk: {d.milk.amount}ml");
            if (d.milk.steamed) sb.AppendLine("Steamed");
            if (d.milk.frothed) sb.AppendLine("Frothed");
        }

        if (d.syrupFlavor != default)
            sb.AppendLine($"Syrup: {d.syrupFlavor}");

        orderText.text = sb.ToString();
    }

    public Drink GetCurrentOrder() => currentOrder;
}