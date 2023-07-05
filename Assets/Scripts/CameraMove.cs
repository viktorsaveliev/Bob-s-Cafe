using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 _cameraBoundsMin;
    [SerializeField] private Vector3 _cameraBoundsMax;

    private Vector3 _currentMousePosition;

    private readonly float _cameraSpeed = 1f;
    private bool _isCameraDragging;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCameraDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCameraDrag();
        }

        if (_isCameraDragging)
        {
            UpdateCameraPosition();
        }
    }

    private void StartCameraDrag()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (!isPointerOverUI)
        {
            _isCameraDragging = true;
            _currentMousePosition = Input.mousePosition;
        } 
    }

    private void StopCameraDrag()
    {
        _isCameraDragging = false;
    }

    private void UpdateCameraPosition()
    {
        Vector3 newMousePosition = Input.mousePosition;
        Vector3 direction = newMousePosition - _currentMousePosition;
        direction.z = direction.y;
        direction.y = 0;

        float distance = direction.magnitude;
        float cameraSpeed = _cameraSpeed * distance;

        Vector3 newRotation = cameraSpeed * Time.deltaTime * -direction.normalized;
        Vector3 desiredPosition = transform.position + newRotation;

        transform.position = desiredPosition;

        //_isCameraOutOfBounds = IsCameraOutOfBounds();
        _currentMousePosition = newMousePosition;
    }

    /*private bool IsCameraOutOfBounds()
    {
        Vector3 cameraPosition = transform.position;
        return cameraPosition.x < _cameraBoundsMin.x || cameraPosition.x > _cameraBoundsMax.x ||
               cameraPosition.z < _cameraBoundsMin.z || cameraPosition.z > _cameraBoundsMax.z;
    }*/
}
