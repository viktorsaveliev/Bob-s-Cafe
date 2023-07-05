using System.Collections;

public interface IDraggableObject
{
    public IEnumerator DragObject();
    public float GetObjectHeightAboveGround();
    public void SetTransparentMaterial(bool isAllowed);
}
