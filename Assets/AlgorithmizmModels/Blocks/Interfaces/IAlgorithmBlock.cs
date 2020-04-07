namespace AlgorithmizmModels.Blocks
{
    public interface IAlgorithmBlock
    {
        string Name { get; }
        IAlgorithmBlock Next { get; }

        BlockType Type { get; }
    }
}