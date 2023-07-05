using UnityEngine;

public class WaitingState : VisitorState
{
    private readonly Animator _animator;
    private readonly string _animationName = "Waiting";

    public WaitingState(Visitor visitor, Animator animator) : base(visitor)
    {
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.SetBool(_animationName, true);
    }

    public override void Update()
    {
        Visitor.UpdatePatience();
    }

    public override void Exit()
    {
        _animator.SetBool(_animationName, false);
    }

    
}
