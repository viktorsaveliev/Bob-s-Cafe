using UnityEngine;
using UnityEngine.AI;

public class WalkingState : VisitorState
{
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private readonly string _animationName = "Walking";

    private IVisitorActionAfterPointReached _actionAfter;
    private Vector3 _targetPosition;

    private bool _isReachedPoint;
    
    public WalkingState(Visitor visitor, Animator animator) : base(visitor)
    {
        _navMeshAgent = visitor.GetComponent<NavMeshAgent>();
        _animator = animator;
    }

    public void UpdateData(Vector3 targetPosition, IVisitorActionAfterPointReached actionAfter)
    {
        _navMeshAgent.isStopped = false;
        _isReachedPoint = false;
        _targetPosition = targetPosition;
        _actionAfter = actionAfter;
    }

    public override void Enter()
    {
        Move();
        _animator.SetBool(_animationName, true);
    }

    public override void Update()
    {
        if (_isReachedPoint) return;

        if (_navMeshAgent.remainingDistance < 0.2f)
        {
            Stop();
        }
    }

    public override void Exit()
    {
        _animator.SetBool(_animationName, false);
    }

    private void Move()
    {
        _navMeshAgent.SetDestination(_targetPosition);
    }

    private void Stop()
    {
        _isReachedPoint = true;
        _navMeshAgent.isStopped = true;

        _actionAfter?.OnVisitorPointReached(Visitor);
    }
}
