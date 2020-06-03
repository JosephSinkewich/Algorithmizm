using UnityEngine;
using UnityEngine.Events;
using AlgorithmizmModels.Blocks;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using AlgorithmizmModels.Math;

namespace Algorithmizm
{
    public class AlgorithmBlockUI : MonoBehaviour, IPointerClickHandler
    {
        private const float ONE_TAB_LENGTH = 64;

        [SerializeField] private RectTransform _tabulation;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        private List<ValueUI> _valueUis = new List<ValueUI>();

        public IAlgorithmBlock BlockData { get; set; }

        private float _tabulationHeight;

        public int TabulationLevel { get; set; }

        public AlgorithmBlockUI PrevBlock { get; set; }
        public bool IsInsidePrevBlock { get; set; }

        public AlgorithmBlockUI NextBlock { get; set; }
        public AlgorithmBlockUI AlternativeNextBlock { get; set; }

        public UnityEvent<AlgorithmBlockUI> OnClick { get; set; } =
            new AlgorithmBlockUIEvent();

        public UnityEvent<ValueUI, ActiveLabel> OnLabelClick { get; set; } =
            new ValueUIEvent();

        public void RefreshAnData()
        {
            SetTabulationLengthAnData();

            foreach (ValueUI itValue in _valueUis)
            {
                Destroy(itValue.gameObject);
            }
            _valueUis.Clear();

            foreach (ParameterData itParameter in BlockData.Data.parameters)
            {
                InitParameter(itParameter);
            }

            _text.text = BlockData.Name;
            _image.sprite = _resourceProvider.BlockTypeSprites[BlockData.Type];
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClick?.Invoke(this);
            }
        }

        private void Start()
        {
            _tabulationHeight = _tabulation.sizeDelta.y;
        }

        private void SetTabulationLengthAnData()
        {
            int prevLevel = PrevBlock != null ? PrevBlock.TabulationLevel : 0;
            TabulationLevel = prevLevel + (IsInsidePrevBlock ? 1 : 0);
            float tabulationWidth = TabulationLevel * ONE_TAB_LENGTH;

            _tabulation.sizeDelta = new Vector2(tabulationWidth, _tabulationHeight);
        }

        private void InitParameter(ParameterData parameter)
        {
            ValueUI valueUi = CreateValueUI(parameter.type);
            valueUi?.OnLabelClick.AddListener(LabelClickHandler);
        }

        private void OnDestroy()
        {
            foreach (ValueUI itValue in _valueUis)
            {
                itValue?.OnLabelClick.RemoveListener(LabelClickHandler);
            }
        }

        private ValueUI CreateValueUI(ValueType type)
        {
            ValueUI result = Instantiate(_resourceProvider.ValueUiPrefab, transform);
            result.Type = type;
            result.RebuildAnValue();
            _valueUis.Add(result);

            return result;
        }

        private void LabelClickHandler(ValueUI valueUi, ActiveLabel label)
        {
            OnLabelClick?.Invoke(valueUi, label);
        }
    }
}
