using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Simulation
{
    public class SimulationUIController : MonoBehaviour
    {
        public void ExitSimulation()
        {
            SceneManager.LoadScene(0);
        }
    }
}
