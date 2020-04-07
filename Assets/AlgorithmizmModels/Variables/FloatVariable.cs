using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Variables
{
    public class FloatVariable : IVariable, INumber
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }
}
