namespace AlgorithmizmModels.Math
{
    public class NotLogicOperation : IBoolean
    {
        public IBoolean Boolean { get; set; }

        public bool IsTrue => !Boolean.IsTrue;
    }
}
