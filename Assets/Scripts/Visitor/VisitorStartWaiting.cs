public class VisitorStartWaiting : IVisitorActionAfterPointReached
{
    public void OnVisitorPointReached(Visitor visitor)
    {
        visitor.Waiting();
    }
}
