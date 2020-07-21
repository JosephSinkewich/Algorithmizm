using TMPro;
using UnityEngine;

namespace LevelEditor
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CameraPositionMonitor : MonoBehaviour
    {
        private TextMeshProUGUI _label;
        
        private void Start()
        {
            _label = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            Vector3 cameraPos = Camera.main.transform.position;

            _label.text = $"coords: [{cameraPos.x:F1}; {cameraPos.y:F1}]";
        }
    }
}