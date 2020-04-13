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

        private List<GameObject> _labels = new List<GameObject>();

        public IReadOnlyCollection<GameObject> Labels => _labels;

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
            foreach (GameObject itLabel in _labels)
            {
                ActiveLabel itActiveLabel = itLabel.GetComponent<ActiveLabel>();
                if (itActiveLabel != null)
                {
                    itActiveLabel.OnClick.RemoveListener(LabelClickHandler);
                    itActiveLabel.OnClick.AddListener(LabelClickHandler);
                }
            }
        }

        private void SetContentSiblings()
        {
            for (int i = 0; i < _labels.Count; i++)
            {
                _labels[i].transform.SetSiblingIndex(i);
            }
        }

        private void AddLabel(GameObject label)
        {
            _labels.Add(label);
            RefreshTreeBlocksListeners();
            SetContentSiblings();
        }

        private void RemoveLabel(GameObject label)
        {
            _labels.Remove(label);
            ActiveLabel itActiveLabel = label.GetComponent<ActiveLabel>();
            if (itActiveLabel != null)
            {
                itActiveLabel.OnClick.RemoveListener(LabelClickHandler);
            }

            Destroy(label);
            SetContentSiblings();
        }

        private TextMeshProUGUI CreateTextBlock()
        {
            TextMeshProUGUI result = Instantiate(_resourceProvider.AutosizebleTextPrefab, transform);
            AddLabel(result.gameObject);

            return result;
        }

        private ActiveLabel CreateActiveLabel()
        {
            ActiveLabel result = Instantiate(_resourceProvider.ActiveLabelPrefab, transform);
            AddLabel(result.gameObject);

            return result;
        }

        private void LabelClickHandler(ActiveLabel label)
        {
            OnLabelClick?.Invoke(this, label);
        }
    }
}
