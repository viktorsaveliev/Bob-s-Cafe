using System.Collections;
using UnityEngine;

public class DraggableObject : MonoBehaviour, IDraggableObject
{
    private LayerMask _allowedLayer;
    private MeshRenderer _meshRenderer;
    private ObjectDragController.FixedOn _fixedOn;

    private Material[] _transperentMaterial;
    private Camera _camera;
    private Renderer _objRenderer;

    private readonly float _gridSize = 1f;

    private float _hoverHeight;
    private int _collisionsCount;

    private IObjectDragController _dragController;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
        {
            _dragController.DestroyDraggableObject();
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            _dragController.SpawnItem(_collisionsCount == 0);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0) RotateObject(scroll);

        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateObject(1f);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateObject(-1f);
        }
    }

    public void Init(IObjectDragController dragController, LayerMask allowedLayer, ObjectDragController.FixedOn fixedOn)
    {
        if(TryGetComponent<InteractableObject>(out var interactObj))
        {
            Destroy(interactObj);
        }

        if (TryGetComponent<Rigidbody>(out var rigidbody))
        {
            Destroy(rigidbody);
        }

        _camera = Camera.main;

        _meshRenderer = GetComponent<MeshRenderer>();
        _objRenderer = GetComponent<Renderer>();
        _fixedOn = fixedOn;

        _allowedLayer = allowedLayer;
        _transperentMaterial = dragController.GetObjectStatusMaterials();
        _dragController = dragController;
        _hoverHeight = GetObjectHeightAboveGround();

        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;

        SetTransparentMaterial(true);
        StartCoroutine(DragObject());
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
            if(--_collisionsCount <= 0)
            {
                SetTransparentMaterial(true);
            }
        }
    }

    public IEnumerator DragObject()
    {
        while (true)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _allowedLayer))
            {
                if(_fixedOn == ObjectDragController.FixedOn.Floor)
                {
                    MoveObject(hit.point);
                }
                else
                {
                    float wallDistance = 0.2f;
                    float wallRotationY = Quaternion.LookRotation(hit.normal).eulerAngles.y;

                    Vector3 newPosition = hit.point - ray.direction * wallDistance;
                    newPosition.y = 2.35f;

                    transform.SetPositionAndRotation(newPosition, Quaternion.Euler(0f, wallRotationY, 0f));
                }
            }

            yield return null;
        }
    }

    private void MoveObject(Vector3 hitPoint)
    {
        Vector3 targetPosition = hitPoint + Vector3.up * _hoverHeight;

        float x = Mathf.Round(targetPosition.x / _gridSize) * _gridSize;
        float y = targetPosition.y;
        float z = Mathf.Round(targetPosition.z / _gridSize) * _gridSize;

        transform.position = new Vector3(x, y, z);
    }

    private void RotateObject(float scroll)
    {
        float currentRotation = transform.eulerAngles.z;
        float targetRotation = currentRotation + 45f;

        transform.Rotate(Vector3.up, scroll > 0 ? targetRotation : -targetRotation);
    }

    public float GetObjectHeightAboveGround()
    {
        if (Physics.Raycast(transform.position + Vector3.forward * 5f, Vector3.down, out RaycastHit hit))
        {
            float distanceToGround = hit.distance;
            float objectHeight = _objRenderer.bounds.size.y;
            float heightAboveGround = objectHeight - distanceToGround;
            return heightAboveGround;
        }

        return 0f;
    }

    public void SetTransparentMaterial(bool isAllowed)
    {
        _meshRenderer.material = _transperentMaterial[isAllowed ? 0 : 1];

        MeshRenderer[] chairsMesh = GetComponentsInChildren<MeshRenderer>();
        if(chairsMesh.Length > 0)
        {
            foreach (MeshRenderer mesh in chairsMesh)
            {
                mesh.material = _transperentMaterial[isAllowed ? 0 : 1];
            }
        }
    }
}
