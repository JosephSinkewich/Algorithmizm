using System;

namespace AlgorithmizmModels.Math
{
    public class Expression : IExpression, INumber
    {
        public INumber Value1 { get; set; }
        public INumber Value2 { get; set; }
        public Operations Operation { get; set; }

        public ValueType Type => ValueType.Number;

        public double Value => CalculateExpression();

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
