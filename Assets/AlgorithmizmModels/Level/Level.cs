using Assets.AlgorithmizmModels.Level.LevelObjects;
using System.Collections.Generic;

namespace Assets.AlgorithmizmModels.Level
{
    public class Level
    {
        public string Name { get; set; }

        public List<LevelObject> LevelObjects { get; set; }
    }
}
