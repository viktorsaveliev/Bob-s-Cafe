public interface IShopItemObserver
{
    public void OnShopItemClicked<T>(T prefab, int price);
}
