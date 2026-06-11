using System;
using UnityEngine;

[Serializable]
public class Drink
{
    public DrinkTypes type;

    public Coffee coffee;
    public SyrupTypes syrupFlavor;
    public Milk milk;
    public Sizes size;
    public bool hasIce;
    public Water water;
}

[Serializable]
public class Coffee
{
    public bool decaf;
    public int intensity;
}

[Serializable]
public class Milk
{
    public bool steamed;
    public bool frothed;
    public float amount;
    public bool cold;
    public float steamPosition;
    public float frothPosition;

    private const float PerfectSteamY = 12f;
    private const float PerfectFrothY = -12f;

    public float SteamError()
    {
        return Mathf.Abs(steamPosition - PerfectSteamY);
    }

    public float FrothError()
    {
        return Mathf.Abs(frothPosition - PerfectFrothY);
    }

    public bool IsPerfectSteam()
    {
        return SteamError() <= 1f;
    }

    public bool IsPerfectFroth()
    {
        return FrothError() <= 1f;
    }
    public float SteamQuality()
    {
        return Mathf.Clamp01(1f - SteamError() / 10f);
    }

    public float FrothQuality()
    {
        return Mathf.Clamp01(1f - FrothError() / 10f);
    }
}

[Serializable]
public class Water
{
    public float amount;
    public bool cold;
}