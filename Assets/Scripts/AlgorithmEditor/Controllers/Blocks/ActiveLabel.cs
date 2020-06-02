using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using AlgorithmizmModels.Math;
using AlgorithmizmModels.Variables;

namespace Algorithmizm
{
    public class ActiveLabel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        private IValue _value;

        public IValue Value
        {
            get => _value;
            set
            {
                if (_value != null && _value.Parent != null)
                {

                    Expression expression = _value.Parent as Expression;
                    LogicExpression logicExpression = _value.Parent as LogicExpression;
                    if (expression != null)
                    {
                        if (expression.Value1 == _value)
                        {
                            _value = value;
                            expression.Value1 = (INumber)_value;
                        }
                        else
                        {
                            _value = value;
                            expression.Value2 = (INumber)_value;
                        }
                    }

                    if (logicExpression != null)
                    {
                        if (logicExpression.Boolean1 == _value)
                        {
                            _value = value;
                            logicExpression.Boolean1 = (IBoolean)_value;
                        }
                        else
                        {
                            _value = value;
                            logicExpression.Boolean2 = (IBoolean)_value;
                        }
                    }
                }
                else
                {
                    _value = value;
                }

                ValueType = _value.Type;
                RefreshView();
            }
        }

        public ValueType ValueType { get; set; }
        public ActiveLabelType LabelType { get; set; }
        
        public UnityEvent<ActiveLabel> OnClick { get; set; } =
            new ActiveLabelEvent();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClick?.Invoke(this);
            }
        }

        private void Start()
        {
            RefreshView();
        }

        private void RefreshView()
        {
            if (_value != null)
            {
                _label.color = _resourceProvider.ActiveLabelNormalColor;
            }
            else
            {
                _label.color = _resourceProvider.ActiveLabelErrorColor;
            }

            if (Value == null)
            {
                _label.text = "SetValue";
            }
            else if (Value is FloatConstant floatConstant)
            {
                _label.text = floatConstant.Value.ToString();
            }
            else if (Value is BoolConstant boolConstant)
            {
                _label.text = boolConstant.IsTrue.ToString();
            }
            else if (Value is IVariable variable)
            {
                _label.text = variable.Name;
            }
            else if (Value is Expression expression)
            {
                _label.text = OperationToString(expression.Operation);
            }
            else if (Value is LogicExpression logicExpression)
            {
                _label.text = LogicOperationToString(logicExpression.Operation);
            }
        }

        private string OperationToString(Operations operation)
        {
            switch (operation)
            {
                case Operations.Add:
                    return "+";
                case Operations.Substract:
                    return "-";
                case Operations.Multiple:
                    return "*";
                case Operations.Divide:
                    return "/";
                default:
                    return "???";
            }
        }

        private string LogicOperationToString(LogicOperations operation)
        {
            switch (operation)
            {
                case LogicOperations.And:
                    return "&&";
                case LogicOperations.Or:
                    return "||";
                default:
                    return "???";
            }
        }
    }
}
