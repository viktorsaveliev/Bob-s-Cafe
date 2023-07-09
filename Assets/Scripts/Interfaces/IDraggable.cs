using UnityEngine;

public interface IDraggable
{
    public void Move(Ray ray, RaycastHit hit);
}
