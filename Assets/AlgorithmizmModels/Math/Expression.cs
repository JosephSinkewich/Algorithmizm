using System;

namespace AlgorithmizmModels.Math
{
    public class Expression : IExpression, INumber
    {
        private INumber _value1;
        private INumber _value2;
        
        public INumber Value1
        {
            get
            {
                return _value1;
            }
            set
            {
                _value1 = value;
                _value1.Parent = this;
            }
        }

        public INumber Value2
        {
            get
            {
                return _value2;
            }
            set
            {
                _value2 = value;
                _value2.Parent = this;
            }
        }

        public Operations Operation { get; set; }

        public ValueType Type => ValueType.Number;

        public IValue Parent { get; set; }
        
        public double Value
        {
            get
            {
                return CalculateExpression();
            }
            set { }
        }

        private double CalculateExpression()
        {
            if (Value1 == null || Value2 == null)
            {
                throw new Exception("Expression is incomplete!");
            }

            if (Operation == Operations.Add)
            {
                return Value1.Value + Value2.Value;
            }
            else if (Operation == Operations.Substract)
            {
                return Value1.Value - Value2.Value;
            }
            else if (Operation == Operations.Multiple)
            {
                return Value1.Value * Value2.Value;
            }
            else if (Operation == Operations.Divide)
            {
                return Value1.Value / Value2.Value;
            }

            throw new Exception("Expression has unknown operator");
        }
    }
}
