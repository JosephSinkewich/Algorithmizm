namespace AlgorithmizmModels.Math
{
    public interface IValue
    {
        ValueType Type { get; }

        IValue Parent { get; set; }
    }
}
