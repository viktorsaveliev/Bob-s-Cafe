using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class TableEditHandler : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [Inject] private readonly HandleSelector _selector;

    #region MonoBehaviour

    private void Start()
    {
        //InitVisualElements();
    }

    private void OnEnable()
    {
        EventBus.OnPlayerSelectTable += ShowEditUI;
        EventBus.OnPlayerUnSelectTable += HideEditUI;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerSelectTable -= ShowEditUI;
        EventBus.OnPlayerUnSelectTable -= HideEditUI;
    }

    #endregion

    #region Private methods

    private void InitVisualElements()
    {
        VisualElement rootVisualElement = _uiDocument.rootVisualElement;
        Button button = rootVisualElement.Q<Button>("EditButton");

        if (button != null)
        {
            button.clicked += EditTable;
        }
    }

    private void ShowEditUI(Table table)
    {
        //_uiDocument.enabled = true;
    }

    private void HideEditUI(Table table)
    {
        //_uiDocument.enabled = false;
    }

    private void EditTable()
    {
        //_selector.GetSelectedTable.Edit();
    }

    #endregion
}
