using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Blocks
{
    public class IfBlock : IAlgorithmBlock
    {
        public string Name { get; set; }

        public IAlgorithmBlock ThenBlock { get; set; }
        public IAlgorithmBlock ElseBlock { get; set; }

        public IBoolean Condition { get; set; }

        public IAlgorithmBlock Next
        {
            get
            {
                if (Condition.IsTrue)
                {
                    return ThenBlock;
                }
                else
                {
                    return ElseBlock;
                }
            }
        }

        public BlockType Type => BlockType.If;
    }
}
