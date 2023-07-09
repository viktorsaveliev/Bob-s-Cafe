using UnityEngine;
using DG.Tweening;

public class Table : Furniture, IFurnitureOnFloor
{
    [SerializeField] private Chair[] _chairs;

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

    public int GetChairsCount() => _chairs.Length;

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
