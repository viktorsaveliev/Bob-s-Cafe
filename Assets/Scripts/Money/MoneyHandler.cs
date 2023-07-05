using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    public int Money { get; private set; }

    private void Start()
    {
        Set(6000);
    }

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
            print("Dont have a money");
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
