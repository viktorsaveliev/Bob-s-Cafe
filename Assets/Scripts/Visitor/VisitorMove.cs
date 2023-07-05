using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VisitorMove : MonoBehaviour
{
    private NavMeshAgent _agent;
    private bool _isMoving;
    private Coroutine _waitingForEndMove;
    private bool _destroyAfterMove;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void GoTo(Vector3 targetPos, bool destroyAfterMove = false)
    {
        if (_agent.isOnNavMesh == false) return;

        _agent.SetDestination(targetPos);
        _isMoving = true;
        _destroyAfterMove = destroyAfterMove;

        if (_waitingForEndMove != null)
        {
            StopCoroutine(_waitingForEndMove);
        }

        _waitingForEndMove = StartCoroutine(WaitForEndPath());
    }

    public void Stop()
    {
        if (_isMoving == false) return;

        _isMoving = false;

        if (_waitingForEndMove != null)
        {
            StopCoroutine(_waitingForEndMove);
            _waitingForEndMove = null;
        }

        if (_destroyAfterMove)
        {
            Destroy();
        }
    }

    private IEnumerator WaitForEndPath()
    {
        if (_isMoving == false) yield break;

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (_agent.remainingDistance < 0.2f)
            { 
                Stop();
                break;
            }
        }
    }
    
    private void Destroy()
    {
        _destroyAfterMove = false;
        gameObject.SetActive(false);
    }
}
