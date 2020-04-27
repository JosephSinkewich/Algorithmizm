using UnityEngine;
using Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders;
using AlgorithmizmModels.Math;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;

namespace Algorithmizm
{
    public class ValueUI : MonoBehaviour
    {
        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        public IValue Value { get; private set; }

        private List<GameObject> _expressionParts = new List<GameObject>();

        public IReadOnlyCollection<GameObject> ExpressionParts => _expressionParts;

        public UnityEvent<ValueUI, ActiveLabel> OnLabelClick { get; set; } =
            new ValueUIEvent();

        private void Start()
        {
            ActiveLabel emptyLabel = CreateActiveLabel();

            emptyLabel.Text = "SetValue";
            emptyLabel.IsSeted = false;
        }

        private void RefreshTreeBlocksListeners()
        {
            foreach (GameObject itPart in _expressionParts)
            {
                ActiveLabel itActiveLabel = itPart.GetComponent<ActiveLabel>();
                if (itActiveLabel != null)
                {
                    itActiveLabel.OnClick.RemoveListener(LabelClickHandler);
                    itActiveLabel.OnClick.AddListener(LabelClickHandler);
                }
            }
        }

        private void SetContentSiblings()
        {
            for (int i = 0; i < _expressionParts.Count; i++)
            {
                _expressionParts[i].transform.SetSiblingIndex(i);
            }
        }

        private void AddExpressionPart(GameObject expressionPart)
        {
            _expressionParts.Add(expressionPart);
            RefreshTreeBlocksListeners();
            SetContentSiblings();
        }

        private void RemoveExpressionPart(GameObject expressionPart)
        {
            _expressionParts.Remove(expressionPart);
            ActiveLabel itActiveLabel = expressionPart.GetComponent<ActiveLabel>();
            if (itActiveLabel != null)
            {
                itActiveLabel.OnClick.RemoveListener(LabelClickHandler);
            }

            Destroy(expressionPart);
            SetContentSiblings();
        }

        private TextMeshProUGUI CreateTextBlock()
        {
            TextMeshProUGUI result = Instantiate(_resourceProvider.AutosizebleTextPrefab, transform);
            AddExpressionPart(result.gameObject);

            return result;
        }

        private ActiveLabel CreateActiveLabel()
        {
            ActiveLabel result = Instantiate(_resourceProvider.ActiveLabelPrefab, transform);
            AddExpressionPart(result.gameObject);

            return result;
        }

        private void LabelClickHandler(ActiveLabel label)
        {
            OnLabelClick?.Invoke(this, label);
        }
    }
}
