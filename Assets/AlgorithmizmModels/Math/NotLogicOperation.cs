namespace AlgorithmizmModels.Math
{
    public class NotLogicOperation : IBoolean
    {
        public IBoolean Boolean { get; set; }

        public ValueType Type => ValueType.Bool;

        public IValue Parent { get; set; }

        public bool IsTrue => !Boolean.IsTrue;
    }
}
