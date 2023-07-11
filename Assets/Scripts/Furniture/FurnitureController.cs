
public class FurnitureController : IStartDayObserver, IEndDayObserver
{
    private readonly FurniturePlacemant _furniturePlacemant;
    private readonly FurnitureUI _furnitureUI;

    private Furniture _selectedFurniture;

    private bool _isEditAllowed;

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

        _isEditAllowed = true;
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

        if(_isEditAllowed)
            _furnitureUI.ShowUI();
    }

    private void OnUnSelectFurniture(Furniture furniture)
    {
        _selectedFurniture = null;

        if (_isEditAllowed)
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

    public void OnDayStarted()
    {
        _isEditAllowed = false;
        _furnitureUI.HideUI();
    }

    public void OnDayEnded()
    {
        _isEditAllowed = true;
    }
}
