using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.MenuScripts
{
    public class LevelsPageController : MonoBehaviour
    {
        [SerializeField] private Canvas _mainMenuCanvas;
        [SerializeField] private Canvas _leaderBoardCanvas;

        public void ClosePage()
        {
            gameObject.SetActive(false);
            _mainMenuCanvas.gameObject.SetActive(true);
        }

        public void ShowLeaderBoardPage()
        {
            gameObject.SetActive(false);
            _leaderBoardCanvas.gameObject.SetActive(true);
        }

        public void BeginLevel()
        {
            SceneManager.LoadScene(1);
        }
    }
}
