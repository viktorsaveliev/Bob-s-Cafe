using UnityEngine;

public class FurnitureSeller
{
    private readonly Furniture _furniture;

    public FurnitureSeller(Furniture furniture)
    {
        _furniture = furniture;
    }

    public void Sale()
    {
        Notification.Show("Sale furniture", $"Do you want sale this furniture for {_furniture.GetPrice / 2}?", ConfirmSale);
    }

    public void ConfirmSale()
    {
        _furniture.UnSelect();

        Money.Give(_furniture.GetPrice / 2);
        Object.Destroy(_furniture.gameObject);
    }
}
