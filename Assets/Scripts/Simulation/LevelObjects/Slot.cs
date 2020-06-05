using AlgorithmizmModels.Primitives;
using System.Collections.Generic;

namespace LevelModule
{
    public class Slot
    {
        public Int2 Coords { get; set; }

        public List<LevelObjectComponent> LevelObjects { get; private set; } = new List<LevelObjectComponent>();
    }
}
