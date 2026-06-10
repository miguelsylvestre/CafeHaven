using UnityEngine;

public class OrderManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Drink order;
    void Start()
    {
        order.size = Sizes.Tall;
        order = Recipes.GetRecipe(DrinkTypes.Latte, order.size);
        order.syrupFlavor = SyrupTypes.None;
        order.coffee.decaf = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
