using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace LevelModule
{
    [CreateAssetMenu(fileName = "SimulationResourceProvider", menuName = "ResourceProviders/SimulationResourceProvider")]
    public class SimulationResourceProvider : SerializedScriptableObject
    {
        [OdinSerialize] private Dictionary<string, LevelObjectComponent> _levelObjects;

        public Dictionary<string, LevelObjectComponent> LevelObjects => _levelObjects;
    }
}