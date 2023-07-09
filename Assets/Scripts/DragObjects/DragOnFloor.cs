using UnityEngine;

public class DragOnFloor : IDraggable
{
    private readonly Transform _target;
    private readonly Renderer _renderer;

    private readonly float _hoverHeight;
    private readonly float _gridSize;

    public DragOnFloor(Transform target, float gridSize)
    {
        _target = target;
        _gridSize = gridSize;

        _renderer = target.GetComponent<Renderer>();
        _hoverHeight = GetObjectHeightAboveGround();
    }

    public void Move(Ray ray, RaycastHit hit)
    {
        Vector3 targetPosition = hit.point + Vector3.up * _hoverHeight;

        float x = Mathf.Round(targetPosition.x / _gridSize) * _gridSize;
        float y = targetPosition.y;
        float z = Mathf.Round(targetPosition.z / _gridSize) * _gridSize;

        _target.position = new Vector3(x, y, z);
    }

    private float GetObjectHeightAboveGround()
    {
        if (Physics.Raycast(_target.position + Vector3.forward * 5f, Vector3.down, out RaycastHit hit))
        {
            float distanceToGround = hit.distance;
            float objectHeight = _renderer.bounds.size.y;
            float heightAboveGround = objectHeight - distanceToGround;
            return heightAboveGround;
        }

        return 0f;
    }
}
