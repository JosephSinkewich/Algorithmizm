using TMPro;
using UnityEngine;

namespace Algorithmizm
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class WidthTMProResizer : MonoBehaviour
    {
        private TextMeshProUGUI _textComponent;
        private RectTransform _rectTransform;
        private float _height;

        private float _prevValue;

        private void Start()
        {
            _textComponent = GetComponent<TextMeshProUGUI>();
            _rectTransform = GetComponent<RectTransform>();
            _height = _rectTransform.sizeDelta.y;
        }

        private void Update()
        {
            float textWidth = _textComponent.preferredWidth;
            if (_prevValue != textWidth)
            {
                _prevValue = textWidth;
                _rectTransform.sizeDelta = new Vector2(textWidth, _height);
            }
        }
    }
}