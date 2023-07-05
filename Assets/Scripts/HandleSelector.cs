using UnityEngine;

[RequireComponent(typeof(Groups))]
public class HandleSelector : MonoBehaviour
{
    [SerializeField] private GameObject _acceptButton;

    private readonly SitOnChairsHandler _sitOnChairsHandler = new();

    private Groups _groups;

    private Visitor _selectedVisitor;
    public Visitor GetSelectedVisitor => _selectedVisitor;

    private Table _selectedTable;
    public Table GetSelectedTable => _selectedTable;

    private void OnEnable()
    {
        EventBus.OnPlayerSelectVisitor += SelectVisitor;
        EventBus.OnPlayerUnSelectVisitor += UnSelectVisitor;
        EventBus.OnVisitorHasLeftQueue += UnSelectVisitor;

        EventBus.OnPlayerSelectTable += OnSelectTable;
        EventBus.OnPlayerUnSelectTable += OnUnSelectTable;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerSelectVisitor -= SelectVisitor;
        EventBus.OnPlayerUnSelectVisitor -= UnSelectVisitor;
        EventBus.OnVisitorHasLeftQueue -= UnSelectVisitor;

        EventBus.OnPlayerSelectTable -= OnSelectTable;
        EventBus.OnPlayerUnSelectTable -= OnUnSelectTable;
    }

    private void Start()
    {
        _groups = GetComponent<Groups>();
    }

    private void SelectVisitor(Visitor visitor)
    {
        _selectedVisitor = visitor;

        if(visitor.CurrentState is EatingState == false) _acceptButton.SetActive(_selectedTable != null);
    }

    private void UnSelectVisitor(Visitor visitor)
    {
        if (_selectedVisitor == null || (_selectedVisitor != visitor && _selectedVisitor.MemberGroupID != visitor.MemberGroupID)) return;

        _selectedVisitor.UnSelect();
        _selectedVisitor = null;

        _acceptButton.SetActive(false);
    }

    private void OnSelectTable(Table table)
    {
        if(_selectedTable != table)
        {
            if(_selectedTable != null)
            {
                UnSelectTable();
            }

            _selectedTable = table;
            _acceptButton.SetActive(true);
        }
    }

    private void OnUnSelectTable(Table table)
    {
        if (_selectedTable == table)
        {
            UnSelectTable();
        }
    }

    private void UnSelectTable()
    {
        _selectedTable.UnSelect();
        _selectedTable = null;
        _acceptButton.SetActive(false);
    }

    public void SeatVisitorsInChairs()
    {
        if (_selectedVisitor == null || _selectedTable == null)
        {
            _acceptButton.SetActive(false);
            return;
        }

        if(_selectedVisitor.IsMemberGroup)
        {
            Visitor[] visitorsInGroup = _groups.Visitors[_selectedVisitor.MemberGroupID].ToArray();

            if (visitorsInGroup.Length > _selectedTable.GetFreeChairsCount())
            {
                print("Нет столько свободных стульев!");
                return;
            }

            _sitOnChairsHandler.UpdateData(visitorsInGroup, _selectedTable);
            _sitOnChairsHandler.SitGroupOnChairs();

            foreach(Visitor visitor in visitorsInGroup)
            {
                if (visitor == _selectedVisitor) continue;
                visitor.UnSelect();
            }
        }
        else
        {
            if (_selectedTable.GetFreeChairsCount() == 0)
            {
                print("Нет свободных стульев!");
                return;
            }

            _sitOnChairsHandler.UpdateData(_selectedTable);
            _sitOnChairsHandler.SitOnFreeChair(_selectedVisitor);
        }

        UnSelectVisitor(_selectedVisitor);
        UnSelectTable();
    }
}
