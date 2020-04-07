using UnityEngine;

namespace Game.Scripts.MenuScripts
{
    public class LeaderBoardCanvas : MonoBehaviour
    {
        [SerializeField] private Canvas _levelsCanvas;

        public void ClosePage()
        {
            gameObject.SetActive(false);
            _levelsCanvas.gameObject.SetActive(true);
        }
    }
}
