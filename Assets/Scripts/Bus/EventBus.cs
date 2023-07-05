using System;
using UnityEngine;

public static class EventBus
{
    public static Action<Visitor> OnPlayerSelectVisitor;
    public static Action<Visitor> OnPlayerUnSelectVisitor;

    public static Action<Visitor> OnVisitorHasLeftQueue;
    public static Action<Visitor> OnVisitorSitInChair;

    public static Action<Table> OnPlayerClickOnTable;
    public static Action<Table> OnPlayerSelectTable;
    public static Action<Table> OnPlayerUnSelectTable;

    public static Action OnMoneyValueChanged;

    public static Action OnNewDayStarted;
    public static Action OnCurrentDayEnded;

    public static Action<GameObject, int> OnPlayerSelectShopItem;
    public static Action OnPlayerBoughtShopItem;

    public static Action<Material, int> OnPlayerSelectWallpaper;
}
