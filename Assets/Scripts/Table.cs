using UnityEngine;
using DG.Tweening;

public class Table : InteractableObject
{
    [SerializeField] private Chair[] _chairs;
    private bool _isAnimationActived = false;

    public void Select()
    {
        if (IsSelected) return;

        IsSelected = true;

        ShowOutline();
        ChangeOutlineColor(OutlineType.Selected);

        EventBus.OnPlayerSelectTable?.Invoke(this);
    }

    public void UnSelect()
    {
        if (IsSelected == false) return;

        IsSelected = false;

        ChangeOutlineColor(OutlineType.MouseStay);

        if (IsOnMouseEnter == false)
        {
            HideOutline();
        }

        EventBus.OnPlayerUnSelectTable?.Invoke(this);
    }

    protected override void HandleClick()
    {
        if (_isAnimationActived == false)
        {
            _isAnimationActived = true;
            transform.DOShakeScale(0.3f, 0.2f).OnComplete(() =>
            {
                _isAnimationActived = false;
            });
        }

        if(IsSelected)
        {
            UnSelect();
        }
        else
        {
            Select();
        }
    }

    public Chair FindFreeChair()
    {
        Chair freeChair = null;
        foreach(Chair chair in _chairs)
        {
            if (chair.IsUsed) continue;
            freeChair = chair;
            break;
        }
        return freeChair;
    }

    public int GetFreeChairsCount()
    {
        int freeChairsCount = 0;
        foreach (Chair chair in _chairs)
        {
            if (chair.IsUsed) continue;
            freeChairsCount++;
        }
        return freeChairsCount;
    }

    public override void ShowOutline()
    {
        if (IsSelected) return;
        base.ShowOutline();
    }

    public override void HideOutline()
    {
        if (IsSelected) return;
        base.HideOutline();
    }

    public void SetChairsStatus(bool isOpen)
    {
        foreach (Chair chair in _chairs)
        {
            if (isOpen)
            {
                chair.gameObject.SetActive(true);
                chair.transform.DOLocalMove(chair.GetPosWhenTableShowed, 0.25f).SetEase(Ease.OutBack);
            }
            else
            {
                chair.transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    chair.gameObject.SetActive(false);
                });
            }
        }
    }
}
