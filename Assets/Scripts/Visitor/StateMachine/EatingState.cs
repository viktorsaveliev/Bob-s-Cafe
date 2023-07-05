using UnityEngine;

public class EatingState : VisitorState
{
    private readonly Animator _animator;
    private readonly string _animationName = "Eating";

    private Chair _usedChair;

    public EatingState(Visitor visitor, Animator animator) : base(visitor)
    {
        _animator = animator;
    }

    public void UpdateData(Chair chair)
    {
        _usedChair = chair;
    }

    public override void Enter()
    {
        SitOnChair();
        _animator.SetBool(_animationName, true);
    }

    public override void Update()
    {
        Visitor.UpdateSatiety();
    }

    public override void Exit()
    {
        EndEating();
        _animator.SetBool(_animationName, false);
    }

    private void EndEating()
    {
        GetUpFromChair();
        Visitor.PayForService();
    }

    private void SitOnChair()
    {
        Visitor.OrderInQueue = -1;
        Visitor.HUD.HideBar();
    }

    private void GetUpFromChair()
    {
        if (_usedChair != null)
        {
            _usedChair.SetEmpty();
        }
    }
}
