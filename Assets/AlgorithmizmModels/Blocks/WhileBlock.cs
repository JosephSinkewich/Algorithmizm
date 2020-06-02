using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Blocks
{
    public class WhileBlock : IAlgorithmBlock
    {
        public BlockData Data { get; set; }

        public string Name => "While " + Condition?.ToString();

        public IAlgorithmBlock InnerBlock { get; set; }
        public IAlgorithmBlock OuterBlock { get; set; }

        public IBoolean Condition { get; set; }

        public IAlgorithmBlock Next
        {
            get
            {
                if (Condition.IsTrue)
                {
                    return InnerBlock;
                }
                else
                {
                    return OuterBlock;
                }
            }
        }

        public BlockType Type => BlockType.While;
    }
}
