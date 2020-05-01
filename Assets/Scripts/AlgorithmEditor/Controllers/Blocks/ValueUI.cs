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
        public ValueType Type { get; set; }

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
            IValue value = Value;
            _expressionParts = CreateExpression(ref value);
            Value = value;
        }

        private List<GameObject> CreateExpression(ref IValue value)
        {
            List<GameObject> result = new List<GameObject>();

            if (value == null)
            {
                ActiveLabel variableLabel = CreateActiveLabel();
                variableLabel.ValueType = Type;

                switch (Type)
                {
                    case ValueType.Bool:
                        {
                            value = new BoolConstant();
                        }
                        break;
                    case ValueType.Number:
                        {
                            value = new FloatConstant();
                        }
                        break;
                    default:
                        Debug.Log("ActiveLabelType is operation but Expression is null!");
                        break;
                }

                variableLabel.Value = value;

                result.Add(variableLabel.gameObject);
            }
            else if (value is IExpression)
            {
                List<GameObject> leftValue = new List<GameObject>();
                ActiveLabel operation = null;
                List<GameObject> rightValue = new List<GameObject>();

                if (value is Expression expression)
                {
                    IValue value1 = expression.Value1;
                    leftValue = CreateExpression(ref value1);
                    expression.Value1 = (INumber)value1;

                    operation = CreateActiveLabel();
                    operation.LabelType = ActiveLabelType.Expression;
                    operation.Value = expression;

                    IValue value2 = expression.Value2;
                    rightValue = CreateExpression(ref value2);
                    expression.Value2 = (INumber)value2;
                }
                else if (value is LogicExpression logicExpression)
                {
                    IValue bool1 = logicExpression.Boolean1;
                    leftValue = CreateExpression(ref bool1);
                    logicExpression.Boolean1 = (IBoolean)bool1;

                    operation = CreateActiveLabel();
                    operation.LabelType = ActiveLabelType.Expression;
                    operation.Value = logicExpression;

                    IValue bool2 = logicExpression.Boolean2;
                    rightValue = CreateExpression(ref bool2);
                    logicExpression.Boolean2 = (IBoolean)bool2;
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
                variableLabel.LabelType = ActiveLabelType.Variable;
                variableLabel.Value = variable;
                result.Add(variableLabel.gameObject);
            }
            else if (value is FloatConstant floatConstant)
            {
                ActiveLabel variableLabel = CreateActiveLabel();
                variableLabel.LabelType = ActiveLabelType.Constant;
                variableLabel.Value = floatConstant;
                result.Add(variableLabel.gameObject);
            }
            else if (value is BoolConstant boolConstant)
            {
                ActiveLabel variableLabel = CreateActiveLabel();
                variableLabel.LabelType = ActiveLabelType.Constant;
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
