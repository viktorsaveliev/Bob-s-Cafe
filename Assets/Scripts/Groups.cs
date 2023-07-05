using System.Collections.Generic;
using UnityEngine;

public class Groups : MonoBehaviour
{
    private const int MAX_GROUPS = 2;
    
    public List<Visitor>[] Visitors = new List<Visitor>[MAX_GROUPS];

    private Visitor _currentVisitor;

    public void Init()
    {
        for (int i = 0; i < MAX_GROUPS; i++)
        {
            Visitors[i] = new List<Visitor>();
        }
    }

    private void OnEnable()
    {
        EventBus.OnPlayerSelectVisitor += Select;
        EventBus.OnPlayerUnSelectVisitor += UnSelect;

        EventBus.OnVisitorHasLeftQueue += ResetGroup;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerSelectVisitor -= Select;
        EventBus.OnPlayerUnSelectVisitor -= UnSelect;

        EventBus.OnVisitorHasLeftQueue -= ResetGroup;
    }

    private void ChangeSelectStatus(Visitor visitor, bool isSelect)
    {
        if(visitor == null || visitor.IsMemberGroup == false) return;

        foreach (Visitor vis in Visitors[visitor.MemberGroupID])
        {
            if (isSelect) vis.Select();
            else vis.UnSelect();
        }
    }

    private void Select(Visitor visitor)
    {
        if(_currentVisitor != null && _currentVisitor.IsMemberGroup)
        {
            ChangeSelectStatus(_currentVisitor, false);
        }

        ChangeSelectStatus(visitor, true);
        _currentVisitor = visitor;
    }

    private void UnSelect(Visitor visitor)
    {
        ChangeSelectStatus(visitor, false);
        _currentVisitor = null;
    }

    public int FindFreeSlot()
    {
        int freeGroup = -1;
        for(int i = 0; i < MAX_GROUPS; i++)
        {
            if (Visitors[i].Count > 0) continue;
            freeGroup = i;
        }
        return freeGroup;
    }

    private void ResetGroup(Visitor visitor)
    {
        if (visitor.IsMemberGroup == false) return;
        Visitors[visitor.MemberGroupID].Clear();
    }
}
