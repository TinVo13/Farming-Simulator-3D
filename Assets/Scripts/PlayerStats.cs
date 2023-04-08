using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public static int money { get; private set; } = 10000;

    public const string CURRENCY = "VND";

    public static void Spend(int cost)
    {
        if (cost > money)
        {
            Debug.Log("Player does not have enough money");
            return;
        }
        money -= cost;

        UIManager.Instance.RenderPlayerStats();
    }

    public static void Earn(int income)
    {
        money += income;

        UIManager.Instance.RenderPlayerStats();
    }

    public static void LoadStats(int currentMoney)
    {
        money = currentMoney;
        UIManager.Instance.RenderPlayerStats();
    }
}
