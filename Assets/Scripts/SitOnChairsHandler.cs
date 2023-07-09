public class SitOnChairsHandler : IVisitorActionAfterPointReached
{
    private Visitor[] _visitorsInGroup;
    private Table _table;

    public void UpdateData(Table table)
    {
        _table = table;
    }

    public void UpdateData(Visitor[] visitors, Table table)
    {
        _visitorsInGroup = visitors;
        _table = table;
    }

    public void SitGroupOnChairs()
    {
        foreach (Visitor visitor in _visitorsInGroup)
        {
            SitOnFreeChair(visitor);
        }
    }

    public void SitOnFreeChair(Visitor visitor)
    {
        if (visitor == null) return;

        Chair freeChair = _table.FindFreeChair();
        if (freeChair != null)
        {
            freeChair.SetUsing();

            visitor.Walk(freeChair.transform.position, this);
            visitor.HUD.HideBar();

            visitor.SetChairForSitting(freeChair);
            EventBus.OnVisitorSitInChair?.Invoke(visitor);
        }
    }

    public void OnVisitorPointReached(Visitor visitor)
    {
        visitor.Eating();
    }
}
