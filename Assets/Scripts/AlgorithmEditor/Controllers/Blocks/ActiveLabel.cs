using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders;
using TMPro;
using AlgorithmizmModels.Math;

namespace Algorithmizm
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ActiveLabel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        private TextMeshProUGUI _label;
        private bool _isInitialized;
        private bool _isSeted;

        public bool IsSeted
        {
            get => _isSeted;
            set
            {
                _isSeted = value;
                RefreshView();
            }
        }

        public string Text
        {
            get => _label.text;
            set
            {
                _label.text = value;
            }
        }

        public IValue Value { get; set; }
        public LogicOperations LogicOperation { get; set; }
        public Operations Operation { get; set; }

        public ActiveLabelType Type { get; set; }

        public UnityEvent<ActiveLabel> OnClick { get; set; } =
            new ActiveLabelEvent();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClick?.Invoke(this);
            }
        }

        private void Awake()
        {
            _label = GetComponent<TextMeshProUGUI>();

            RefreshView();
        }

        private void RefreshView()
        {
            if (_isSeted)
            {
                _label.color = _resourceProvider.ActiveLabelNormalColor;
            }
            else
            {
                _label.color = _resourceProvider.ActiveLabelErrorColor;
            }
        }
    }
}
