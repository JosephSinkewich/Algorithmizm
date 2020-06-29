using Algorithmizm;
using AlgorithmizmModels.Blocks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelModule
{
    public class SimulationUIController : MonoBehaviour
    {
        [SerializeField] private Canvas _simulationCanvas;
        [SerializeField] private Canvas _algorithmCanvas;

        [SerializeField] private Button _algorithmButton;
        [SerializeField] private Button _algorithmDoneButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _nextButton;

        [SerializeField] private MainAlgorithmEditorController _algorithmController;

        [SerializeField] private Interpreatator _interpretator;

        private Algorithm _algorithm;

        private void Start()
        {
            _algorithmButton.onClick.AddListener(OpenAlgorithm);
            _algorithmDoneButton.onClick.AddListener(CloseAlgorithm);
            _exitButton.onClick.AddListener(ExitSimulation);

            _algorithmController.OnAlgorithmDone.AddListener(AlgorithmDoneHander);

            _startButton.onClick.AddListener(StartButtonClickHandler);
            _nextButton.onClick.AddListener(NextButtonClickHandler);
        }

        private void OnDestroy()
        {
            _algorithmButton.onClick.RemoveListener(OpenAlgorithm);
            _algorithmDoneButton.onClick.RemoveListener(CloseAlgorithm);
            _exitButton.onClick.RemoveListener(ExitSimulation);

            _algorithmController.OnAlgorithmDone.RemoveListener(AlgorithmDoneHander);

            _startButton.onClick.RemoveListener(StartButtonClickHandler);
            _nextButton.onClick.RemoveListener(NextButtonClickHandler);
        }

        private void OpenAlgorithm()
        {
            _simulationCanvas.gameObject.SetActive(false);
            _algorithmCanvas.gameObject.SetActive(true);
        }

        private void CloseAlgorithm()
        {
            _simulationCanvas.gameObject.SetActive(true);
            _algorithmCanvas.gameObject.SetActive(false);
        }

        private void ExitSimulation()
        {
            SceneManager.LoadScene(0);
        }

        private void AlgorithmDoneHander()
        {
            _algorithm = _algorithmController.Algorithm;
        }

        private void StartButtonClickHandler()
        {
            _interpretator.Algorithm = _algorithm;
            _interpretator.StartAlgorithm();
        }

        private void NextButtonClickHandler()
        {
            _interpretator.ExecuteNext();
        }
    }
}
