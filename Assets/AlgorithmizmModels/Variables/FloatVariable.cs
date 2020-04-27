using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Variables
{
    public class FloatVariable : IVariable, INumber
    {
        public ValueType Type => ValueType.Number;

        public double Value { get; set; }
        public string Name { get; set; }
    }
}
