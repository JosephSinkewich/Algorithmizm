using AlgorithmizmModels.Math;
using System.Collections.Generic;

namespace AlgorithmizmModels.Blocks
{
    public class IfBlock : IAlgorithmBlock
    {
        public BlockData Data { get; set; }
        public List<ParameterData> Parameters { get; set; }

        public string Name => "If " + Condition?.ToString();

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
