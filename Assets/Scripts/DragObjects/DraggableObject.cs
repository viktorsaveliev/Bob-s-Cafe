using System;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Material[] _transparentMaterial;

    private LayerMask _allowedLayer;
    private Camera _camera;
    private Renderer _objRenderer;

    private int _collisionsCount;
    private IDraggable _drag;

    public event Action OnDragCanceled;
    public event Action<bool> OnDragCompleted;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
        {
            OnDragCanceled?.Invoke();
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            OnDragCompleted?.Invoke(_collisionsCount == 0);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            Rotate(scroll);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1f);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1f);
        }

        Drag();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _allowedLayer)
        {
            _collisionsCount++;
            SetTransparentMaterial(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != _allowedLayer)
        {
            if (--_collisionsCount <= 0)
            {
                SetTransparentMaterial(true);
            }
        }
    }

    public void Init(LayerMask allowedLayer, Material[] transperentMaterial, IDraggable drag)
    {
        RemoveUnusedComponents();

        _camera = Camera.main;
        _transparentMaterial = transperentMaterial;
        _drag = drag;
        _allowedLayer = allowedLayer;

        _objRenderer = GetComponent<Renderer>();
        GetComponent<Collider>().isTrigger = true;

        SetTransparentMaterial(true);
    }

    private void RemoveUnusedComponents()
    {
        if (TryGetComponent<InteractableObject>(out var interactObj))
        {
            Destroy(interactObj);
        }

        if (TryGetComponent<Rigidbody>(out var rigidbody))
        {
            Destroy(rigidbody);
        }
    }

    public void Drag()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _allowedLayer))
        {
            _drag.Move(ray, hit);
        }
    }

    private void Rotate(float scroll)
    {
        float currentRotation = transform.eulerAngles.z;
        float targetRotation = currentRotation + 45f;

        transform.Rotate(Vector3.up, scroll > 0 ? targetRotation : -targetRotation);
    }

    public void SetTransparentMaterial(bool isAllowed)
    {
        _objRenderer.material = _transparentMaterial[isAllowed ? 0 : 1];

        var chairsMesh = GetComponentsInChildren<MeshRenderer>();
        if(chairsMesh.Length > 0)
        {
            foreach (MeshRenderer mesh in chairsMesh)
            {
                mesh.material = _transparentMaterial[isAllowed ? 0 : 1];
            }
        }
    }
}