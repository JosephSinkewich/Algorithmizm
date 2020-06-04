using UnityEngine;
using UnityEngine.UI;

namespace LevelModule
{
    public class SimulationController : MonoBehaviour
    {
        [SerializeField] private SimulationUIController _uiController;

        [SerializeField] private Button _algorithmButton;

        private void Start()
        {
            _algorithmButton.onClick.AddListener(OpenAlgorithmEditor);
        }

        private void OnDestroy()
        {
            _algorithmButton.onClick.RemoveListener(OpenAlgorithmEditor);
        }

        private void OpenAlgorithmEditor()
        {
            _uiController.OpenAlgorithm();
        }
    }
}
