using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 _cameraBoundsMin;
    [SerializeField] private Vector3 _cameraBoundsMax;

    private readonly float _cameraSpeed = 1f;

    private Vector3 _currentMousePosition;
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

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            ChangeHeight(scroll);
        }

        if (_isCameraDragging)
        {
            UpdateCameraPosition();
        }
    }

    private void ChangeHeight(float scroll)
    {
        Vector3 currentPosition = transform.position;

        float sensitivity = 4;
        float targetHeight = currentPosition.y - scroll * sensitivity;

        Vector3 newPosition = new(currentPosition.x, FixOutOfBoundsY(targetHeight), currentPosition.z);
        transform.position = newPosition;
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

        transform.position = FixOutOfBoundsXZ(desiredPosition);
        _currentMousePosition = newMousePosition;
    }

    private Vector3 FixOutOfBoundsXZ(Vector3 cameraPosition)
    {
        if (cameraPosition.x < _cameraBoundsMin.x)
            cameraPosition.x = _cameraBoundsMin.x;

        else if (cameraPosition.x > _cameraBoundsMax.x)
            cameraPosition.x = _cameraBoundsMax.x;

        if (cameraPosition.z < _cameraBoundsMin.z)
            cameraPosition.z = _cameraBoundsMin.z;

        else if (cameraPosition.z > _cameraBoundsMax.z)
            cameraPosition.z = _cameraBoundsMax.z;

        return cameraPosition;
    }

    private float FixOutOfBoundsY(float y)
    {
        if (y < _cameraBoundsMin.y)
            y = _cameraBoundsMin.y;

        else if (y > _cameraBoundsMax.y)
            y = _cameraBoundsMax.y;

        return y;
    }
}
