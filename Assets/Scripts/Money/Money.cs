using UnityEngine;

public class Money : MonoBehaviour
{
    private static MoneyHandler _money;

    public static int Value => _money.Money;

    public void Init()
    {
        _money = new();
        _money.Set(1000);
    }

    public static bool Give(int amount) => _money.Give(amount);
    public static bool Spend(int amount) => _money.Spend(amount);
    public static void Set(int amount) => _money.Set(amount);
}
