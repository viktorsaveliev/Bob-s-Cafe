using DG.Tweening;

public class Furniture : InteractableObject
{
    private int _price;
    public int GetPrice => _price;

    public void SetPrice(int price)
    {
        if (price < 1) return;
        _price = price;
    }

    protected override void HandleClick()
    {
        AnimateFurniture();

        if (IsSelected)
        {
            UnSelect();
        }
        else
        {
            Select();
        }
    }

    protected override void OnSelected()
    {
        EventBus.OnPlayerSelectFurniture?.Invoke(this);
    }

    protected override void OnUnSelected()
    {
        EventBus.OnPlayerUnSelectFurniture?.Invoke(this);
    }

    private void AnimateFurniture()
    {
        if (IsAnimationActived == false)
        {
            IsAnimationActived = true;
            transform.DOShakeScale(0.3f, 0.2f).OnComplete(() =>
            {
                IsAnimationActived = false;
            });
        }
    }
}
