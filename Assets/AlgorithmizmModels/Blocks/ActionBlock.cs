namespace AlgorithmizmModels.Blocks
{
    public class ActionBlock : IAlgorithmBlock
    {
        public BlockData Data { get; set; }

        public string Name { get; set; }
        public IAlgorithmBlock Next { get; set; }

        public BlockType Type => BlockType.Action;
    }
}
