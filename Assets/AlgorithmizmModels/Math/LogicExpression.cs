using System;

namespace AlgorithmizmModels.Math
{
    public class LogicExpression : IExpression, IBoolean
    {
        private IBoolean _boolean1;
        private IBoolean _boolean2;

        public IBoolean Boolean1
        {
            get
            {
                return _boolean1;
            }
            set
            {
                _boolean1 = value;
                _boolean1.Parent = this;
            }
        }

        public IBoolean Boolean2
        {
            get
            {
                return _boolean2;
            }
            set
            {
                _boolean2 = value;
                _boolean2.Parent = this;
            }
        }
        public LogicOperations Operation { get; set; }

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
