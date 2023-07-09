using UnityEngine;

public class MoneyHandler
{
    public int Money { get; private set; }

    public bool Give(int amount)
    {
        if (amount < 1) return false;

        Money += amount;
        EventBus.OnMoneyValueChanged?.Invoke();

        return true;
    }

    public bool Spend(int amount)
    {
        if (amount < 1 || Money < amount)
        {
            Debug.Log("Dont have a money");
            return false;
        }

        Money -= amount;
        EventBus.OnMoneyValueChanged?.Invoke();

        return true;
    }

    public void Set(int amount)
    {
        if (amount < 0) return;

        Money = amount;
        EventBus.OnMoneyValueChanged?.Invoke();
    }
}
