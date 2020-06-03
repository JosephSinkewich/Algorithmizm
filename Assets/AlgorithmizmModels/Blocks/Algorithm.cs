using AlgorithmizmModels.Variables;
using System.Collections.Generic;

namespace AlgorithmizmModels.Blocks
{
    public class Algorithm
    {
        public BeginBlock BeginBlock { get; set; }
        public List<IVariable> Variables { get; set; }
    }
}
