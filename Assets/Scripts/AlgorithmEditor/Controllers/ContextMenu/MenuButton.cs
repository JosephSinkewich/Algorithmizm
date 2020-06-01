using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Algorithmizm
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;

        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }

        public UnityEvent<MenuButton> OnClick = new MenuButtonEvent();

        private void Start()
        {
            _button.onClick.AddListener(ButtonClickHandler);
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ButtonClickHandler);
        }

        private void ButtonClickHandler()
        {
            OnClick?.Invoke(this);
        }
    }
}
