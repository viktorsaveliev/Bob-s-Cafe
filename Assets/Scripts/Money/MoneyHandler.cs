public class MoneyHandler
{
    public int Money { get; private set; }

    public bool Give(int amount)
    {
        if (amount < 1)
        {
            Notification.ShowSimple(Notification.MessageType.IncorrectValue);
            return false;
        }

        Money += amount;
        EventBus.OnMoneyValueChanged?.Invoke();

        return true;
    }

    public bool TrySpend(int amount)
    {
        if (amount < 1 || Money < amount)
        {
            Notification.ShowSimple(Notification.MessageType.DontHaveMoney);
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
