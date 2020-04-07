using UnityEngine;

namespace Game.Scripts.MenuScripts
{
    public class RegistrationCanvas : MonoBehaviour
    {
        [SerializeField] private MainMenuController _mainMenuCanvas;

        public void Registration()
        {
            gameObject.SetActive(false);
            _mainMenuCanvas.Registred = true;
            _mainMenuCanvas.gameObject.SetActive(true);
        }

        public void ClosePage()
        {
            gameObject.SetActive(false);
            _mainMenuCanvas.gameObject.SetActive(true);
        }
    }
}
