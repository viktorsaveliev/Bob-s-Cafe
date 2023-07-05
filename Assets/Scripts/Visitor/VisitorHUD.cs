using UnityEngine;
using UnityEngine.UI;

public class VisitorHUD : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private Image _patienceProgress;

    private Camera _camera;
    private bool _isActive;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_isActive == false) return;
        
        Vector3 cameraDirection = _camera.transform.forward;
        Vector3 lookDirection = new(cameraDirection.x, _canvas.transform.position.y, cameraDirection.z);

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        targetRotation.x = _camera.transform.rotation.x;
        _canvas.transform.rotation = targetRotation;
    }

    public void UpdateBar(float value)
    {
        if (value < 0 || value > 1) return;

        if(value < 0.3f)
        {
            if (_isActive == false) ShowBar();
        }
        else
        {
            HideBar();
        }

        _patienceProgress.fillAmount = value;
    }

    private void ShowBar()
    {
        if (_progressBar.activeSelf == true) return;
        _progressBar.SetActive(true);
        _isActive = true;
    }

    public void HideBar()
    {
        if (_progressBar.activeSelf == false) return;
        _progressBar.SetActive(false);
        _isActive = false;
    }
}
