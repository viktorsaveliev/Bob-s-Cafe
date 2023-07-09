
public class FurnitureController
{
    private readonly FurniturePlacemant _furniturePlacemant;
    private readonly FurnitureUI _furnitureUI;

    private Furniture _selectedFurniture;

    public FurnitureController(FurniturePlacemant furniturePlacemant, FurnitureUI furnitureUI)
    {
        _furniturePlacemant = furniturePlacemant;
        _furnitureUI = furnitureUI;
    }

    public void Init()
    {
        EventBus.OnPlayerSelectFurniture += OnSelectFurniture;
        EventBus.OnPlayerUnSelectFurniture += OnUnSelectFurniture;

        _furnitureUI.OnClickEditButton += EditFurniture;
        _furnitureUI.OnClickSaleButton += SaleFurniture;
    }

    public void DeInit()
    {
        EventBus.OnPlayerSelectFurniture -= OnSelectFurniture;
        EventBus.OnPlayerUnSelectFurniture -= OnUnSelectFurniture;

        _furnitureUI.OnClickEditButton -= EditFurniture;
        _furnitureUI.OnClickSaleButton -= SaleFurniture;
    }

    private void OnSelectFurniture(Furniture furniture)
    {
        _selectedFurniture = furniture;
        _furnitureUI.ShowUI();
    }

    private void OnUnSelectFurniture(Furniture furniture)
    {
        _selectedFurniture = null;
        _furnitureUI.HideUI();
    }

    private void EditFurniture()
    {
        _furniturePlacemant.CreateDraggableObject(_selectedFurniture.gameObject, 0);
        _selectedFurniture.gameObject.SetActive(false);

        _selectedFurniture.UnSelect();
    }

    private void SaleFurniture()
    {
        FurnitureSeller seller = new(_selectedFurniture);
        seller.Sale();
    }
}
