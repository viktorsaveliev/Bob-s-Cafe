/*using System.Collections.Generic;
using UnityEngine;

public class ChairsSelector : MonoBehaviour
{
    private List<Chair> _selectedChairs = new();

    private void OnEnable()
    {
        EventBus.OnPlayerClickOnChair += OnClickOnChair;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerClickOnChair -= OnClickOnChair;
    }

    private void OnClickOnChair(Chair chair)
    {
        if(chair.IsSelected)
        {
            RemoveChair(chair);
        }
        else
        {
            AddChair(chair);
        }
    }

    private void RemoveChair(Chair chair)
    {
        chair.UnSelect();
        _selectedChairs.Remove(chair);
    }

    private void AddChair(Chair chair)
    {
        chair.Select();
        _selectedChairs.Add(chair);
    }
}*/