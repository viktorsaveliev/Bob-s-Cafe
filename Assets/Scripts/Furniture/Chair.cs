using UnityEngine;
using UnityEngine.UI;

public class Chair : MonoBehaviour
{
    [SerializeField] private Vector3 _posWhenTableShowed;

    public Vector3 GetPosWhenTableShowed => _posWhenTableShowed;
    private bool _isUsed;
    public bool IsUsed => _isUsed;

    public void SetUsing()
    {
        _isUsed = true;
    }
    
    public void SetEmpty()
    {
        _isUsed = false;
    }
}
