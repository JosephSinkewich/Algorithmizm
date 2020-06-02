using System;
using System.Collections.Generic;

namespace AlgorithmizmModels.Blocks
{
    [Serializable]
    public class BlockData
    {
        public string name;
        public BlockType type;
        public List<ParameterData> parameters;
    }
}
