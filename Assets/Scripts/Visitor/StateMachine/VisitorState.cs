public abstract class VisitorState : State
{
    protected readonly Visitor Visitor;

    public VisitorState(Visitor visitor)
    {
        Visitor = visitor;
    }
}
