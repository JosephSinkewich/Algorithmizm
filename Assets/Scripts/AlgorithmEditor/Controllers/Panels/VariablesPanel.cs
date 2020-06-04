using AlgorithmizmModels.Variables;
using AlgorithmizmModels.Math;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Algorithmizm
{
    public class VariablesPanel : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Button _viewportButton;

        [SerializeField] private AlgorithmResourcesProvider _resourceProvider;

        private List<VariableUI> _variablesUIs = new List<VariableUI>();

        public List<IVariable> Variables { get; private set; } = new List<IVariable>();

        public IReadOnlyCollection<VariableUI> VariablesUIs => _variablesUIs;

        public UnityEvent<VariableUI> OnVariableClick { get; set; } =
            new VariableUIEvent();

        public UnityEvent OnPanelClick { get; set; } =
            new VoidEvent();

        public void AddVariable(IVariable newVariable)
        {
            Variables.Add(newVariable);

            VariableUI newVariableUI = CreateVaribale(newVariable);

            _variablesUIs.Add(newVariableUI);

            SetContentSiblings();

            RefreshTreeBlocksListeners();
        }

        private void RefreshTreeBlocksListeners()
        {
            foreach (VariableUI itVariableUI in _variablesUIs)
            {
                itVariableUI.OnClick.RemoveListener(VaribaleClickHandler);
                itVariableUI.OnClick.AddListener(VaribaleClickHandler);
            }
        }

        private void Start()
        {
            _viewportButton.onClick.AddListener(ViewportClickHandler);
        }


        private void OnDestroy()
        {
            _viewportButton.onClick.RemoveListener(ViewportClickHandler);
        }

        private VariableUI CreateVaribale(IVariable variableData)
        {
            VariableUI result = Instantiate(_resourceProvider.VariableUIPrefab, _content);
            result.VariableData = variableData;
            result.RefreshAnData();

            return result;
        }

        private void SetContentSiblings()
        {
            List<VariableUI> boolVariables = new List<VariableUI>();
            List<VariableUI> numberVariables = new List<VariableUI>();

            foreach (VariableUI itVariable in _variablesUIs)
            {
                if (itVariable.VariableData.Type == ValueType.Bool)
                {
                    boolVariables.Add(itVariable);
                }
                else
                {
                    numberVariables.Add(itVariable);
                }
            }

            SortVariableListByNames(boolVariables);
            SortVariableListByNames(numberVariables);

            _variablesUIs.Clear();
            foreach (VariableUI itVariable in boolVariables)
            {
                _variablesUIs.Add(itVariable);
            }
            foreach (VariableUI itVariable in numberVariables)
            {
                _variablesUIs.Add(itVariable);
            }

            for (int i = 0; i < _variablesUIs.Count; i++)
            {
                _variablesUIs[i].transform.SetSiblingIndex(i);
            }
        }

        private void SortVariableListByNames(List<VariableUI> list)
        {
            list.Sort((x, y) =>
            {
                string xName = x.VariableData.Name;
                string yName = y.VariableData.Name;

                return xName.CompareTo(yName);
            });
        }

        private void VaribaleClickHandler(VariableUI variable)
        {
            OnVariableClick?.Invoke(variable);
        }

        private void ViewportClickHandler()
        {
            OnPanelClick?.Invoke();
        }
    }
}
