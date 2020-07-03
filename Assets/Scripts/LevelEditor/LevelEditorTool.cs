using UnityEngine;
using UnityEngine.UI;

public class LevelEditorTool : MonoBehaviour
{
    [SerializeField] private string _objectName;
    [SerializeField] private Toggle _toggle;

    public Toggle Toggle => _toggle;
    public string ObjectName => _objectName;
}
