using AlgorithmizmModels.Math;
using AlgorithmizmModels.Variables;

namespace AlgorithmizmModels.Blocks
{
    public class SetBlock : IAlgorithmBlock
    {
        public BlockData Data { get; set; }

        public string Name => "=";
        public IAlgorithmBlock Next { get; set; }
        public BlockType Type => BlockType.Set;

        public IVariable Variable { get; set; }
        public IValue Value { get; set; }
    }
}
