namespace AlgorithmizmModels.Blocks
{
    public class BeginBlock : IAlgorithmBlock
    {
        public string Name => "BEGIN";
        public IAlgorithmBlock Next { get; set; }
        public BlockType Type => BlockType.Begin;
    }
}
