using System;

namespace AlgorithmizmModels.Math
{
    public class LogicExpression : IExpression, IBoolean
    {
        public IBoolean Boolean1 { get; set; }
        public IBoolean Boolean2 { get; set; }
        public LogicOperations Operation { get; set; }

        public bool IsTrue => CalculateExpression();

        private bool CalculateExpression()
        {
            if (Boolean1 == null || Boolean2 == null)
            {
                throw new Exception("LogicExpression is incomplete!");
            }

            if (Operation == LogicOperations.And)
            {
                return Boolean1.IsTrue && Boolean2.IsTrue;
            }
            else if (Operation == LogicOperations.Or)
            {
                return Boolean1.IsTrue || Boolean2.IsTrue;
            }

            throw new Exception("LogicExpression has unknown operator");
        }
    }
}
