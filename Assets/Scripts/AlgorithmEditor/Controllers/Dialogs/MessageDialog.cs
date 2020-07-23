using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Algorithmizm
{
    public class MessageDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Button _okButton;

        public void Init(string text)
        {
            _label.text = text;
        }

        private void Start()
        {
            _okButton.onClick.AddListener(OkClickHandler);
        }

        private void OkClickHandler()
        {
            Destroy(gameObject);
        }
    }
}
