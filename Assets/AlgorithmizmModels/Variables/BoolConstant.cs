using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Variables
{
    public class BoolConstant : IBoolean
    {
        public bool IsTrue { get; set; }

        public ValueType Type => ValueType.Bool;

        public IValue Parent { get; set; }
    }
}
