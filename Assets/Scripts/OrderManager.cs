using UnityEngine;

public class OrderManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Drink order;
    void Start()
    {
        order = Recipes.GetRecipe(DrinkTypes.Latte);
        order.syrupFlavor = SyrupTypes.None;
        order.decaf = false;
        order.size = Sizes.Tall;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
