using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Variables
{
    public class BoolVariable : IVariable, IBoolean
    {
        public bool IsTrue { get; set; }
        public string Name { get; set; }
    }
}
