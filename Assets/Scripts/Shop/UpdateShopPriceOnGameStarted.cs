using UnityEngine;

public class UpdateShopPriceOnGameStarted
{
    private GameDataConfig _gameDataConfig;

    public void Update()
    {
        _gameDataConfig = GameData.Settings.CurrentDifficultyLevel;

        FurnitureShopItem[] furnitures = Object.FindObjectsOfType<FurnitureShopItem>(true);
        foreach(FurnitureShopItem furnitureShopUI in furnitures)
        {
            if(furnitureShopUI.ItemPrefab.TryGetComponent(out Furniture furniture))
            {
                if(furniture is Table table)
                {
                    SetNewPriceForTable(table, furnitureShopUI);
                }
                else if(furniture is Painting painting)
                {
                    SetNewPriceForPainting(painting, furnitureShopUI);
                }
            }
        }
    }

    private void SetNewPriceForTable(Table table, FurnitureShopItem furnitureShopUI)
    {
        int newPrice = _gameDataConfig.PriceForTable + (_gameDataConfig.PriceForChair * table.GetChairsCount());

        furnitureShopUI.SetPrice(newPrice);
        table.SetPrice(newPrice);
    }

    private void SetNewPriceForPainting(Painting painting, FurnitureShopItem furnitureShopUI)
    {
        int newPrice = _gameDataConfig.PriceForPainting;

        furnitureShopUI.SetPrice(newPrice);
        painting.SetPrice(newPrice);
    }
}