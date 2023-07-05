using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Color[] _outlineColors = new Color[2];

    public bool IsSelected { get; protected set; }
    public bool IsOnMouseEnter { get; private set; }

    private Outline _outline;

    public enum OutlineType
    {
        MouseStay,
        Selected
    }

    protected virtual void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 0;
    }

    private void OnMouseEnter()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            ShowOutline();
            IsOnMouseEnter = true;
        }
    }

    private void OnMouseExit()
    {
        IsOnMouseEnter = false;

        if (IsSelected) return;

        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            HideOutline();
        }
    }

    private void OnMouseUp()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            HandleClick();
        }
    }

    public virtual void ShowOutline()
    {
        _outline.OutlineWidth = 5;
    }

    public virtual void HideOutline()
    {
        _outline.OutlineWidth = 0;
    }

    protected void ChangeOutlineColor(OutlineType type)
    {
        if (_outline == null) return;
        _outline.OutlineColor = _outlineColors[(int)type];
    }

    protected abstract void HandleClick();
}