using AlgorithmizmModels.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Algorithmizm
{
    public class VariableUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        public IVariable VariableData { get; set; }

        public UnityEvent<VariableUI> OnClick { get; set; } =
            new VariableUIEvent();

        public void RefreshAnData()
        {
            _text.text = VariableData.Name;
            _image.sprite = _resourceProvider.ValueTypeSprites[VariableData.Type];
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClick?.Invoke(this);
            }
        }
    }
}
