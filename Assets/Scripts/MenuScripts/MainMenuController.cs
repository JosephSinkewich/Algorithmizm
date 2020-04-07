using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.MenuScripts
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Canvas _levelsCanvas;
        [SerializeField] private Canvas _registrationCanvas;
        [SerializeField] private Button _registrationButton;

        public bool Registred { get; set; }

        public void ShowLevelsPage()
        {
            gameObject.SetActive(false);
            _levelsCanvas.gameObject.SetActive(true);
        }

        public void ShowRegistrationPage()
        {
            gameObject.SetActive(false);
            _registrationCanvas.gameObject.SetActive(true);
        }

        public void Exit()
        {

        }

        private void OnEnable()
        {
            _registrationButton.gameObject.SetActive(!Registred);
        }
    }
}
