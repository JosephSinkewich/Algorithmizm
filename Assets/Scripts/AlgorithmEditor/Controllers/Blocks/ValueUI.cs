using UnityEngine;
using Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders;
using AlgorithmizmModels.Math;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;
using AlgorithmizmModels.Variables;

namespace Algorithmizm
{
    public class ValueUI : MonoBehaviour
    {
        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        public IValue Value { get; set; }

        private List<GameObject> _expressionParts = new List<GameObject>();

        public IReadOnlyCollection<GameObject> ExpressionParts => _expressionParts;

        public UnityEvent<ValueUI, ActiveLabel> OnLabelClick { get; set; } =
            new ValueUIEvent();

        public void RebuildAnValue()
        {
            DestroyExpression();
            Build();
        }

        private void Build()
        {
            _expressionParts = CreateExpression(Value);
        }

        private List<GameObject> CreateExpression(IValue value)
        {
            List<GameObject> result = new List<GameObject>();

            if (value == null)
            {
                ActiveLabel variableLabel = CreateActiveLabel();
                variableLabel.Value = null;
                result.Add(variableLabel.gameObject);
            }
            else if (value is IExpression)
            {
                List<GameObject> leftValue = new List<GameObject>();
                ActiveLabel operation = null;
                List<GameObject> rightValue = new List<GameObject>();

                if (value is Expression expression)
                {

                    leftValue = CreateExpression(expression.Value1);

                    operation = CreateActiveLabel();
                    operation.Value = expression;

                    rightValue = CreateExpression(expression.Value2);
                }
                else if (value is LogicExpression logicExpression)
                {
                    leftValue = CreateExpression(logicExpression.Boolean1);

                    operation = CreateActiveLabel();
                    operation.Value = logicExpression;

                    rightValue = CreateExpression(logicExpression.Boolean2);
                }

                TextMeshProUGUI leftParanthesis = CreateTextBlock();
                leftParanthesis.text = "(";
                TextMeshProUGUI rightParanthesis = CreateTextBlock();
                leftParanthesis.text = ")";

                result.Add(leftParanthesis.gameObject);
                result.AddRange(leftValue);
                result.Add(operation.gameObject);
                result.AddRange(rightValue);
                result.Add(rightParanthesis.gameObject);
            }
            else if (value is IVariable variable)
            {
                ActiveLabel variableLabel = CreateActiveLabel();
                variableLabel.Value = variable;
                result.Add(variableLabel.gameObject);
            }
            else if (value is FloatConstant floatConstant)
            {
                ActiveLabel variableLabel = CreateActiveLabel();
                variableLabel.Value = floatConstant;
                result.Add(variableLabel.gameObject);
            }
            else if (value is BoolConstant boolConstant)
            {
                ActiveLabel variableLabel = CreateActiveLabel();
                variableLabel.Value = boolConstant;
                result.Add(variableLabel.gameObject);
            }

            return result;
        }

        private void DestroyExpression()
        {
            foreach (GameObject itPart in _expressionParts)
            {
                ActiveLabel itActiveLabel = itPart.GetComponent<ActiveLabel>();
                if (itActiveLabel != null)
                {
                    itActiveLabel.OnClick.RemoveListener(LabelClickHandler);
                }
                Destroy(itPart);
            }

            _expressionParts.Clear();
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
