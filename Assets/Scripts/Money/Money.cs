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
    public static bool TrySpend(int amount) => _money.TrySpend(amount);
    public static void Set(int amount) => _money.Set(amount);
}
