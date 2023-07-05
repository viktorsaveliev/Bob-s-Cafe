using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class VisitorInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject _infoUI;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _orderText;
    [SerializeField] private Image _patienceBar;
    [SerializeField] private Gradient _patienceGradient;

    private bool _isVisitorSelected;
    private Visitor _currentVisitor;

    private void OnEnable()
    {
        EventBus.OnPlayerSelectVisitor += Show;
        EventBus.OnPlayerUnSelectVisitor += DisableUI;
        EventBus.OnVisitorHasLeftQueue += DisableUI;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerSelectVisitor -= Show;
        EventBus.OnPlayerUnSelectVisitor -= DisableUI;
        EventBus.OnVisitorHasLeftQueue -= DisableUI;
    }

    private void Show(Visitor visitor)
    {
        if(_currentVisitor != null && _currentVisitor != visitor)
        {
            _currentVisitor.UnSelect();
        }
        if (_isVisitorSelected && _currentVisitor == visitor)
        {
            DisableUI(visitor);
        }
        else
        {
            _currentVisitor = visitor;
            _infoUI.SetActive(true);
            _isVisitorSelected = true;

            UpdateVisitorInfoUI(visitor);
            StartCoroutine(UpdateTimer());
        }
    }

    private void UpdateVisitorInfoUI(Visitor visitor)
    {
        _nameText.text = visitor.GetName;
        _orderText.text = $"Order in queue: {visitor.OrderInQueue}";

        float patienceAmount = (visitor.CurrentState is WaitingState ? visitor.Patience : visitor.Satiety) / 100f;
        _patienceBar.DOFillAmount(patienceAmount, 0.5f);
        _patienceBar.color = _patienceGradient.Evaluate(patienceAmount);
    }

    private IEnumerator UpdateTimer()
    {
        yield return new WaitForSeconds(1f);
        if (_isVisitorSelected)
        {
            UpdateVisitorInfoUI(_currentVisitor);
            StartCoroutine(UpdateTimer());
        }
    }

    private void DisableUI(Visitor visitor)
    {
        if (_isVisitorSelected == false || (_currentVisitor != visitor && _currentVisitor.MemberGroupID != visitor.MemberGroupID)) return;

        _isVisitorSelected = false;
        StopCoroutine(UpdateTimer());
        _currentVisitor = null;
        _infoUI.SetActive(false);
    }
}