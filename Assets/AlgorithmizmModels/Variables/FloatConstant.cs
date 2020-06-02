using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Variables
{
    public class FloatConstant : INumber
    {
        public double Value { get; set; }

        public ValueType Type => ValueType.Number;

        public IValue Parent { get; set; }
    }
}
