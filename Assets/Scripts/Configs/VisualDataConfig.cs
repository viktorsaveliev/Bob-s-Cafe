using UnityEngine;

[CreateAssetMenu(fileName = "VisualDataConfig", menuName = "Game/Visual Data Config")]
public class VisualDataConfig : ScriptableObject
{
    [SerializeField] private Color[] _outlineColors = new Color[2];

    public Color[] OutlineColors => _outlineColors;

    public enum OutlineType
    {
        MouseStay,
        Selected
    }
}
