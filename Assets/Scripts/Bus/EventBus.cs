using System;

public static class EventBus
{
    // === Visitor
    public static Action<Visitor> OnPlayerSelectVisitor;
    public static Action<Visitor> OnPlayerUnSelectVisitor;

    public static Action<Visitor> OnVisitorHasLeftQueue;
    public static Action<Visitor> OnVisitorSitInChair;

    // === Furniture
    public static Action<Furniture> OnPlayerSelectFurniture;
    public static Action<Furniture> OnPlayerUnSelectFurniture;

    // === Other
    public static Action OnMoneyValueChanged;

    public static Action OnNewDayStarted;
    public static Action OnCurrentDayEnded;

    public static Action OnPlayerBoughtShopItem;
}
