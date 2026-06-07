using System;

[Serializable] public class Drink
{
    public DrinkTypes type;

    public bool decaf;
    public int intensity;
    public SyrupTypes syrupFlavor;
    public Milk milk;
    public Sizes size;
    public bool hasIce;
    public Water water;
}


[Serializable] public class Milk
{
    public bool steamed;
    public bool frothed;
    public float amount;
    public bool cold;
}

[Serializable] public class Water
{
    public float amount;
    public bool cold;
}