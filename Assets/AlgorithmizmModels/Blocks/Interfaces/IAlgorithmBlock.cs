namespace AlgorithmizmModels.Blocks
{
    public interface IAlgorithmBlock
    {
        BlockData Data { get; }

        string Name { get; }
        IAlgorithmBlock Next { get; }

        BlockType Type { get; }
    }
}