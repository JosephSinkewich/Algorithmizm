using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Algorithmizm
{
    public class SetValueDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _okButton;

        public UnityEvent<string> OnOk = new StringEvent();

        public void Init(string caption, string inputValue)
        {
            _label.text = caption;
            _inputField.text = inputValue;
        }

        private void Start()
        {
            _okButton.onClick.AddListener(OkClickHandler);
        }

        private void OkClickHandler()
        {
            OnOk?.Invoke(_inputField.text);
            Destroy(gameObject);
        }
    }
}
