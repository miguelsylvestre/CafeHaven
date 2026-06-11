using UnityEngine;
using TMPro;

public class OrderManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Drink order;
    public TMP_Text orderText;

    void Start()
    {
        order.size = Sizes.Tall;
        order = Recipes.GetRecipe(DrinkTypes.Latte, order.size);
        order.syrupFlavor = SyrupTypes.None;
        order.coffee.decaf = false;
    }

    public void displayOrder()
    {
        orderText.text =
            "Drink: " + order + "\n" +
            "Size: " + order.size + "\n" +
            "Syrup: " + order.syrupFlavor + "\n" +
            "Decaf: " + (order.coffee.decaf ? "Yes" : "No");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
