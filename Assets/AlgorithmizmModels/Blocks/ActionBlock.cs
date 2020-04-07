namespace AlgorithmizmModels.Blocks
{
    public class ActionBlock : IAlgorithmBlock
    {
        public string Name { get; set; }
        public IAlgorithmBlock Next { get; set; }

        public BlockType Type => BlockType.Action;
    }
}
