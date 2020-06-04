using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelModule
{
    public class SimulationUIController : MonoBehaviour
    {
        [SerializeField] private Canvas _simulationCanvas;
        [SerializeField] private Canvas _algorithmCanvas;

        public void OpenAlgorithm()
        {
            _simulationCanvas.gameObject.SetActive(false);
            _algorithmCanvas.gameObject.SetActive(true);
        }

        public void CloseAlgorithm()
        {
            _simulationCanvas.gameObject.SetActive(true);
            _algorithmCanvas.gameObject.SetActive(false);
        }

        public void ExitSimulation()
        {
            SceneManager.LoadScene(0);
        }
    }
}
