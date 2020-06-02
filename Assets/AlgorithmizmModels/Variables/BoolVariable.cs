using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Variables
{
    public class BoolVariable : IVariable, IBoolean
    {
        public ValueType Type => ValueType.Bool;

        public bool IsTrue { get; set; }
        public string Name { get; set; }

        public IValue Parent { get; set; }
    }
}
