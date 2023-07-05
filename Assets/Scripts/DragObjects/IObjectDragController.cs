using UnityEngine;

public interface IObjectDragController
{
    public void CreateDraggableObject(GameObject itemPrefab, int price);
    public void DestroyDraggableObject();
    public void SpawnItem(bool isAllowed);
    public Material[] GetObjectStatusMaterials();
}
