using System;

namespace AlgorithmizmModels.Math
{
    public class Condition : IBoolean
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

        public Relations Relation { get; set; }

        public ValueType Type => ValueType.Bool;

        public IValue Parent { get; set; }

        public bool IsTrue
        {
            get
            {
                return CalculateExpression();
            }
            set { }
        }

        private bool CalculateExpression()
        {
            if (Value1 == null || Value2 == null)
            {
                throw new Exception("Condition is incomplete!");
            }

            if (Relation == Relations.Equal)
            {
                return Value1.Value == Value2.Value;
            }
            else if (Relation == Relations.NotEqual)
            {
                return Value1.Value != Value2.Value;
            }
            else if (Relation == Relations.More)
            {
                return Value1.Value > Value2.Value;
            }
            else if (Relation == Relations.Less)
            {
                return Value1.Value < Value2.Value;
            }
            else if (Relation == Relations.MoreEqual)
            {
                return Value1.Value >= Value2.Value;
            }
            else if (Relation == Relations.LessEqual)
            {
                return Value1.Value <= Value2.Value;
            }

            throw new Exception("Condition has unknown operator");
        }
    }
}
