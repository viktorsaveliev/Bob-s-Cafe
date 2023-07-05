public class VisitorDestroyer : IVisitorActionAfterPointReached
{
    public void OnVisitorPointReached(Visitor visitor)
    {
        visitor.gameObject.SetActive(false);
    }
}
