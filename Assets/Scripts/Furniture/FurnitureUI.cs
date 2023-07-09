using System;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureUI : MonoBehaviour
{
    [SerializeField] private GameObject _editUI;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _saleButton;

    public event Action OnClickEditButton;
    public event Action OnClickSaleButton;

    private void OnEnable()
    {
        _editButton.onClick.AddListener(EditButton);
        _saleButton.onClick.AddListener(SaleButton);
    }

    private void OnDisable()
    {
        _editButton.onClick.RemoveListener(EditButton);
        _saleButton.onClick.RemoveListener(SaleButton);
    }

    private void EditButton() => OnClickEditButton?.Invoke();
    private void SaleButton() => OnClickSaleButton?.Invoke();

    public void ShowUI() => _editUI.SetActive(true);
    public void HideUI() => _editUI.SetActive(false);
}
