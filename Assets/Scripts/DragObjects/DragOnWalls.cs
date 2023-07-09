using UnityEngine;

public class DragOnWalls : IDraggable
{
    private readonly Transform _target;
    private readonly float _distanceFromWall = 0.2f;

    public DragOnWalls(Transform target)
    {
        _target = target;
    }

    public void Move(Ray ray, RaycastHit hit)
    {
        float wallRotationY = Quaternion.LookRotation(hit.normal).eulerAngles.y;

        Vector3 newPosition = hit.point - ray.direction * _distanceFromWall;
        newPosition.y = 2.35f;

        _target.SetPositionAndRotation(newPosition, Quaternion.Euler(0f, wallRotationY, 0f));
    }
}
