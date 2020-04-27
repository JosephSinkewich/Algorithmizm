namespace AlgorithmizmModels.Math
{
    public class NotLogicOperation : IBoolean
    {
        public IBoolean Boolean { get; set; }

        public ValueType Type => ValueType.Bool;

        public bool IsTrue => !Boolean.IsTrue;
    }
}
