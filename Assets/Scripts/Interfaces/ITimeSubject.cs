public interface ITimeSubject
{
    public void AddStartDayObserver(IStartDayObserver observer);
    public void RemoveStartDayObserver(IStartDayObserver observer);

    public void AddEndDayObserver(IEndDayObserver observer);
    public void RemoveEndDayObserver(IEndDayObserver observer);
}
