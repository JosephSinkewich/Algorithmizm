using System;

namespace AlgorithmizmModels.Math
{
    public class Condition : IBoolean
    {
        public INumber Value1 { get; set; }
        public INumber Value2 { get; set; }
        public Relations Relation { get; set; }

        public bool IsTrue => CalculateExpression();

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
