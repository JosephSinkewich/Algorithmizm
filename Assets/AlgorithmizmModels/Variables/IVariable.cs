﻿using AlgorithmizmModels.Math;

namespace AlgorithmizmModels.Variables
{
    public interface IVariable : IValue
    {
        string Name { get; set; }
    }
}
